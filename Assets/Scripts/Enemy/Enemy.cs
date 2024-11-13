using System;
using UnityEngine;
using UnityEngine.Serialization;

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
                //Attack:
                if (Target.Health <= 0)
                    return;

                _currentTime -= Time.fixedDeltaTime;

                if (_currentTime <= 0)
                {
                    Vector2 startPosition = FirePoint.position;
                    Vector2 vector = (Vector2)Target.transform.position - startPosition;
                    Vector2 direction = vector.normalized;
                    OnFire?.Invoke(startPosition, direction);

                    _currentTime += _countdown;
                }
            }
            else
            {
                //Move:
                Vector2 vector = _destination - (Vector2)transform.position;

                if (vector.magnitude <= 0.25f)
                {
                    _isPointReached = true;
                    return;
                }

                Vector2 direction = vector.normalized * Time.fixedDeltaTime;
                Vector2 nextPosition = Rigidbody.position + direction * Speed;
                Rigidbody.MovePosition(nextPosition);
            }
        }
    }
}