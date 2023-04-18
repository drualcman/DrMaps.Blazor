namespace DrMaps.Blazor.Entities;
public class DraggedEventArgs
{
    public int MarkerId { get; set; }
    public LatLong Point { get; set; }

    public DraggedEventArgs(int markerId, LatLong point)
    {
        MarkerId = markerId;
        Point = point;
    }
}
