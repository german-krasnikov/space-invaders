using UnityEngine;

namespace ShootEmUp
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [SerializeField]
        public bool IsPlayer;
        [SerializeField]
        public Transform FirePoint;
        [SerializeField]
        public int Health;
        [SerializeField]
        public Rigidbody2D Rigidbody;
        [SerializeField]
        public float Speed;

        public void DealDamage(int damage, bool spawnByPlayer)
        {
            if (spawnByPlayer == IsPlayer || Health <= 0)
            {
                return;
            }

            Health = Mathf.Max(0, Health - damage);
            AfterDealDamage(damage);
        }

        public void MoveByDirection(Vector2 direction)
        {
            var moveStep = direction * Time.fixedDeltaTime * Speed;
            var targetPosition = Rigidbody.position + moveStep;
            Rigidbody.MovePosition(targetPosition);
        }

        protected virtual void AfterDealDamage(int damage)
        {
        }
    }
}