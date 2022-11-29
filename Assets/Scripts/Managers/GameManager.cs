using System.Collections.Generic;
using TetrisBoard;
using UnityEngine;
using Tiles;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private Board tetrisBoard;

        [SerializeField] private int rowAmount;
        [SerializeField] private int colAmount;

        private List<Tile[]> runtimeTileList;

        //TODO: Make this a list afterwards
        [SerializeField] private Tile tilesList;

        void Start()
        {
            InitalizeTileList();
            int someInt = 1;
            Vector2 startPos = new Vector2(-4, 0);
            tetrisBoard = new Board(rowAmount, colAmount);

            for (int rows = 0; rows < rowAmount; rows++)
            {
                for (int columns = 0; columns < colAmount; columns++)
                {
                    SpawnGrid(new Vector2(startPos.x + rows, startPos.y + columns), columns, rows);
                    if (rows == 5 || rows == 2 || rows == 3 || rows == 4 || rows == 6 || rows == 7)
                    {
                        tetrisBoard.MapGridToUIGrid(rows, columns, someInt);
                    }
                    else
                    {
                        tetrisBoard.MapGridToUIGrid(rows, columns, Random.Range(1, 99));
                    }
                }

                someInt++;
            }

            tetrisBoard.PrintAllDataFromArray();
            CheckRowPairs(out List<int> rowsBroke);


            tetrisBoard.PrintAllDataFromArray();

            MoveTilesDown(rowsBroke);
            tetrisBoard.PrintAllDataFromArray();
        }

        private void SpawnGrid(Vector2 pos, int posX, int posY)
        {
            var tile = Instantiate(tilesList);
            var gameObject = tile.gameObject;
            gameObject.name = $"tile[{posX}][{posY}]";
            gameObject.transform.transform.position = new Vector3(pos.x, pos.y, 0);
            runtimeTileList[posX][posY] = tile;
            tile.Init(posX, posY);
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
                    runtimeTileList[data.posX][data.posY] = currentTile;
                    runtimeTileList[i][j] = null;
                }
            }
        }
    }
}
