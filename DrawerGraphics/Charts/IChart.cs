namespace DrawerGraphics.Charts
{
    public interface IChart
    {
        int XMin { get; }
        int XMax { get; }
        
        int YMin { get; }
        int YMax { get; }
        
        int CountPoints { get; }
    }
}