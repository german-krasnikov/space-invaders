using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : BaseUnit
    {
        public delegate void FireHandler(Vector2 position, Vector2 direction);
        public event FireHandler OnFire;

        [SerializeField]
        private float _countdown;

        [NonSerialized]
        public Player Target;
        private Vector2 _destination;
        private float _currentTime;
        private bool _isPointReached;

        public void Reset()
        {
            _currentTime = _countdown;
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destination = endPoint;
            _isPointReached = false;
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
                Vector2 startPosition = FirePoint.position;
                var vector = (Vector2)Target.transform.position - startPosition;
                var direction = vector.normalized;
                OnFire?.Invoke(startPosition, direction);

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

            var direction = distance.normalized * Time.fixedDeltaTime;
            var nextPosition = Rigidbody.position + direction * Speed;
            Rigidbody.MovePosition(nextPosition);
        }
    }
}