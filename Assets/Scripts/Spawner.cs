using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public sealed class Spawner : MonoBehaviour
    {
        public GameObject[] _enemies;

        public int _rate = 100;

        public float _radius;

        private int _rateCd;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private void OnEnable()
        {
            foreach (Transform t in transform)
                Destroy(t.gameObject);
            _rateCd = 0;
        }

        private void Spawn()
        {
            var a = Random.Range(0, Mathf.PI * 2);
            var o = _enemies[Random.Range(0, _enemies.Length)];
            var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
            var e = Instantiate(o, transform);
            e.transform.localPosition = d * _radius;
            e.transform.localRotation = Quaternion.LookRotation(-d, Vector3.up);
        }

        private void FixedUpdate()
        {
            if (++_rateCd >= _rate)
            {
                _rateCd = 0;
                Spawn();
            }
        }
    }
}