using UnityEngine;

namespace SO2RTweaks
{
    internal static class Utils
    {
        public static float SquareMagnitudeBetween(Vector3 position1, Vector3 position2)
        {
            return (position1 - position2).sqrMagnitude;
        }
    }
}
