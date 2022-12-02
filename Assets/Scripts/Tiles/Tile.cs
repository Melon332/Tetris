using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        private Transform tileTransform;
        public eShape selectedShape;
        private TilesData tileData;

        public void Init()
        {
            tileTransform = GetComponent<Transform>();
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
