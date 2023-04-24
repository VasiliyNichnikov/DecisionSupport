using System.Drawing;
using ZedGraph;

namespace DrawerGraphics.Utils
{
    public struct ChartSettings
    {
        public readonly string Title;
        public readonly SymbolType? SymbolType;
        public readonly Color? CurveColor;
        public readonly Color? PointCenterColor;
        public readonly bool? LineIsVisible;
        public readonly Color? SymbolFillColor;
        public readonly FillType? SymbolFillType;
        public readonly int? SymbolSize;

        public static ChartSettings GetSettingsForAlgorithPAM()
        {
            return new ChartSettings(
                "Алгоритм PAM",
                ZedGraph.SymbolType.Diamond, 
                Color.Blue, 
                Color.Brown,
                false, 
                Color.Blue,
                FillType.Solid,
                7);
        }

        private ChartSettings(string title, SymbolType? symbolType, Color? curveColor, Color? pointCenterColor, bool? lineIsVisible, Color? symbolFillColor, FillType? symbolFillType, int? symbolSize)
        {
            Title = title;
            SymbolType = symbolType;
            CurveColor = curveColor;
            PointCenterColor = pointCenterColor;
            LineIsVisible = lineIsVisible;
            SymbolFillColor = symbolFillColor;
            SymbolFillType = symbolFillType;
            SymbolSize = symbolSize;
        }
    }
}