using Buildings;
using UnityEngine;

namespace Utils
{
    public static class ItemUtils
    {
        public static ItemOrientation GetRelativeOrientation(Vector3 reference, Vector3 other)
        {
            Vector3 diff = reference - other;
            if (diff.x > .01f)  return ItemOrientation.Right;
            else if (diff.x < -.01f) return ItemOrientation.Left;
            else if (diff.y > .01f)  return ItemOrientation.Up;
            else if (diff.y < -.01f) return ItemOrientation.Down;

            Debug.LogError("GetRelativeOrientation: Positions are too close!");
            return ItemOrientation.Down;
        }
    }
}