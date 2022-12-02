using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        private TilesData tileData;

        public void Init()
        {
            
        }
        public void SetPosition(float posX, float posY)
        {
            transform.position = new Vector3(posX, posY , 0);
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
