namespace Blahjong
{
    public class Tile
    {
        public Coordinate Coordinate { get; set; }

        public double X => Coordinate.X;
        public double Y => Coordinate.Y;
        public double Z => Coordinate.Z;

        public double LeftEdge => X;
        public double RightEdge => X + 1;
        public double TopEdge => Y;
        public double BottomEdge => Y + 1;

        public string cssClass => $"tile {(IsSelected ? "selected" : (IsFree ? "free" : "locked"))}";

        public int Value = 0;
        public MatchOn MatchOn = MatchOn.Value;
        public string TileType = "";

        public bool IsFree { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMatched { get; set; }
    }
}