using UnityEngine;

namespace ShootEmUp
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private void Awake()
        {
            player.OnHealthEmpty += Lose;
        }

        private void OnDestroy()
        {
            player.OnHealthEmpty -= Lose;
        }

        private void Lose()
        {
            Time.timeScale = 0;
        }
    }
}