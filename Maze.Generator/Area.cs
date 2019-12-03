namespace MazeGenerator
{
    public class Area
    {
        public Area(ushort x, ushort y, ushort width, ushort height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public ushort X { get; internal set; }
        public ushort Y { get; internal set; }
        public ushort Width { get; internal set; }
        public ushort Height { get; internal set; }
    }
}