using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> OnHit;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        private bool _isPlayer;
        private int _damage;
        public void SetParent(Transform parent) => transform.parent = parent;
        public Vector3 Position => transform.position;

        public void StartMoving(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
            _spriteRenderer.color = color;
            transform.position = position;
            gameObject.layer = physicsLayer;
            _damage = damage;
            _isPlayer = isPlayer;
        }

        private void DealDamage(GameObject target)
        {
            if (_damage <= 0)
            {
                return;
            }

            if (target.TryGetComponent(out BaseUnit unit))
            {
                unit.DealDamage(_damage, _isPlayer);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DealDamage(collision.gameObject);
            OnHit?.Invoke(this);
        }
    }
}