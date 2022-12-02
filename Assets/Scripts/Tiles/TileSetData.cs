using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    [Serializable]
    public class TileSetData
    {
        public eShape shape;
        public List<Vector2> tilePosition = new List<Vector2>();

        public List<Vector2> GetTilePositions()
        {
            return tilePosition;
        }

        public Vector2 GetTilePositionAtSpecifiedLocation(int position)
        {
            if (position > tilePosition.Count)
            {
                Debug.LogWarning("The specified data didn't exist in the specified tile position");
                return Vector2.zero;
            }

            return tilePosition[position];
        }

        public int GetTileListCount()
        {
            return tilePosition.Count;
        }

        public eShape GetShape()
        {
            return shape;
        }
    }
}
