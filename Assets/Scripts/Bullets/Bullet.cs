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
        public SpriteRenderer SpriteRenderer;
        [NonSerialized]
        public bool IsPlayer;
        [NonSerialized]
        public int Damage;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this, collision);
        }
    }
}