using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public sealed class Enemy : MonoBehaviour
    {
        [Range(0, 10)]
        public float _minSpeed, _maxSpeed;
        private float _speed;

        public GameObject _onDeath;
        public Animator _animator;

        private float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                _animator.SetFloat("Speed", Speed);
            }
        }

        private void Start()
        {
            Speed = Random.Range(_minSpeed, _maxSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            var p = other.GetComponent<Player>();
            if (p != null)
                p.Kill();
        }

        private void FixedUpdate()
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Speed * Time.fixedDeltaTime);
        }

        public void Kill()
        {
            if (_onDeath)
                Instantiate(_onDeath, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}