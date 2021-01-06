using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blahjong
{
    public static class Extensions
    {
        static Random _rand = new Random();

        public static T TakeRandom<T>(this List<T> list) where T : class
        {
            if (list.Count == 0)
            {
                return null;
            }

            return list[_rand.Next(0, list.Count - 1)];
        }

        public static bool Matches(this Tile tile1, Tile tile2)
        {
            if (tile2 != null)
            {

                if (tile1.MatchOn == MatchOn.Value)
                {
                    if (tile2.Value == tile1.Value)
                    {
                        return true;
                    }
                }
                else if (tile1.MatchOn == MatchOn.TileType)
                {
                    if (tile2.TileType == tile1.TileType)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Overlaps(this Tile tile1, Tile tile2)
        {
            //tile1 is Covering tile2

            if (tile1.Z.Equals(tile2.Z + 1))
            {
                //one tile is above another one
                if (tile1.BottomEdge <= tile2.TopEdge || tile2.BottomEdge <= tile1.TopEdge)
                {
                    return false;
                }

                // one tile is to the right of another one
                if (tile1.LeftEdge >= tile2.RightEdge || tile2.LeftEdge >= tile1.RightEdge)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool IsAdjacentLeft(this Tile tile1, Tile tile2)
        {
            if (tile1.Z.Equals(tile2.Z))
            {
                if (tile1.RightEdge.Equals(tile2.LeftEdge))
                {
                    //one tile is above another one
                    if (tile1.BottomEdge <= tile2.TopEdge || tile2.BottomEdge <= tile1.TopEdge)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool IsAdjacentRight(this Tile tile1, Tile tile2)
        {
            if (tile1.Z.Equals(tile2.Z))
            {
                if (tile1.LeftEdge.Equals(tile2.RightEdge))
                {
                    //one tile is above another one
                    if (tile1.BottomEdge <= tile2.TopEdge || tile2.BottomEdge <= tile1.TopEdge)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool IsFree(this Tile tile, List<Tile> tiles)
        {
            bool blockedLeft = false;
            bool blockedRight = false;
            bool overlaps = false;

            foreach (var t in tiles)
            {
                //same tile... skip!
                if (t == tile) //compare by value!
                {
                    continue;
                }

                if (t.Overlaps(tile))
                {
                    overlaps = true;
                }
                else if (t.IsAdjacentLeft(tile))
                {
                    blockedLeft = true;
                }
                else if (t.IsAdjacentRight(tile))
                {
                    blockedRight = true;
                }

                if (overlaps || (blockedLeft && blockedRight))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
