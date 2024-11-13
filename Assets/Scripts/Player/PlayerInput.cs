using System;
using UnityEngine;

namespace ShootEmUp
{
    public class PlayerInput : MonoBehaviour
    {
        public event Action OnFire;
        public int MoveDirection => _moveDirection;
        private int _moveDirection;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                OnFire?.Invoke();
            if (Input.GetKey(KeyCode.LeftArrow))
                _moveDirection = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                _moveDirection = 1;
            else
                _moveDirection = 0;
        }
    }
}