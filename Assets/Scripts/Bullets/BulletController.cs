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
        private Pool<Bullet> _pool;

        public void SpawnBullet(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            var bullet = _pool.Get();
            bullet.StartMoving(position, color, physicsLayer, damage, isPlayer, velocity);
        }

        private void Awake()
        {
            _pool = new(10, CreateBullet, OnGetFromPool, OnReleaseToPool);
        }

        private Bullet CreateBullet() => Instantiate(_bulletPrefab, _poolContainer);

        private void OnGetFromPool(Bullet bullet)
        {
            bullet.SetParent(_spawnContainer);
            bullet.OnHit += OnBulletHit;
            _activeBullets.Add(bullet);
        }

        private void OnReleaseToPool(Bullet bullet)
        {
            bullet.OnHit -= OnBulletHit;
            bullet.SetParent(_poolContainer);
            _activeBullets.Remove(bullet);
        }

        private void FixedUpdate()
        {
            for (int i = 0, count = _activeBullets.Count; i < count; i++)
            {
                if (_activeBullets.Count <= i) continue;
                var bullet = _activeBullets[i];

                if (!_levelBounds.InBounds(bullet.Position))
                {
                    RemoveBullet(bullet);
                }
            }
        }

        private void OnBulletHit(Bullet bullet)
        {
            _pool.Release(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            _pool.Release(bullet);
        }
    }
}