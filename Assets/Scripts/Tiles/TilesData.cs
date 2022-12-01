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
        eJShape = 6,
        eShapeMAX = 7 // default
    }
    
    public class TilesData
    {
        public int posX { set; get; }
        public int posY { set; get; }

        public eShape shape;
        
        public int lengthInX { set; get; }

        public TilesData(eShape _shape, int _posX, int _posY)
        {
            posX = _posX;
            posY = _posY;
            shape = _shape;
            switch (shape)
            {
                case eShape.eIShape:
                    lengthInX = 1;
                    break;
                default: Debug.LogWarning("The shape doesn't exist!");
                    break;
            }
        }

        public void SetPosition(int _posX, int _posY)
        {
            posX = _posX;
            posY = _posY;
        }
    }
}
