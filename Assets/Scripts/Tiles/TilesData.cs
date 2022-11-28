using UnityEngine;

namespace Tiles
{
    public enum eShape
    {
        eLShape = 0,
        eOShape = 1,
        eIShape = 2,
        eTShape = 3,
        eSShape = 4,
        eZShape = 5,
        eJShape = 6
    }
    
    public class TilesData
    {
        public int posX { private set; get; }
        public int posY { private set; get; }

        public eShape shape;

        public TilesData(eShape _shape, int _posX, int _posY)
        {
            posX = _posX;
            posY = _posY;
            shape = _shape;
        }
    }
}
