using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupport.Core;
using DecisionSupport.Extensions;
using DecisionSupport.Utils;

namespace DecisionSupport.PAM
{
    public class Algorithm
    {
        private struct PointAndDistance
        {
            public readonly Vector2 Point;
            public readonly float Distance;

            public PointAndDistance(Vector2 point, float distance)
            {
                Point = point;
                Distance = distance;
            }
        }
        
        private readonly List<Vector2> _medoids = new List<Vector2>();
        private readonly List<Cluster> _clusters = new List<Cluster>();

        private readonly List<Vector2> _set = new List<Vector2>()
        {
            new Vector2(38, 40),
            new Vector2(24, 36),
            new Vector2(25, 47),
            new Vector2(38, 43),
            new Vector2(34, 4),
            new Vector2(24, 40),
            new Vector2(31, 25),
            new Vector2(39, 17),
            new Vector2(28, 3),
            new Vector2(35, 35),

            new Vector2(30, 43),
            new Vector2(37, 24),
            new Vector2(13, 17),
            new Vector2(24, 2),
            new Vector2(13, 22),
            new Vector2(11, 44),
            new Vector2(29, 31),
            new Vector2(26, 34),
            new Vector2(39, 15),
            new Vector2(45, 10)
        };

        public IEnumerable<ReadOnlyCollection<Cluster>> GetClusters()
        {
            Build();
            CalculateClusters();
            yield return _clusters.AsReadOnly();
            Swap();

        }

        private void Build()
        {
            // Поиск первого медоида
            var c1 = FindFirstMedoid();
            _medoids.Add(c1);
            _set.Remove(c1);

            // Поиск последующих медоидов
            for (var i = 0; i < DataPAM.NumberClusters - 1; i++)
            {
                var ci = FindMedoid();
                if (!_medoids.Contains(ci))
                {
                    _medoids.Add(ci);
                }

                if (_set.Contains(ci))
                {
                    _set.Remove(ci);
                }

                Console.WriteLine($"Number medoids: {_medoids.Count}; Number set: {_set.Count}");
            }

            Console.WriteLine("[Build end]");
        }

        /// <summary>
        /// Первым делом находим первый медоид
        ///
        /// В качестве первого медоида выбирается объект, сумма расстояний от которого
        /// до всех остальных объектов является наименьшей
        /// </summary>
        private Vector2 FindFirstMedoid()
        {
            var result = Vector2.Zero;
            var minDistance = float.MaxValue;

            for (var i = 0; i < _set.Count; i++)
            {
                var sumDistance = .0f;
                var vector1 = _set[i];

                for (var j = 0; j <_set.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var vector2 = _set[j];
                    var distance = Vector2.EuclideanDistance(vector1, vector2);
                    sumDistance += distance;
                }

                if (sumDistance < minDistance)
                {
                    result = vector1;
                    minDistance = sumDistance;
                }
            }

            return result.Abs();
        }
        
        /// <summary>
        /// Находим первый медоид
        /// </summary>
        private Vector2 FindMedoid()
        {
            var result = Vector2.Zero;
            var minDistance = float.MaxValue;

            for (var i = 0; i < _set.Count; i++)
            {
                var vector1 = _set[i];
                var sumDistance = 0.0f;
                
                for (var j = 0; j < _set.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var vector2 = _set[j];
                    var nearestMedoid = GetNearestMedoid(vector1);
                    var distanceBetweeenV1andV2 = Vector2.EuclideanDistance(vector1, vector2);

                    var minimumVector =
                        GetMinimumVectorUsingDistance(nearestMedoid, (vector1, distanceBetweeenV1andV2));
                    sumDistance += minimumVector.distance;

                    // if (minimumVector.distance < minDistance)
                    // {
                    //     minDistance = minimumVector.distance;
                    //     result = minimumVector.vector;
                    // }
                }
                if (sumDistance < minDistance)
                {
                    minDistance = sumDistance;
                    result = vector1;
                }
            }
            return result.Abs();
        }

        private (Vector2 vector, float distance) GetMinimumVectorUsingDistance((Vector2 vector, float distance) a,
            (Vector2 vector, float distance) b)
        {
            var minDistance = Math.Min(a.distance, b.distance);
            return Math.Abs(minDistance - a.distance) < 0.001f ? a : b;
        }

        /// <summary>
        /// Находим ближайший медоид для выбранной точки
        /// </summary>
        /// <returns></returns>
        private (Vector2 medoid, float distance) GetNearestMedoid(Vector2 point)
        {
            var result = Core.Utils.CompareDataAndFindSmallest(point, Vector2.EuclideanDistance, _medoids.AsReadOnly());
            return result;
        }
        
        
        private void Swap()
        {
            var tMin = .0f;
            var maxNumberOfCycles = 10000;
            do
            {
                var values = FindTMin();
                tMin = values.tMin;

                var cMin = values.cMin;
                var xMin = values.xMin;

                _set.Remove(xMin);
                _set.Add(cMin);

                _medoids.Remove(cMin);
                _medoids.Add(xMin);

                Console.WriteLine($"Tmin: {tMin}. Cmin: {cMin}. Xmin: {xMin}");
                maxNumberOfCycles--;
            } while (tMin < 0.0f && maxNumberOfCycles > 0);
            Console.WriteLine("[Swap end]");
        }

