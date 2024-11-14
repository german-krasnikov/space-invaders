using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : Ship
    {
        public event Action OnHealthEmpty;

        public void MoveByDirection(Vector2 direction)
        {
            var moveStep = direction * Time.fixedDeltaTime * Speed;
            var targetPosition = Rigidbody.position + moveStep;
            Rigidbody.MovePosition(targetPosition);
        }

        public override void Fire()
        {
            FireBullet(FirePoint.position, FirePoint.rotation * Vector3.up * 3, Color.blue, (int)PhysicsLayer.PLAYER_BULLET);
        }

        protected override void AfterDealDamage(int damage)
        {
            if (Health <= 0)
            {
                OnHealthEmpty?.Invoke();
            }
        }
    }
}