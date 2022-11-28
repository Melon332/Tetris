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
    }
}
