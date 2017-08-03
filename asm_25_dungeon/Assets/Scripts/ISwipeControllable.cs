using UnityEngine;

namespace Asm
{
    internal interface ISwipeControllable
    {
        void TouchEvent(Vector2 totalDelta);
    }
}