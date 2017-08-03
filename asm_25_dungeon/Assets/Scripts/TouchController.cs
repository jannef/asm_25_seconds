using System.Collections.Generic;
using UnityEngine;

namespace Asm
{
    /**
     * NOTE(JanneF): Highly untested class still :)
     */
    public class TouchController : MonoBehaviour
    {
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
                    break;
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
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
                Debug.Log(string.Format("touch with id {0} ended with total delta: {1}", index, totalDelta));
                if (TouchEnded != null)
                {
                    TouchEnded.Invoke(totalDelta);
                }
            }
        }
    }
}
