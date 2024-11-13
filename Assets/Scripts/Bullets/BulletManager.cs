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

        public readonly HashSet<Bullet> m_activeBullets = new();
        public readonly Queue<Bullet> m_bulletPool = new();
        private readonly List<Bullet> m_cache = new();

        private void Awake()
        {
            for (var i = 0; i < 10; i++)
            {
                Bullet bullet = Instantiate(prefab, container);
                m_bulletPool.Enqueue(bullet);
            }
        }

        private void FixedUpdate()
        {
            m_cache.Clear();
            m_cache.AddRange(m_activeBullets);

            for (int i = 0, count = m_cache.Count; i < count; i++)
            {
                Bullet bullet = m_cache[i];
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
            Vector2 velocity
        )
        {
            if (m_bulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(worldTransform);
            }
            else
            {
                bullet = Instantiate(prefab, worldTransform);
            }

            bullet.transform.position = position;
            bullet.spriteRenderer.color = color;
            bullet.gameObject.layer = physicsLayer;
            bullet.damage = damage;
            bullet.isPlayer = isPlayer;
            bullet.rigidbody2D.velocity = velocity;

            if (m_activeBullets.Add(bullet))
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
            if (m_activeBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= OnBulletCollision;
                bullet.transform.SetParent(container);
                m_bulletPool.Enqueue(bullet);
            }
        }

        private void DealDamage(Bullet bullet, GameObject other)
        {
            int damage = bullet.damage;
            if (damage <= 0)
                return;
            
            if (other.TryGetComponent(out Player player))
            {
                if (bullet.isPlayer != player.isPlayer)
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
                if (bullet.isPlayer != enemy.isPlayer)
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