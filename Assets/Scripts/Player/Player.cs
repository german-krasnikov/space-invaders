using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : BaseUnit
    {
        public event Action OnHealthEmpty;
        
        public void MoveByDirection(Vector2 direction)
        {
            var moveStep = direction * Time.fixedDeltaTime * Speed;
            var targetPosition = Rigidbody.position + moveStep;
            Rigidbody.MovePosition(targetPosition);
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