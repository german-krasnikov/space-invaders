using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletController : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;
        [SerializeField]
        private Transform _spawnContainer;
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField]
        private Transform _poolContainer;

        private readonly List<Bullet> _activeBullets = new();
        private readonly Queue<Bullet> _bulletPool = new();

        public void SpawnBullet(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            if (_bulletPool.TryDequeue(out var bullet))
            {
                bullet.SetParent(_spawnContainer);
            }
            else
            {
                bullet = Instantiate(_bulletPrefab, _spawnContainer);
            }

            bullet.StartMoving(position, color, physicsLayer, damage, isPlayer, velocity);
            bullet.OnCollisionEntered += OnBulletCollision;
            _activeBullets.Add(bullet);
        }

        private void Awake()
        {
            for (var i = 0; i < 10; i++)
            {
                var bullet = Instantiate(_bulletPrefab, _poolContainer);
                _bulletPool.Enqueue(bullet);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0, count = _activeBullets.Count; i < count; i++)
            {
                var bullet = _activeBullets[i];

                if (!_levelBounds.InBounds(bullet.Position))
                {
                    RemoveBullet(bullet);
                }
            }
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            DealDamage(bullet, collision.gameObject);
            RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            _activeBullets.Remove(bullet);
            bullet.OnCollisionEntered -= OnBulletCollision;
            bullet.SetParent(_poolContainer);
            _bulletPool.Enqueue(bullet);
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
                if (bullet.IsPlayer != enemy.isPlayer && enemy.health > 0)
                {
                    enemy.health = Mathf.Max(0, enemy.health - damage);
                }
            }
        }
    }
}