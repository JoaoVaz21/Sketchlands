using UnityEngine;

namespace Helpers
{
    public static class MathHelper
    {
        public static Vector2[] Vector3ToVector2(Vector3[] vectors)
        {
            Vector2[] vectors2D = new Vector2[vectors.Length];
            for (var i = 0; i < vectors.Length; i++)
            {
                vectors2D[i] = new Vector2(vectors[i].x, vectors[i].y);
            }

            return vectors2D;

        }
    }
}