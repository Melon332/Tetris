using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        private Transform tileTransform;
        public eShape selectedShape;
        private TilesData tileData;

        public int lengthOnYAxis { private set; get; }

        public void Init()
        {
            tileTransform = GetComponent<Transform>();

            switch (selectedShape)
            {
                case eShape.eIShape:
                    lengthOnYAxis = 3;
                    break;
                default: Debug.LogWarning("The specified shape doesn't exist or isn't in the switch case");
                    break;
            }
        }
        public void MoveTile(int posY, int newPosY)
        {
            tileTransform.position = new Vector3(tileTransform.position.x, tileTransform.position.y - posY , 0);
            gameObject.name = $"tile[{tileData.posY}][{tileData.posX}]";
        }

        public void SetPosition(float posX, float posY)
        {
            tileTransform.position = new Vector3(posX, posY , 0);
        }

        public void UpdatePosition()
        {
            if (tileData == null) return;
            tileTransform.position = new Vector3(tileData.posX, tileData.posY , 0);
        }

        public TilesData GetTilesData()
        {
            return tileData;
        }

        public void SetTileData(TilesData data)
        {
            tileData = data;
        }

        [ContextMenu("PrintData")]
        public void PrintTileData()
        {
            Debug.LogWarning($"The tile data is posX: {tileData.posX} and posY: {tileData.posY}");
        }
    }
}
