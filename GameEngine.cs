using System;
using System.Collections.Generic;
using System.Linq;

namespace Blahjong
{
    public class GameEngine
    {
        public static List<Tile> NewGame()
        {
            var tileTemplates = GameEngine.GetTileTemplates();
            var layout = GameEngine.GetLayout(GameEngine.DoublePyramid());
            var tiles = AssignValues(layout, tileTemplates);

            //re-evaluate all tiles!
            return EvaluateFreeTiles(tiles);
        }

        public static List<TileTemplate> GetTileTemplates()
        {
            List<TileTemplate> tileTemplates = new List<TileTemplate>();
            // @* winds *@
            for (var i = 126976; i < 126980; i++)
            {
                tileTemplates.Add(new TileTemplate() { TileType = "Wind", Value = i });
            }

            //@* dragons *@
            for (var i = 126980; i < 126983; i++)
            {
                tileTemplates.Add(new TileTemplate() { TileType = "Dragon", Value = i });
            }

            //@* numerals *@
            for (var i = 126983; i < 126992; i++)
            {
                tileTemplates.Add(new TileTemplate() { TileType = "Numeral", Value = i });
            }

            //@* bamboo *@
            for (var i = 126992; i < 127001; i++)
            {
                tileTemplates.Add(new TileTemplate { TileType = "Bamboo", Value = i });
            }

            //@* circles *@
            for (var i = 127001; i < 127010; i++)
            {
                tileTemplates.Add(new TileTemplate { TileType = "Circles", Value = i });
            }

            //@* flowers *@
            for (var i = 127010; i < 127014; i++)
            {
                tileTemplates.Add(new TileTemplate { TileType = "Flowers", Value = i, MatchOn = MatchOn.TileType });
            }

            //@* seasons *@
            for (var i = 127014; i < 127018; i++)
            {
                tileTemplates.Add(new TileTemplate { TileType = "Seasons", Value = i, MatchOn = MatchOn.TileType });
            }

            return tileTemplates;
        }

        private static List<Tile> AssignValues(List<Tile> tiles, List<TileTemplate> tileTemplates)
        {
            tiles = EvaluateFreeTiles(tiles, (list) => list.Where(t => !t.IsAssigned).ToList());

            TilePair pair = GetRandomFreePair(tiles);
            while (pair != null)
            {
                var usedTiles = tiles
                    .GroupBy(t => new { t.Value, t.MatchOn },
                        (i, g) => new { i.Value, i.MatchOn, Count = g.Count() })
                    .Where(x =>
                        (x.MatchOn == MatchOn.Value && x.Count >= 4)
                        ||
                        (x.MatchOn == MatchOn.TileType && x.Count >= 1))
                    .Select(x => x.Value)
                    .ToList();

                var template = tileTemplates.Where(tt => !usedTiles.Contains(tt.Value)).ToList().TakeRandom();
                //var template = tileTemplates.TakeRandom();

                pair.Tile1.Value = template.Value;
                pair.Tile1.MatchOn = template.MatchOn;
                pair.Tile1.TileType = template.TileType;
                pair.Tile1.IsAssigned = true;

                TileTemplate template2 = null;
                if (template.MatchOn == MatchOn.Value)
                {
                    template2 = template;
                }
                else if (template.MatchOn == MatchOn.TileType)
                {
                    template2 = tileTemplates
                        .Where(t => t.TileType == template.TileType)
                        .Where(t => t.Value != template.Value)
                        .ToList()
                        .TakeRandom();
                }

                pair.Tile2.Value = template2.Value;
                pair.Tile2.MatchOn = template2.MatchOn;
                pair.Tile2.TileType = template2.TileType;
                pair.Tile2.IsAssigned = true;

                tiles = EvaluateFreeTiles(tiles, (tiles) => tiles.Where(t => !t.IsAssigned).ToList());
                pair = GetRandomFreePair(tiles);
            }
            return tiles;
        }

