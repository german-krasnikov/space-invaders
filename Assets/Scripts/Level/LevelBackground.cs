using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBackground : MonoBehaviour
    {
        [SerializeField]
        private float _startPositionY;
        [SerializeField]
        private float _endPositionY;
        [SerializeField]
        private float _movingSpeedY;

        private Vector3 _startPosition;

        private void Awake()
        {
            var position = transform.position;
            _startPosition = new Vector3(position.x, _startPositionY, position.z);
        }

        private void FixedUpdate()
        {
            var y = transform.position.y <= _endPositionY ? _startPositionY : _movingSpeedY * Time.fixedDeltaTime;
            transform.position -= new Vector3(_startPosition.x, y, _startPosition.z);
        }
    }
}