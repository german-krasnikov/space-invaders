using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnPositions;
        [SerializeField]
        private Transform[] attackPositions;
        [SerializeField]
        private Player character;
        [SerializeField]
        private Transform worldTransform;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private Enemy prefab;
        [SerializeField]
        private BulletController bulletController;

        private IterablePool<Enemy> _iterablePool;

        private void Awake()
        {
            _iterablePool = new IterablePool<Enemy>(new Pool<Enemy>(10, OnCreate, OnGetFromPool, OnReleaseToPool));
        }

        private Enemy OnCreate()
        {
            var result = Instantiate(prefab, container);
            result.Construct(bulletController);
            return result;
        }

        private void OnGetFromPool(Enemy enemy) => enemy.SetParent(worldTransform);

        private void OnReleaseToPool(Enemy enemy) => enemy.SetParent(container);

        public void Spawn()
        {
            var enemy = _iterablePool.Get();
            var spawnPosition = RandomPoint(spawnPositions);
            var attackPosition = RandomPoint(attackPositions);
            enemy.StartMovingToPlayer(spawnPosition, attackPosition, character);
        }

        private void FixedUpdate() => _iterablePool.Iterate(OnIterate);

        private void OnIterate(Enemy enemy)
        {
            if (enemy.IsDead)
            {
                _iterablePool.Release(enemy);
            }
        }

        private Transform RandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}