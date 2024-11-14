using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : BaseUnit
    {
        [SerializeField]
        private float _countdown;

        [NonSerialized]
        public Player Target;
        private Vector2 _destination;
        private float _currentTime;
        private bool _isPointReached;

        public void SetParent(Transform parent) => transform.parent = parent;

        public void Reset()
        {
            _currentTime = _countdown;
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destination = endPoint;
            _isPointReached = false;
        }

        public override void Fire()
        {
            Vector2 startPosition = FirePoint.position;
            var vector = (Vector2)Target.transform.position - startPosition;
            var direction = vector.normalized;
            FireBullet(startPosition, direction * 2, Color.red, (int)PhysicsLayer.ENEMY_BULLET);
        }

        public void StartMovingToPlayer(Transform spawnPosition, Transform attackPosition, Player player)
        {
            transform.position = spawnPosition.position;
            SetDestination(attackPosition.position);
            Target = player;
        }

        private void FixedUpdate()
        {
            if (_isPointReached)
            {
                Attack();
            }
            else
            {
                MoveToDestination();
            }
        }

        private void Attack()
        {
            if (Target.Health <= 0)
            {
                return;
            }

            _currentTime -= Time.fixedDeltaTime;

            if (_currentTime <= 0)
            {
                Fire();
                _currentTime += _countdown;
            }
        }

        private void MoveToDestination()
        {
            var distance = _destination - (Vector2)transform.position;

            if (distance.magnitude <= 0.25f)
            {
                _isPointReached = true;
                return;
            }

            var moveDirection = distance.normalized * Time.fixedDeltaTime;
            var nextPosition = Rigidbody.position + moveDirection * Speed;
            Rigidbody.MovePosition(nextPosition);
        }
    }
}