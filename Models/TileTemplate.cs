namespace Blahjong
{
    public class TileTemplate
    {
        public int Value { get; set; } = 0;
        public MatchOn MatchOn { get; set; } = MatchOn.Value;
        public string TileType { get; set; } = "";
    }
}