using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private PlayerInput _playerInput;

        private void OnEnable()
        {
            _playerInput.OnFire += player.Fire;
        }

        private void OnDisable()
        {
            _playerInput.OnFire -= player.Fire;
        }

        private void FixedUpdate()
        {
            var moveDirection = new Vector2(_playerInput.MoveDirection, 0);
            player.MoveByDirection(moveDirection);
        }
    }
}