using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        private Transform tileTransform;
        [SerializeField] private eShape selectedShape;
        private TilesData tileData;

        public void Init(int posX, int posY)
        {
            tileData = new TilesData(selectedShape, posX, posY);
            tileTransform = GetComponent<Transform>();
        }

        public void MoveTile(int posY, int newPosY)
        {
            tileTransform.position = new Vector3(tileTransform.position.x, tileTransform.position.y - posY , 0);
            tileData.posX = newPosY;
            gameObject.name = $"tile[{tileData.posX}][{tileData.posY}]";
        }

        public TilesData GetTilesData()
        {
            return tileData;
        }

        [ContextMenu("PrintData")]
        public void PrintTileData()
        {
            Debug.LogWarning($"The tile data is posX: {tileData.posX} and posY: {tileData.posY}");
        }
    }
}
