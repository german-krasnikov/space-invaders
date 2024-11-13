using UnityEngine;

namespace ShootEmUp
{
    public class BaseUnit : MonoBehaviour
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
    }
}