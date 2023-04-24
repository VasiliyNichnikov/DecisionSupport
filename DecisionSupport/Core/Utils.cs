using System;
using System.Collections.ObjectModel;

namespace DecisionSupport.Core
{
    public static class Utils
    {
        public static (T vector, float distance) CompareDataAndFindSmallest<T>(T data, Func<T, T, float> compare, ReadOnlyCollection<T> set) where T: struct
        {
            var result = default(T);
            var minDistance = float.MaxValue;

            foreach (var item in set)
            {
                if (data.Equals(item))
                {
                    continue;
                }
                
                var distance = compare(data, item);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = item;
                }
            }

            return (result, minDistance);
        }
    }
}