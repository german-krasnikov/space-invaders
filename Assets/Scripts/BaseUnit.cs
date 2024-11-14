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
        [SerializeField]
        private BulletController _bulletController;

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