        private (float tMin, Vector2 cMin, Vector2 xMin) FindTMin()
        {
            var setD = CalculateSetD();
            var setS = CalculateSetS();

            var tMin = float.MaxValue;
            var cMin = Vector2.Zero;
            var xMin = Vector2.Zero;

            foreach (var ci in _medoids)
            {
                foreach (var item in _set)
                {
                    var tIH = 0.0f;
                    var xh = item;
                    for (var j = 0; j < _set.Count; j++)
                    {
                        var xj = _set[j];
                        var sj = setS[j];
                        var dj = setD[j];

                        var q = NonMedoidContributionToCalculation(xj, ci, xh, dj, sj);
                        tIH += q;
                    }
                    
                    if (tIH < tMin)
                    {
                        tMin = tIH;
                        cMin = ci.Abs();
                        xMin = xh.Abs();
                    }
                }
            }

            return (tMin, cMin, xMin);
        }

        /// <summary>
        /// Рассчитываем вклад не медода в вычисление
        /// </summary>
        private float NonMedoidContributionToCalculation(Vector2 xj, Vector2 ci, Vector2 xh, float dj, float sj)
        {
            var q = .0f;
            if (Vector2.EuclideanDistance(xj, ci) > dj && Vector2.EuclideanDistance(xj, xh) > dj)
            {
                q = 0;
            }
            else if (Math.Abs(Vector2.EuclideanDistance(xj, ci) - dj) < 0.001f)
            {
                var distanceXjandXh = Vector2.EuclideanDistance(xj, xh);
                if (distanceXjandXh < sj)
                {
                    q = distanceXjandXh - dj;
                }
                else
                {
                    q = sj - dj;
                }
            }
            else if (Vector2.EuclideanDistance(xj, xh) < dj)
            {
                q = Vector2.EuclideanDistance(xj, xh) - dj;
            }

            return q;
        }

        /// <summary>
        /// Рассчитываем множество D
        /// Это можество расстояний от каждого объекта до ближайшего медода
        /// </summary>
        private ReadOnlyCollection<float> CalculateSetD()
        {
            var result = new List<float>();
            foreach (var point in _set)
            {
                var medoidsClosestToPoint = ClosestMedoidsToPoint(point);
                var cmin = medoidsClosestToPoint[0];

                var di = Vector2.EuclideanDistance(point, cmin);
                result.Add(di);
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Рассчитываем множество S
        /// Множество расстояний от каждого объекта до второго ближайшего медоида
        /// </summary>
        private ReadOnlyCollection<float> CalculateSetS()
        {
            var result = new List<float>();
            foreach (var point in _set)
            {
                var medoidsClosestToPoint = ClosestMedoidsToPoint(point);
                var cmin2 = medoidsClosestToPoint[1];

                var si = Vector2.EuclideanDistance(point, cmin2);
                result.Add(si);
            }

            return result.AsReadOnly();
        }

        private ReadOnlyCollection<Vector2> ClosestMedoidsToPoint(Vector2 point)
        {
            var medoidsAndDistance = new List<PointAndDistance>();
            foreach (var medoid in _medoids)
            {
                var distance = Vector2.EuclideanDistance(medoid, point);
                medoidsAndDistance.Add(new PointAndDistance(medoid, distance));
            }

            medoidsAndDistance.Sort(SortPointsInDescendingOrder);
            medoidsAndDistance.Reverse();
            return medoidsAndDistance.Select(data => data.Point).ToList().AsReadOnly();
        }
        
        private int SortPointsInDescendingOrder(PointAndDistance dataA, PointAndDistance dataB)
        {
            var distanceA = (int)Math.Round(dataA.Distance);
            var distanceB = (int)Math.Round(dataB.Distance);
            return distanceB - distanceA;
        }

        /// <summary>
        /// Рассчитываем класстеры
        /// </summary>
        private void CalculateClusters()
        {
            _clusters.Clear();
            if (_medoids.Count == 0)
            {
                Console.WriteLine("Medoids not initialized");
                return;
            }
            
            foreach (var medoid in _medoids)
            {
                _clusters.Add(new Cluster(medoid));
            }

            Console.WriteLine($"set: {_set.Count}");
            foreach (var point in _set)
            {
                var cluster = GetClusterForPoint(point);
                cluster.AddPoint(point);
            }
        }

        private Cluster GetClusterForPoint(Vector2 point)
        {
            if (_clusters.Count == 0)
            {
                Console.WriteLine("Error.GetClusterForPoint. Number clusters less then 0");
                return null;
            }

            (float nearValue, Cluster cluster) result = new ValueTuple<float, Cluster>(float.MaxValue, null);
            foreach (var cluster in _clusters)
            {
                var nearValue = Vector2.EuclideanDistance(cluster.Center, point);
                if (nearValue < result.nearValue)
                {
                    result.nearValue = nearValue;
                    result.cluster = cluster;
                }
            }
            
            return result.cluster;
        }
    }
}