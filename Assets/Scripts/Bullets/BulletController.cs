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

        private IterablePool<Bullet> _iterablePool;

        public void SpawnBullet(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            var bullet = _iterablePool.Get();
            bullet.StartMoving(position, color, physicsLayer, damage, isPlayer, velocity);
        }

        private void Awake()
        {
            _iterablePool = new IterablePool<Bullet>(new Pool<Bullet>(10, CreateBullet, OnGetFromPool, OnReleaseToPool));
        }

        private Bullet CreateBullet() => Instantiate(_bulletPrefab, _poolContainer);

        private void OnGetFromPool(Bullet bullet)
        {
            bullet.SetParent(_spawnContainer);
            bullet.OnHit += OnBulletHit;
        }

        private void OnReleaseToPool(Bullet bullet)
        {
            bullet.OnHit -= OnBulletHit;
            bullet.SetParent(_poolContainer);
        }

        private void FixedUpdate() => _iterablePool.Iterate(OnIterate);

        private void OnIterate(Bullet bullet)
        {
            if (!_levelBounds.InBounds(bullet.Position))
            {
                RemoveBullet(bullet);
            }
        }

        private void OnBulletHit(Bullet bullet) => RemoveBullet(bullet);

        private void RemoveBullet(Bullet bullet) => _iterablePool.Release(bullet);
    }
}