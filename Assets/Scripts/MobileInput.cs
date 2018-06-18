using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Assets
{
    public sealed class MobileInput : MonoBehaviour
    {
        private class TouchRecordPool
        {
            private Stack<TouchRecord> _pool = new Stack<TouchRecord>();

            public TouchRecordPool()
            {
                const int capacity = 16;
                for (int i = 0; i < capacity; i++)
                    _pool.Push(new TouchRecord());
            }

            public TouchRecord ObtainFor(Touch t)
            {
                if (_pool.Count == 0)
                {
                    var r = new TouchRecord();
                    r.InitWith(t);
                    Debug.LogWarning("Pool has grown");
                    _pool.Push(r);
                    return r;
                }
                else
                {
                    var r = _pool.Pop();
                    r.InitWith(t);
                    return r;
                }
            }

            public void Return(TouchRecord t)
            {
                _pool.Push(t);
            }
        }

        private class TouchRecord
        {
            public Touch _touch;
            public float _totalDuration;
            public Vector2 _origin, _lastPosition;

            public bool _notATap;

            public Vector2 Delta { get { return _touch.position - _lastPosition; } }

            public void UpdateTouch(Touch t)
            {
                _lastPosition = _touch.position;
                _touch = t;
            }

            public void InitWith(Touch t)
            {
                _touch = t;
                _origin = _lastPosition = t.position;
                _totalDuration = 0;
                _notATap = false;
            }
        }

        private TouchRecordPool _touchRecordPool = new TouchRecordPool();

        public float _touchGrace = 0.15f;

        public float _tapMaxSqrMagnitude = 2500;

        public event Action<Vector2> pointerDown, touchBegan;
        public event Action<Vector2, Vector2, float> touchEnded, tapped;
        public event Action<Vector2, Vector2> moved;

        private Dictionary<int, TouchRecord> _trackedTouches = new Dictionary<int, TouchRecord>();

        private void Update()
        {
            var touches = Input.touches;
            foreach (var t in touches)
            {
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        {
                            if (_trackedTouches.ContainsKey(t.fingerId))
                            {
                                Debug.LogWarning("Possibly leaked touch");
                                FinalizeTouch(t);
                            }
                            var touchRecord = _touchRecordPool.ObtainFor(t);
                            _trackedTouches.Add(t.fingerId, touchRecord);
                            PointerDown(touchRecord);
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        FinalizeTouch(t);
                        break;
                }
            }
            // CleanupBuggedTouches(touches);
            foreach (var v in _trackedTouches.Values)
            {
                v._totalDuration += Time.deltaTime;
                v.UpdateTouch(touches.First(s => s.fingerId == v._touch.fingerId));
                bool isNotATap = ((v._totalDuration >= _touchGrace) || (v._origin - v._touch.position).sqrMagnitude >= _tapMaxSqrMagnitude);
                if (!v._notATap && isNotATap)
                {
                    v._notATap = true;
                    TouchBegan(v);
                }
                Move(v);
            }
        }

        private void FinalizeTouch(Touch t)
        {
            if (!_trackedTouches.ContainsKey(t.fingerId))
                return;
            var touchRecord = _trackedTouches[t.fingerId];
            if (!touchRecord._notATap)
                Tap(touchRecord);
            TouchEnded(touchRecord);
            _trackedTouches.Remove(t.fingerId);
            _touchRecordPool.Return(touchRecord);
        }

        private void Move(TouchRecord v)
        {
            var e = moved;
            if (e != null)
                e(v._lastPosition, v._touch.position);
        }

        /// <summary>
        /// We may need this on iOS.
        /// </summary>
        private void CleanupBuggedTouches(Touch[] touches)
        {
            List<int> toRemoveKeys = new List<int>();
            foreach (var key in _trackedTouches.Keys)
            {
                if (!touches.Any(s => s.fingerId == key))
                    toRemoveKeys.Add(key);
            }
            foreach (var key in toRemoveKeys)
                _trackedTouches.Remove(key);
        }

        private void Tap(TouchRecord t)
        {
            var e = tapped;
            if (e != null)
                e(t._origin, Input.mousePosition, t._totalDuration);
        }

        private void PointerDown(TouchRecord t)
        {
            var e = pointerDown;
            if (e != null)
                e(t._touch.position);
        }

        private void TouchBegan(TouchRecord t)
        {
            var e = touchBegan;
            if (e != null)
                e(t._touch.position);
        }

        private void TouchEnded(TouchRecord t)
        {
            var e = touchEnded;
            if (e != null)
                e(t._origin, Input.mousePosition, t._totalDuration);
        }
    }
}