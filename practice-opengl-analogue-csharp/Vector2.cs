using System;

namespace practice_opengl_analogue_csharp {
    public class Vector2 {
        public float X;
        public float Y;

        public float SqrMagnitude => X * X + Y * Y;
        public float Magnitude => (float) Math.Sqrt(SqrMagnitude);

        public Vector2(float x, float y) {
            X = x;
            Y = y;
        }

        public static int Distance(Vector2 p1, Vector2 p2) {
            return (int) Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }

        public static Vector2 operator +(Vector2 p0, Vector2 p1) {
            return new Vector2(p0.X+p1.X, p0.Y+p1.Y);
        }
        
        public static Vector2 operator -(Vector2 p0, Vector2 p1) {
            return new Vector2(p0.X-p1.X, p0.Y-p1.Y);
        }
    }
}