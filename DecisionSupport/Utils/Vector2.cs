using System;

namespace DecisionSupport.Utils
{
    public readonly struct Vector2
    {
        public float Y => _y;
        public float X => _x;
        
        private readonly float _x;
        private readonly float _y;

        public static Vector2 Zero => new Vector2(0, 0);
        
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public static float EuclideanDistance(Vector2 a, Vector2 b)
        {
            var x2 = (float)Math.Pow(a._x - b._x, 2);
            var y2 = (float)Math.Pow(a._y - b._y, 2);

            return (float)Math.Sqrt(x2 + y2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2 v2)
            {
                return Math.Abs(_x - v2._x) < 0.001f && Math.Abs(_y - v2._y) < 0.001f;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Vector2({_x}, {_y})";
        }
    }
}