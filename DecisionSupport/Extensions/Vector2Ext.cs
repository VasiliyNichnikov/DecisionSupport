using System;
using DecisionSupport.Utils;

namespace DecisionSupport.Extensions
{
    public static class Vector2Ext
    {
        public static Vector2 Abs(this Vector2 vector2)
        {
            var absX = Math.Abs(vector2.X);
            var absY = Math.Abs(vector2.Y);
            return new Vector2(absX, absY);
        }
    }
}