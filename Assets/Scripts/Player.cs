using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

namespace Assets
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Player : MonoBehaviour
    {
        public enum ControlInterpretation
        {
            Horizontal,
            Circular
        }

        public float _flashlightAngleCos = Mathf.Cos(Mathf.PI / 3);
        public MobileInput _control;
        public ControlInterpretation _interpretation;
        public UnityEvent _onKill, _onDead;

        public Transform _enemyRegister;

        public float _angularVelocityScale = 0.1f;

        private Rigidbody _rb;
        private float _angle = Mathf.PI;

        public float Angle
        {
            get { return _angle; }
            set { _angle = Mathf.Repeat(value, Mathf.PI * 2); }
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _control.tapped += (start, end, time) => TryKillNearestEnemy();
            _control.moved += UpdateMoved;
        }

        private void UpdateMoved(Vector2 start, Vector2 end)
        {
            var delta = end - start;
            switch (_interpretation)
            {
                case ControlInterpretation.Horizontal:
                    Angle += _angularVelocityScale * delta.x;
                    break;
                case ControlInterpretation.Circular:
                    Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                    var direction = Mathf.Sign(Vector3.Cross(end - start, start - screenPos).z);
                    Angle += _angularVelocityScale * delta.magnitude * direction;
                    break;
            }
        }

        private void TryKillNearestEnemy()
        {
            Enemy enemy = null;
            float deltaMin = float.PositiveInfinity;
            foreach (Transform t in _enemyRegister)
            {
                var e = t.GetComponent<Enemy>();
                if (!e)
                    continue;
                var d = t.position - transform.position;
                var x = d.magnitude;
                if (Vector3.Dot(d / x, transform.forward) < _flashlightAngleCos)
                    continue;
                if (x < deltaMin)
                {
                    deltaMin = x;
                    enemy = e;
                }
            }
            if (enemy)
            {
                enemy.Kill();
                _onKill.Invoke();
            }
        }

        private void FixedUpdate()
        {
            transform.localRotation = Quaternion.AngleAxis(Angle * Mathf.Rad2Deg, Vector3.up);
        }

        public void Kill()
        {
            _onDead.Invoke();
            Destroy(gameObject);
        }
    }
}