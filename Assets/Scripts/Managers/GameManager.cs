using System.Collections.Generic;
using TetrisBoard;
using UnityEngine;
using Tiles;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private Board tetrisBoard;

        [SerializeField] private Transform StartPosition;

        [SerializeField] private int rowAmount;
        [SerializeField] private int colAmount;

        private List<Tile[]> runtimeTileList;

        //TODO: Make this a list afterwards
        [SerializeField] private Tile tilesList;

        public static int YAxisEnd = 0;

        void Start()
        {
            InitalizeTileList();
            int someInt = 1;
            
            tetrisBoard = new Board(colAmount, rowAmount);


            /*
            tetrisBoard.PrintAllDataFromArray();
            CheckRowPairs(out List<int> rowsBroke);


            tetrisBoard.PrintAllDataFromArray();

            MoveTilesDown(rowsBroke);
            tetrisBoard.PrintAllDataFromArray();
            */

            SpawnTileSet(tilesList);
            
            tetrisBoard.PrintAllDataFromArray();
        }


        private void SpawnGrid()
        {
            Vector2 startPos = new Vector2(-4, 0);
            int valueOfGrid = 1;     
            for (int rows = 0; rows < rowAmount; rows++)
            {
                for (int columns = 0; columns < colAmount; columns++)
                {
                    SpawnTile(new Vector2(startPos.x + columns, startPos.y + rows), columns, rows);
                    if (rows == 5 || rows == 2 || rows == 3 || rows == 4 || rows == 6 || rows == 7)
                    {
                        tetrisBoard.MapGridFromUIGrid(columns, rows, valueOfGrid);
                    }
                    else
                    {
                        tetrisBoard.MapGridFromUIGrid(columns, rows, Random.Range(1, 99));
                    }
                }

                valueOfGrid++;
            }
            
        }

        
        /// <summary>
        /// This is for testing purposes.
        /// Will spawn a tile at the specified vector2
        /// and give it the values of posX and posY to store.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        private void SpawnTile(Vector2 pos, int posX, int posY)
        {
            var tile = Instantiate(tilesList);
            var gameObject = tile.gameObject;
            gameObject.name = $"tile[{posY}][{posX}]";
            gameObject.transform.transform.position = new Vector3(pos.x, pos.y, 0);
            runtimeTileList[posY][posX] = tile;
            tile.Init(posX, posY);
        }

        private void SpawnTileSet(Tile tileToSpawn)
        {
            if (StartPosition == null)
            {
                Debug.LogWarning("You forgot to add a start location for the tiles! Exiting...");
                return;
            }
            var tile = Instantiate(tileToSpawn);
            tile.Init(0,0);
            tile.SetPosition(StartPosition.transform.position.x, StartPosition.transform.position.y);
            switch (tile.GetTilesData().shape)
            {
                case eShape.eIShape:
                for (int i = 0; i < 4; i++)
                {
                    tetrisBoard.MapGridFromUIGrid(0, i, 1);
                }

                break;
                default: Debug.LogWarning("Shape does not exist!");
                    break;
            }
            
            List<int> tilesBroken = new List<int>();
            CheckRowPairs(out tilesBroken);
        }

        private void InitalizeTileList()
        {
            runtimeTileList = new List<Tile[]>();
            for (int i = 0; i < rowAmount; i++)
            {
                runtimeTileList.Add(new Tile[colAmount]);
            }
        }

        private void CheckRowPairs(out List<int> tiles)
        {
            tetrisBoard.CheckForRowPair(out List<int> tileRowBroken);

            tiles = tileRowBroken;
            for (int i = 0; i < tileRowBroken.Count; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    Debug.LogWarning("destroyed");
                    Destroy(runtimeTileList[tileRowBroken[i]][j].gameObject);
                    runtimeTileList[tileRowBroken[i]][j] = null;
                }
            }
        }

        private void MoveTilesDown(List<int> tileRowBroken = null)
        {
            if (tileRowBroken == null || tileRowBroken.Count == 0) return;
            //^1 is equal to the end of the list so its basically tileRowBroken.Count - 1
            if (tetrisBoard.CheckIfTileCanMoveDown(tileRowBroken[^1]))
            {
                List<int> rowDeleted = new List<int>();
                tetrisBoard.MoveAllTilesDownOneStep(tileRowBroken.Count, out rowDeleted);

                MoveTiles();
            }
        }

        private void MoveTiles()
        {
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    if (runtimeTileList[i][j] == null) continue;
                    int stepsToMove = tetrisBoard.CalculateStepsTileCanMove(i, j, runtimeTileList);
                    runtimeTileList[i][j].MoveTile(stepsToMove, i - stepsToMove);
                    Debug.LogWarning($"The tile of row: {i} and column {j} moved: {stepsToMove}");
                }
            }
            SortRunTimeTileList();
        }

        private void SortRunTimeTileList()
        {
            for (int i = 1; i < rowAmount; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    Tile currentTile = runtimeTileList[i][j];
                    if(currentTile == null) continue;
                    
                    //Get the tiles data to access the x and y position of the current tile.
                    //They are stored and changed when the tile gets moved.
                    TilesData data = currentTile.GetTilesData();
                    runtimeTileList[data.posY][data.posX] = currentTile;
                    runtimeTileList[i][j] = null;
                }
            }
        }
    }
}
