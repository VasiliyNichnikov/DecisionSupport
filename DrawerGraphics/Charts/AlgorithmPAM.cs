using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DecisionSupport.PAM;
using DrawerGraphics.Utils;
using ZedGraph;

namespace DrawerGraphics.Charts
{
    public class AlgorithmPAM : IChart
    {
        public int XMin => -100;
        public int XMax => 100;
        public int YMin => -100;
        public int YMax => 100;
        public int CountPoints => DataPAM.NumberElements;

        private List<PointData> _points;
        private readonly DecisionSupport.PAM.Algorithm _algorithm;
        
        public AlgorithmPAM()
        {
            _algorithm = new DecisionSupport.PAM.Algorithm();
        }

        public void UpdatePoints()
        {
            _points = new List<PointData>();

            var clusters = _algorithm.GetClusters().FirstOrDefault();

            if (clusters == null)
            {
                Console.WriteLine("AlgorithmPAM.UpdatePoints: clusters null");
                return;
            }

            for (var i = 0; i < clusters.Count; i++)
            {
                var cluster = clusters[i];
                var clusterId = i;
                
                _points.Add(new PointData(PointType.Center, cluster.Center, clusterId));
                foreach (var point in cluster.Points)
                {
                    _points.Add(new PointData(PointType.Default, point, clusterId));
                }
            }
        }

        public ReadOnlyCollection<LineItem> CreateCurves(GraphPane pane)
        {
            return pane.CreateCurveWithStyle(ChartSettings.GetSettingsForAlgorithPAM(), _points).AsReadOnly();
        }
    }
}