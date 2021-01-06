namespace Blahjong
{
    public class Coordinate
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Coordinate(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate coordinate)
        {
            return coordinate != null && X.Equals(coordinate.X) && Y.Equals(coordinate.Y) && Z.Equals(coordinate.Z);
        }
    }
}