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

        private readonly HashSet<Enemy> _activeEnemies = new();
        private Pool<Enemy> _pool;

        private void Awake()
        {
            _pool = new(7, OnCreate, OnGetFromPool, OnReleaseToPool);
        }

        private Enemy OnCreate()
        {
            var result = Instantiate(prefab, container);
            result.Construct(bulletController);
            return result;
        }

        private void OnGetFromPool(Enemy enemy)
        {
            enemy.SetParent(worldTransform);
            _activeEnemies.Add(enemy);
        }

        private void OnReleaseToPool(Enemy enemy)
        {
            enemy.SetParent(container);
            _activeEnemies.Remove(enemy);
        }

        public void Spawn()
        {
            var enemy = _pool.Get();
            var spawnPosition = RandomPoint(spawnPositions);
            var attackPosition = RandomPoint(attackPositions);
            enemy.StartMovingToPlayer(spawnPosition, attackPosition, character);
        }

        private void FixedUpdate()
        {
            foreach (var enemy in _activeEnemies.ToArray())
            {
                if (enemy.Health <= 0)
                {
                    _pool.Release(enemy);
                }
            }
        }

        private Transform RandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}