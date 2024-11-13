using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collision2D> OnCollisionEntered;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [NonSerialized]
        public bool IsPlayer;
        [NonSerialized]
        public int Damage;
        public void SetParent(Transform parent) => transform.parent = parent;
        public Vector3 Position => transform.position;

        public void StartMoving(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
            _spriteRenderer.color = color;
            transform.position = position;
            gameObject.layer = physicsLayer;
            Damage = damage;
            IsPlayer = isPlayer;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }
    }
}