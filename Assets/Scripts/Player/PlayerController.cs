using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player character;
        [SerializeField]
        private BulletManager bulletManager;

        private bool _fireRequired;
        private float _moveDirection;

        private void Awake()
        {
            character.OnHealthEmpty += _ => Time.timeScale = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                _fireRequired = true;

            if (Input.GetKey(KeyCode.LeftArrow))
                _moveDirection = -1;
            else if (Input.GetKey(KeyCode.RightArrow))
                _moveDirection = 1;
            else
                _moveDirection = 0;
        }

        private void FixedUpdate()
        {
            if (_fireRequired)
            {
                bulletManager.SpawnBullet(
                    character.firePoint.position,
                    Color.blue,
                    (int) PhysicsLayer.PLAYER_BULLET,
                    1,
                    true,
                    character.firePoint.rotation * Vector3.up * 3
                );

                _fireRequired = false;
            }
            
            var moveDirection = new Vector2(_moveDirection, 0);
            var moveStep = moveDirection * Time.fixedDeltaTime * character.speed;
            var targetPosition = character._rigidbody.position + moveStep;
            character._rigidbody.MovePosition(targetPosition);
        }
    }
}