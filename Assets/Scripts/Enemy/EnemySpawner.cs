using System.Collections;
using UnityEngine;

namespace ShootEmUp
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private EnemyController _enemyController;
        
        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));
                _enemyController.Spawn();
            }
        }
    }
}