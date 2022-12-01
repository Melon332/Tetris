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

        [SerializeField] private int stepsPerSecond = 0; //TODO: Use this when we implement the update loop for moving the tile by itself

        private PlayerClass player;

        private List<Tile> CurrentTileSet = new List<Tile>();
        
        [SerializeField] private List<Tile> tilesList;

        [SerializeField] private Transform boardObject;
        
        

        void Start()
        {
            InitalizeTileList();

            tetrisBoard = new Board(colAmount, rowAmount);

            player = FindObjectOfType<PlayerClass>();

            player.PlayerInputClass.OnDownPressed += PlayerInputMoveTilesDown;
            player.PlayerInputClass.OnLeftPressed += PlayerInputMoveTilesSideWays;
            player.PlayerInputClass.OnRightPressed += PlayerInputMoveTilesSideWays;

            tetrisBoard.UpdateGraphics += UpdateTileSetPosition;
            
            tetrisBoard.GotBlocked += ClearRunTimeTileSet;
            tetrisBoard.GotBlocked += SpawnTileSet;
            tetrisBoard.UpdateRunTimeList += RemoveElementFromRunTimeList;

            SpawnTileSet();
            
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
            int randomTile = Random.Range(0, tilesList.Count - 1);
            var tile = Instantiate(tilesList[randomTile]);
            var gameObject = tile.gameObject;
            gameObject.name = $"tile[{posY}][{posX}]";
            gameObject.transform.transform.position = new Vector3(pos.x, pos.y, 0);
            runtimeTileList[posY][posX] = tile;
        }

        private void SpawnTileSet()
        {
            if (CurrentTileSet.Any()) return;
            //Everytime we instaniate a new tile, we should set the y axis to zero as
            //All tiles have different y values
            if (StartPosition == null)
            {
                Debug.LogWarning("You forgot to add a start location for the tiles! Exiting...");
                return;
            }
            eShape selectedShape = eShape.eShapeMAX;
            //Initalize the tile to start from the left top
            for (int i = 0; i < 4; i++)
            {
                var tile = Instantiate(GetRandomizedTile(), boardObject);
                switch (tile.selectedShape)
                {
                    case eShape.eIShape:                
                        tile.Init();
                        tile.SetPosition(StartPosition.transform.position.x, StartPosition.transform.position.y - i);
                        selectedShape = tile.selectedShape;
                        break;
                }
                CurrentTileSet.Add(tile);
            }
            
            tetrisBoard.SpawnDataTile(selectedShape);
        }

        private void UpdateTileSetPosition(List<TilesData> tilesData)
        {
            if (!CurrentTileSet.Any()) return;
            for (int i = 0; i < tilesData.Count; i++)
            {
                CurrentTileSet[i].SetTileData(tilesData[i]);
                CurrentTileSet[i].SetPosition(StartPosition.transform.position.x + tilesData[i].posX, StartPosition.transform.position.y - tilesData[i].posY);
                UpdateRunTimeList(tilesData[i].posX, tilesData[i].posY, CurrentTileSet[i]);
            }
        }

        private void ClearRunTimeTileSet()
        {
            CurrentTileSet.Clear();
            CheckRowPairs();
            MoveTilesDown();
        }

        private void UpdateRunTimeList(int posX, int posY, Tile tile)
        {
            runtimeTileList[posY][posX] = tile;
        }

        private void InitalizeTileList()
        {
            runtimeTileList = new List<Tile[]>();
            for (int i = 0; i < rowAmount; i++)
            {
                runtimeTileList.Add(new Tile[colAmount]);
            }
        }

        private void PlayerInputMoveTilesDown()
        {
            tetrisBoard.PlayerInputMoveDown.Invoke();
        }

        private void PlayerInputMoveTilesSideWays(EMoveTiles eMoveTiles)
        {
            tetrisBoard.PlayerInputMoveSideWays?.Invoke(eMoveTiles);
        }

        [ContextMenu("Print data")]
        private void TestMethod()
        {
            tetrisBoard.PrintAllDataFromArray();
        }

        private bool CheckRowPairs()
        {
            return tetrisBoard.CheckForRowPair();
        }

        private void RemoveElementFromRunTimeList(int posY, int posX)
        {
            Destroy(runtimeTileList[posY][posX].gameObject);
            runtimeTileList[posY][posX] = null;
        }

        /// <summary>
        /// Moves tiles down after a successful row pair
        /// </summary>
        /// <param name="tileRowBroken"></param>
        private void MoveTilesDown()
        {
            if (!CheckRowPairs()) return;
            tetrisBoard.MoveAllTilesDownOneStep();
            MoveTiles();
            
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

        private Tile GetRandomizedTile()
        {
            if (!tilesList.Any())
            {
                Debug.LogError("You need atleast one tile in the tile list to be able to generate a tile!");
                return null;
            }
            int randomizedTileInt = Random.Range(0, tilesList.Count - 1);
            return tilesList[randomizedTileInt];
        }

        private void OnDisable()
        {
            player.PlayerInputClass.OnDownPressed -= PlayerInputMoveTilesDown;
            player.PlayerInputClass.OnLeftPressed-= PlayerInputMoveTilesSideWays;
            player.PlayerInputClass.OnRightPressed -= PlayerInputMoveTilesSideWays;

            tetrisBoard.UpdateGraphics -= UpdateTileSetPosition;
            tetrisBoard.GotBlocked -= ClearRunTimeTileSet;
            tetrisBoard.GotBlocked -= SpawnTileSet;

        }
    }
}
