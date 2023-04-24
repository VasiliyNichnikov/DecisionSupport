using DecisionSupport.Utils;

namespace DrawerGraphics.Charts
{
    public enum PointType
    {
        Center,
        Default
    }

    public struct PointData
    {
        public readonly PointType Type;
        public readonly Vector2 Vector;
        public readonly int ClusterId;

        public PointData(PointType type, Vector2 vector, int clusterId)
        {
            Type = type;
            Vector = vector;
            ClusterId = clusterId;
        }
    }
}