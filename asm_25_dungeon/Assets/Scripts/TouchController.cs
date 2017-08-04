using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Asm
{
    /**
     * NOTE(JanneF): Highly untested class still :)
     */
    public class TouchController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _touchParticles;
        [SerializeField] private Text _text;

        public event TouchResolved TouchEnded;
        private readonly Dictionary<int, Vector2> _touches = new Dictionary<int, Vector2>();

        private void Update()
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                TouchToGameObject(i);
            }
        }

        private void TouchToGameObject(int touchIndex)
        {
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(touchIndex).position);
            var touch = Input.touches[touchIndex];
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TouchBegin(touch, touchIndex);
                    break;
                case TouchPhase.Canceled:
                    break;
                case TouchPhase.Ended:
                    TouchEnds(touch, touchIndex);
                    break;
                case TouchPhase.Moved:
                    TouchContinues(touch, touchIndex);
                    break;
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
            }
        }

        private void TouchContinues(Touch touch, int index)
        {
            var endPosition = touch.position;
            var totalDelta = touch.deltaPosition;
            if (_touchParticles != null)
            {
                var normalizedDelta = totalDelta.normalized;
                _touchParticles.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(endPosition.x, endPosition.y, 0f) + Vector3.up * 11f);
                _touchParticles.transform.LookAt(_touchParticles.transform.position - new Vector3(normalizedDelta.x, 0, normalizedDelta.y));

                _text.text = _touchParticles.transform.position.ToString();


                _touchParticles.Emit(5);
            }
        }

        private void TouchBegin(Touch touch, int index)
        {
            if (_touches.ContainsKey(index))
            {
                // something went wrong probs, but lets deal with it like never happened
                _touches[index] = touch.position; // might have to use rawPosition instead
            }
            else
            {
                _touches.Add(index, touch.position);
            }
        }

        private void TouchEnds(Touch touch, int index)
        {
            if (_touches.ContainsKey(index))
            {
                var endPosition = touch.position;
                var totalDelta = endPosition - _touches[index];
                if (TouchEnded != null)
                {
                    TouchEnded.Invoke(totalDelta);
                    if (_touchParticles != null)
                    {
                        var normalizedDelta = totalDelta.normalized;
                        _touchParticles.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(endPosition.x, endPosition.y, 0f) + Vector3.up * 11f);
                        _touchParticles.transform.LookAt(_touchParticles.transform.position + new Vector3(normalizedDelta.x, 0, normalizedDelta.y));

                        _text.text = _touchParticles.transform.position.ToString();


                        _touchParticles.Emit(52);
                    }
                }
            }
        }
    }
}
