using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Asm
{
    /**
     * TODO(JanneF): This goes to the thing that should be moved by a touch controller. Untested as always.
     */
    class SwipeControllable : MonoBehaviour, ISwipeControllable
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _swipeTreshold;
        [SerializeField] private TouchController _controller;
        [SerializeField] private Player _controlled;
        private void Awake()
        {
            if (_controller == null)
            {
                _controller = FindObjectOfType<TouchController>();
                if (_controller == null)
                {
                    throw new UnityException("No touch controller in the scene");
                }
                else
                {
                    Debug.LogWarning("Touch controller found using FindObjectOFType. COnsider configuring controller for reporting SwipeControllable manually.");
                }
            }

            if (_controlled == null)
            {
                _controlled = GetComponent<Player>();
                if (_controlled == null)
                {
                    throw new UnityException("No Player in this object");
                }
                else
                {
                    Debug.LogWarning("Player found using GetComponent. Please configure player for this SwipeControllable manually. Prefab might be broken.");
                }
            }
        }

        private void Start()
        {
            if (_controller != null)
            {
                _controller.TouchEnded += TouchEvent;
            }
        }

        public void TouchEvent(Vector2 totalDelta)
        {
            // TODO(JanneF): Character movement is handled here
            // TODO(JanneF): No idea if the directions are right... need to check
            if (Mathf.Abs(totalDelta.x) > Mathf.Abs(totalDelta.y))
            {
                // x axis is longer
                if (totalDelta.x > _swipeTreshold)
                {
                    if (_controlled != null) { _controlled.Move(1, 0); }
                } else if (totalDelta.x < -_swipeTreshold)
                {
                    if (_controlled != null) { _controlled.Move(-1, 0); }
                }
            }
            else
            {
                // x axis is longer
                if (totalDelta.y > _swipeTreshold)
                {
                    if (_controlled != null) { _controlled.Move(0, 1); }
                }
                else if (totalDelta.y < -_swipeTreshold)
                {
                    if (_controlled != null) { _controlled.Move(0, -1); }
                }
            }
        }
    }
}
