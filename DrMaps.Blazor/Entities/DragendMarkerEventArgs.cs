namespace DrMaps.Blazor.Entities;
public class DragendMarkerEventArgs : EventArgs
{
    public int MarkerId { get; set; }
    public LatLong Point { get; set; }
}
