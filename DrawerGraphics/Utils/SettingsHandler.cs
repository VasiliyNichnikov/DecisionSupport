using System.Collections.Generic;
using System.Drawing;
using DecisionSupport.PAM;
using DrawerGraphics.Charts;
using ZedGraph;

namespace DrawerGraphics.Utils
{
    public static class SettingsHandler
    {
        public static List<LineItem> CreateCurveWithStyle(this GraphPane pane, ChartSettings settings,
            List<PointData> points)
        {
            pane.CurveList.Clear();
            GeneratorColors.GenerateColors(DataPAM.NumberClusters);
            
            var curveColor = Color.Black;

            if (settings.Title != null)
            {
                pane.Title.Text = settings.Title;
            }

            if (settings.CurveColor != null)
            {
                curveColor = settings.CurveColor.Value;
            }

            var curves = new List<LineItem>();
            foreach (var point in points)
            {
                var label = point.Type == PointType.Default ? "Point" : "CenterPoint"; 
                var symbolType = point.Type == PointType.Default ? SymbolType.Circle : SymbolType.Diamond; 
                
                var curve = pane.AddCurve(
                    label,
                    new double[] { point.Vector.X }, 
                    new double[] { point.Vector.Y },
                    curveColor, 
                    symbolType);

                if (settings.LineIsVisible != null)
                {
                    curve.Line.IsVisible = settings.LineIsVisible.Value;
                }
                
                curve.Symbol.Fill.Color = GeneratorColors.UsedColors[point.ClusterId];

                if (settings.SymbolFillType != null)
                {
                    curve.Symbol.Fill.Type = settings.SymbolFillType.Value;
                }

                if (settings.SymbolSize != null)
                {
                    curve.Symbol.Size = settings.SymbolSize.Value;
                }

                curves.Add(curve);
            }


            return curves;
        }
    }
}