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
            _startPosition = new Vector3(transform.position.x, _startPositionY, transform.position.z);
        }

        private void FixedUpdate()
        {
            if (transform.position.y <= _endPositionY)
            {
                transform.position = new Vector3(_startPosition.x, _startPosition.y, _startPosition.z);
            }

            transform.position -= new Vector3(_startPosition.x, _movingSpeedY * Time.fixedDeltaTime, _startPosition.z);
        }
    }
}