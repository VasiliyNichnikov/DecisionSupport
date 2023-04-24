using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DecisionSupport.Utils;

namespace DecisionSupport.Core
{
    public class Cluster
    {
        public ReadOnlyCollection<Vector2> Points => _points.AsReadOnly();

        public Vector2 Center => _center;

        private Vector2 _center;
        private readonly List<Vector2> _points;

        public Cluster(Vector2 center)
        {
            _center = center;
            _points = new List<Vector2>();
        }

        /// <summary>
        /// Рассчитываем критерий сходимости
        /// </summary>
        /// <returns></returns>
        public float GetConvergenceCriterion()
        {
            var result = 0.0f;
            foreach (var point in _points)
            {
                var differenceX = (float)Math.Pow(point.X - _center.X, 2);
                var differenceY = (float)Math.Pow(point.Y - _center.Y, 2);
                result += differenceX + differenceY;
            }
            
            return result;
        }

        public void AddPoint(Vector2 point)
        {
            _points.Add(point);
        }

        /// <summary>
        /// Рассчитываем центр кластера
        /// После расчета центра точки текущего кластера очищаются
        /// </summary>
        public void CalculateCentroid()
        {
            var sumX = 0.0f;
            var sumY = 0.0f;
            foreach (var point in _points)
            {
                sumX += point.X;
                sumY += point.Y;
            }

            var numberPoints = _points.Count;

            _center = new Vector2(sumX / numberPoints, sumY / numberPoints);
            _points.Clear();
        }
    }
}