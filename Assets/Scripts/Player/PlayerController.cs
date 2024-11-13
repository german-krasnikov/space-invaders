using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private PlayerInput _playerInput;
        [SerializeField]
        private BulletController bulletController;

        private void OnEnable()
        {
            _playerInput.OnFire += FireHandler;
        }

        private void OnDisable()
        {
            _playerInput.OnFire -= FireHandler;
        }

        private void FireHandler()
        {
            bulletController.SpawnBullet(
                player.FirePoint.position,
                Color.blue,
                (int)PhysicsLayer.PLAYER_BULLET,
                1,
                true,
                player.FirePoint.rotation * Vector3.up * 3
            );
        }

        private void FixedUpdate()
        {
            var moveDirection = new Vector2(_playerInput.MoveDirection, 0);
            player.MoveByDirection(moveDirection);
        }
    }
}