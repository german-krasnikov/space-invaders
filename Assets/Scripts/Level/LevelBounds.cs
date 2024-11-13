using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBounds : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftTopBorder;
        [SerializeField]
        private Transform _rightBottomBorder;
        
        public bool InBounds(Vector3 position) =>
            position.x > _leftTopBorder.position.x
            && position.x < _rightBottomBorder.position.x
            && position.y > _rightBottomBorder.position.y
            && position.y < _leftTopBorder.position.y;
    }
}