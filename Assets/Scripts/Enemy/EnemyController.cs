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

        private Enemy OnCreate() => Instantiate(prefab, container);

        private void OnGetFromPool(Enemy enemy)
        {
            enemy.transform.SetParent(worldTransform);
            _activeEnemies.Add(enemy);
        }

        private void OnReleaseToPool(Enemy enemy)
        {
            enemy.OnFire -= OnFire;
            enemy.transform.SetParent(container);
            _activeEnemies.Remove(enemy);
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                var enemy = _pool.Get();
                var spawnPosition = RandomPoint(spawnPositions);
                enemy.transform.position = spawnPosition.position;
                var attackPosition = RandomPoint(attackPositions);
                enemy.SetDestination(attackPosition.position);
                enemy.Target = character;

                if (_activeEnemies.Count < 5 && _activeEnemies.Add(enemy))
                {
                    enemy.OnFire += OnFire;
                }
            }
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

        private void OnFire(Vector2 position, Vector2 direction)
        {
            bulletController.SpawnBullet(
                position,
                Color.red,
                (int)PhysicsLayer.ENEMY_BULLET,
                1,
                false,
                direction * 2
            );
        }

        private Transform RandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index];
        }
    }
}