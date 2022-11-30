using System;
using System.Collections.Generic;
using System.Linq;
using TetrisBoard;
using UnityEngine;
using Tiles;
using Player;
using Random = UnityEngine.Random;

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
        
        [SerializeField] private int stepsPerSecond = 0; //TODO: Use this when we implement the update loop for moving the tile by itself

        public static int YAxisEnd = 0;

        private PlayerClass player;

        private List<Tile> CurrentTileSet = new List<Tile>();
        
        

        void Start()
        {
            InitalizeTileList();

            tetrisBoard = new Board(colAmount, rowAmount);

            player = FindObjectOfType<PlayerClass>();

            SpawnTileSet(tilesList);
            
            tetrisBoard.PrintAllDataFromArray();
        }
        /*
        tetrisBoard.PrintAllDataFromArray();
        CheckRowPairs(out List<int> rowsBroke);


        tetrisBoard.PrintAllDataFromArray();

        MoveTilesDown(rowsBroke);
        tetrisBoard.PrintAllDataFromArray();
        */

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
                        tetrisBoard.MapGridFromUIGrid(columns, rows, false);
                    }
                    else
                    {
                        tetrisBoard.MapGridFromUIGrid(columns, rows, true);
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
        }

        private void SpawnTileSet(Tile tileToSpawn)
        {
            //Everytime we instaniate a new tile, we should set the y axis to zero as
            //All tiles have different y values
            YAxisEnd = 0;
            if (StartPosition == null)
            {
                Debug.LogWarning("You forgot to add a start location for the tiles! Exiting...");
                return;
            }
            //Initalize the tile to start from the left top
            for (int i = 0; i < 4; i++)
            {
                var tile = Instantiate(tileToSpawn);
                tile.Init();
                tile.SetPosition(StartPosition.transform.position.x, StartPosition.transform.position.y - i);
                CurrentTileSet.Add(tile);
            }
            tetrisBoard.SpawnDataTile(eShape.eIShape);
            
            InitalizeCurrentTileSet(tetrisBoard.GetCurrentTileSetData());
        }

        private void InitalizeCurrentTileSet(List<TilesData> tilesData)
        {
            if (!CurrentTileSet.Any()) return;
            for (int i = 0; i < tilesData.Count; i++)
            {
                CurrentTileSet[i].SetTileData(tilesData[i]);
                Debug.LogWarning($"Just to make sure it actually worked: posX: {CurrentTileSet[i].GetTilesData().posX} and posY: {CurrentTileSet[i].GetTilesData().posY}");
                CurrentTileSet[i].SetPosition(StartPosition.transform.position.x - CurrentTileSet[i].GetTilesData().posX, StartPosition.transform.position.y - CurrentTileSet[i].GetTilesData().posY );
            }
        }

        private void InitalizeTileList()
        {
            runtimeTileList = new List<Tile[]>();
            for (int i = 0; i < rowAmount; i++)
            {
                runtimeTileList.Add(new Tile[colAmount]);
            }
        }

        [ContextMenu("Print data")]
        private void TestMethod()
        {
            tetrisBoard.PrintAllDataFromArray();
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

        /// <summary>
        /// Moves tiles down after a successful row pair
        /// </summary>
        /// <param name="tileRowBroken"></param>
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
