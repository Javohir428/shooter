using UnityEngine;

namespace CompleteProject
{
    public class EnemyManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's heatlh.
        public float spawnTime = 3f;            // How long between each spawn.
        public float _radius;
        public GameObject[] _enemies;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        void Start ()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating ("Spawn", spawnTime, spawnTime);
        }


        void Spawn ()
        {
            // If the player has no health left...
            if(playerHealth.currentHealth <= 0f)
            {
                // ... exit the function.
                return;
            }
            var a = Random.Range(0, Mathf.PI * 2);
            var o = _enemies[Random.Range(0, _enemies.Length)];
            var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
            var e = Instantiate(o, transform);
            e.transform.localPosition = d * _radius;
            e.transform.localRotation = Quaternion.LookRotation(-d, Vector3.up);
        }
    }
}