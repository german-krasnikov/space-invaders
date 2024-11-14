using UnityEngine;

namespace ShootEmUp
{
    public abstract class Ship : MonoBehaviour
    {
        [SerializeField]
        protected bool IsPlayer;
        [SerializeField]
        protected Transform FirePoint;
        [SerializeField]
        protected int Health;
        [SerializeField]
        protected Rigidbody2D Rigidbody;
        [SerializeField]
        protected float Speed;
        [SerializeField]
        private BulletController _bulletController;
        
        public bool IsDead => Health <= 0;

        public void Construct(BulletController bulletController)
        {
            _bulletController = bulletController;
        }

        public void DealDamage(int damage, bool spawnByPlayer)
        {
            if (spawnByPlayer == IsPlayer || Health <= 0)
            {
                return;
            }

            Health = Mathf.Max(0, Health - damage);
            AfterDealDamage(damage);
        }

        public abstract void Fire();

        protected void FireBullet(Vector2 position, Vector2 velocity, Color color, int physicsLayer)
        {
            _bulletController.SpawnBullet(position, color, physicsLayer, 1, IsPlayer, velocity);
        }

        protected virtual void AfterDealDamage(int damage)
        {
        }
    }
}