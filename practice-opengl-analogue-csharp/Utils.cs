using System.Linq;
using System.Numerics;

namespace practice_opengl_analogue_csharp {
    public struct Bounds3D {
        public readonly Vector3 Center;
        public readonly Vector3 Size;

        public Bounds3D(Vector3 center, Vector3 size) {
            Center = center;
            Size = size;
        }

        public float BiggestSize() {
            return new[] {Size.X, Size.Y, Size.Z}.Max();
        }
    }

    public static class Utils {
        public static float Clamp(float min, float max, float value) {
            return value < min ? min : value > max ? max : value;
        }
    }
}