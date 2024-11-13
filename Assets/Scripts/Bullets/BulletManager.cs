using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField]
        public Bullet prefab;
        [SerializeField]
        public Transform worldTransform;
        [SerializeField]
        private LevelBounds levelBounds;
        [SerializeField]
        private Transform container;

        private readonly HashSet<Bullet> _activeBullets = new();
        private readonly Queue<Bullet> _bulletPool = new();
        private readonly List<Bullet> _cache = new();

        private void Awake()
        {
            for (var i = 0; i < 10; i++)
            {
                var bullet = Instantiate(prefab, container);
                _bulletPool.Enqueue(bullet);
            }
        }

        private void FixedUpdate()
        {
            _cache.Clear();
            _cache.AddRange(_activeBullets);

            for (int i = 0, count = _cache.Count; i < count; i++)
            {
                var bullet = _cache[i];
                if (!levelBounds.InBounds(bullet.transform.position))
                {
                    RemoveBullet(bullet);
                }
            }
        }

        public void SpawnBullet(
            Vector2 position,
            Color color,
            int physicsLayer,
            int damage,
            bool isPlayer,
            Vector2 velocity)
        {
            if (_bulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(worldTransform);
            }
            else
            {
                bullet = Instantiate(prefab, worldTransform);
            }

            bullet.transform.position = position;
            bullet.SpriteRenderer.color = color;
            bullet.gameObject.layer = physicsLayer;
            bullet.Damage = damage;
            bullet.IsPlayer = isPlayer;
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;

            if (_activeBullets.Add(bullet))
            {
                bullet.OnCollisionEntered += OnBulletCollision;
            }
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            DealDamage(bullet, collision.gameObject);
            RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            if (_activeBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= OnBulletCollision;
                bullet.transform.SetParent(container);
                _bulletPool.Enqueue(bullet);
            }
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            var damage = bullet.Damage;
            if (damage <= 0)
                return;
            
            if (other.TryGetComponent(out Player player))
            {
                if (bullet.IsPlayer != player.isPlayer)
                {
                    if (player.health <= 0)
                        return;

                    player.health = Mathf.Max(0, player.health - damage);
                    player.OnHealthChanged?.Invoke(player, player.health);

                    if (player.health <= 0)
                        player.OnHealthEmpty?.Invoke(player);
                }
            }
            else if (other.TryGetComponent(out Enemy enemy))
            {
                if (bullet.IsPlayer != enemy.isPlayer)
                {
                    if (enemy.health > 0)
                    {
                        enemy.health = Mathf.Max(0, enemy.health - damage);
                    }
                }
            }
        }
    }
}