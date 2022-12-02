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

        [SerializeField] private List<TileSetData> allTileSetData = new List<TileSetData>();
        
        

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
                        tetrisBoard.MapTileDataToGrid(columns, rows, false);
                    }
                    else
                    {
                        tetrisBoard.MapTileDataToGrid(columns, rows, true);
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

            var randomizedTileSet = GetTileSetDataSpecifiedWithNumber(2);
            //Initalize the tile to start from the left top
            for (int i = 0; i < randomizedTileSet.GetTileListCount(); i++)
            {
                var tile = Instantiate(GetRandomizedTile(), boardObject);
                tile.Init();
                tile.SetPosition(StartPosition.transform.position.x + randomizedTileSet.GetTilePositionAtSpecifiedLocation(i).x, 
                    StartPosition.transform.position.y - randomizedTileSet.GetTilePositionAtSpecifiedLocation(i).y);
                tetrisBoard.SpawnDataTile(tile.selectedShape, (int)randomizedTileSet.tilePosition[i].x, (int)randomizedTileSet.tilePosition[i].y, i);

                CurrentTileSet.Add(tile);
            }
        }

        private void UpdateTileSetPosition(List<TilesData> tilesData)
        {
            if (!CurrentTileSet.Any()) return;
            for (int i = 0; i < tilesData.Count; i++)
            {
                CurrentTileSet[i].SetTileData(tilesData[i]);
                CurrentTileSet[i].SetPosition(StartPosition.transform.position.x + tilesData[i].posX, StartPosition.transform.position.y - tilesData[i].posY);
            }
        }

        private void ClearRunTimeTileSet()
        {
            for (int i = 0; i < CurrentTileSet.Count; i++)
            {
                UpdateRunTimeList(CurrentTileSet[i].GetTilesData().posX, CurrentTileSet[i].GetTilesData().posY, CurrentTileSet[i]);
            }
            CurrentTileSet.Clear();
            if (tetrisBoard.CheckForRowPair())
            {
                MoveTilesDown();
            }
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
            tetrisBoard.PlayerInputMoveDown?.Invoke();
        }

        private void PlayerInputMoveTilesSideWays(EMoveTiles eMoveTiles)
        {
            tetrisBoard.PlayerInputMoveSideWays?.Invoke(eMoveTiles);
        }

        [ContextMenu("Print data")]
        private void TestMethod()
        {
            tetrisBoard?.PrintAllDataFromArray();
        }

        private void RemoveElementFromRunTimeList(int posY, int posX)
        {
            if (!runtimeTileList[posY][posX])
            {
                Debug.LogWarning("Error!");
                return;
            }
            Destroy(runtimeTileList[posY][posX].gameObject);
            runtimeTileList[posY][posX] = null;
        }

        /// <summary>
        /// Moves tiles down after a successful row pair
        /// </summary>
        private void MoveTilesDown()
        {
            MoveTiles();
        }

        private void MoveTiles()
        {
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    if (runtimeTileList[i][j] == null) continue;
                    int stepsToMove = CalculateStepsTillTileIsAtBottom(i, j);
                    runtimeTileList[i][j].GetTilesData().SetPosition(j, stepsToMove);
                    runtimeTileList[i][j].SetPosition(StartPosition.transform.position.x + runtimeTileList[i][j].GetTilesData().posX, StartPosition.transform.position.y - runtimeTileList[i][j].GetTilesData().posY);
                    //Debug.LogWarning($"The tile of row: {i} and column {j} moved: {stepsToMove}");
                }
            }
            SortRunTimeTileList();
        }
        private int CalculateStepsTillTileIsAtBottom(int rowStartPos, int columnPos)
        {
            if (runtimeTileList[rowStartPos][columnPos] == null)
            {
                Debug.LogWarning("The specified tile is currently empty!");
                return 0;
            }

            int stepsToMove = rowStartPos;
            for (int i = rowStartPos; i < rowAmount; i++)
            {
                if (i + 1 >= rowAmount) break;
                if (runtimeTileList[i + 1][columnPos] == null)
                {
                    stepsToMove++;
                }
            }

            return stepsToMove;
        }

        private void SortRunTimeTileList()
        {
            //Copy all the non zero rows into the temporary list
            for (int i = rowAmount - 1; i > 0; i--)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    if (i + 1 >= rowAmount) break;
                    if (runtimeTileList[i][j] == null) continue;
                    int newPos = CalculateStepsTillTileIsAtBottom(i, j);
                    Debug.Log($"The tile at {i} and {j} was moved to {newPos} and {j}");
                    runtimeTileList[newPos][j] = runtimeTileList[i][j];
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
        
        private TileSetData GetRandomizedTileSetData()
        {
            if (!tilesList.Any())
            {
                Debug.LogError("You need atleast one tile in the tile list to be able to generate a tile!");
                return null;
            }
            int randomizedTileInt = Random.Range(0, allTileSetData.Count - 1);
            return allTileSetData[randomizedTileInt];
        }

        private TileSetData GetTileSetDataSpecifiedWithNumber(int position)
        {
            if (position > allTileSetData.Count) return null;
            return allTileSetData[position];
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
