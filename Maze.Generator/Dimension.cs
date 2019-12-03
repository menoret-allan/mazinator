namespace MazeGenerator
{
    public class Dimension
    {
        public ushort X { get; }
        public ushort Y { get; }

        public Dimension(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
    }
}