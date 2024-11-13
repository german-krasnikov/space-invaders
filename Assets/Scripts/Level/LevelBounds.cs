using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBounds : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftBorder;
        [SerializeField]
        private Transform _rightBorder;
        [SerializeField]
        private Transform _downBorder;
        [SerializeField]
        private Transform _topBorder;
        
        public bool InBounds(Vector3 position) =>
            position.x > _leftBorder.position.x
            && position.x < _rightBorder.position.x
            && position.y > _downBorder.position.y
            && position.y < _topBorder.position.y;
    }
}