        public static TilePair GetRandomFreePair(List<Tile> tile2)
        {
            var tilePair = tile2
                .Where(t => t.IsFree && !t.IsAssigned)
                .OrderBy(t => Guid.NewGuid())
                .Take(2)
                .ToList();

            if (!tilePair.Any())
            {
                return null;
            }
            else if (tilePair.Count == 1)
            {
                //throw new Exception("mismatch");
                return null;
            }

            return new TilePair { Tile1 = tilePair[0], Tile2 = tilePair[1] };
        }

        public static List<Tile> EvaluateFreeTiles(List<Tile> tiles, Func<List<Tile>, List<Tile>> filter = null)
        {
            if (filter == null)
            {
                filter = (t) => t;
            }
            var filteredTiles = filter(tiles);
            foreach (var tile in filteredTiles)
            {
                tile.IsFree = tile.IsFree(filteredTiles);
            }
            return tiles;
        }

        public static List<Tile> GetLayout(List<string> layers)
        {
            var tiles = new List<Tile>();
            for (var z = 0; z < layers.Count; z++)
            {
                var rows = layers[z]
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
                for (var y = 0; y < rows.Count; y++)
                {
                    var items = rows[y].ToCharArray();
                    for (var x = 0; x < items.Length; x++)
                    {
                        var item = items[x];
                        if (item == '0')
                        {
                            continue;
                        }
                        else if (item == 'X')
                        {
                            tiles.Add(new Tile { Coordinate = new Coordinate(x, y, z) });
                        }
                        else if (item == '↓')
                        {
                            tiles.Add(new Tile { Coordinate = new Coordinate(x, y + .5, z) });
                        }
                        else if (item == '↘')
                        {
                            tiles.Add(new Tile { Coordinate = new Coordinate(x + .5, y + .5, z) });
                        }
                        else if (item == '→')
                        {
                            tiles.Add(new Tile { Coordinate = new Coordinate(x + .5, y, z) });
                        }

                    }
                }
            }
            return tiles;
        }

        public static List<string> Turtle() => new[]
        {
            @"
0XXXXXXXXXXXX00
000XXXXXXXX0000
00XXXXXXXXXX000
↓XXXXXXXXXXXX↓↓
0XXXXXXXXXXXX00
00XXXXXXXXXX000
000XXXXXXXX0000
0XXXXXXXXXXXX00",
            @"
000000000000000
0000XXXXXX00000
0000XXXXXX00000
0000XXXXXX00000
0000XXXXXX00000
0000XXXXXX00000
0000XXXXXX00000
000000000000000
",
            @"
000000000000000
000000000000000
00000XXXX000000
00000XXXX000000
00000XXXX000000
00000XXXX000000
000000000000000
000000000000000
",
            @"
000000000000000
000000000000000
000000000000000
000000XX0000000
000000XX0000000
000000000000000
000000000000000
000000000000000
",
            @"
000000000000000
000000000000000
000000000000000
000000↘00000000
000000000000000
000000000000000
000000000000000
000000000000000
"

        }.ToList();

        public static List<string> DoublePyramid() => new[]
        {
            @"
XXXXXXX0XXXXXXX
XXXXXXX0XXXXXXX
XXXXXXX0XXXXXXX
XXXXXXX0XXXXXXX
XXXXXXX0XXXXXXX",
            @"
0↘↘↘↘0000↘↘↘↘00
↘↘↘↘↘↘00↘↘↘↘↘↘0
↘↘↘↘↘↘00↘↘↘↘↘↘0
0↘↘↘↘0000↘↘↘↘00
000000000000000",
            @"
000000000000000
00XXX00000XXX00
0XXXXX000XXXXX0
00XXX00000XXX00
000000000000000",
            @"
000000000000000
00↘↘000000↘↘000
00↘↘→00000↘↘→00
000000000000000
000000000000000",
            @"
000000000000000
000000000000000
000X0000000X000
000000000000000
000000000000000"



        }.ToList();
    }

}