using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tiles;


namespace TetrisBoard
{
    public class Board
    {
        private List<TilesData> currentTileSetData = new List<TilesData>();

        private List<int[]> board;

        private int cols;
        private int rows;

        public readonly Action PlayerInputMoveDown;
        public readonly Action<EMoveTiles> PlayerInputMoveSideWays;

        public Action<List<TilesData>> UpdateGraphics;

        public Action GotBlocked;

        public Action<int, int> UpdateRunTimeList;

        public Board(int _cols, int _rows)
        {
            rows = _rows;
            cols = _cols;
            board = new List<int[]>();
            for (int i = 0; i < rows; i++)
            {
                int[] tempArray = new int[cols];
                board.Add(tempArray);
            }

            PlayerInputMoveDown += MoveTileSetDownwards;
            PlayerInputMoveSideWays += MoveSideToSide;
            GotBlocked += ClearCurrentTileSet;
        }

        /// <summary>
        /// Returns whether or not it successfully spawned a tile without killing the player.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        public void SpawnDataTile(int posX, int posY, int i)
        {
            TilesData tempData = new TilesData( posX, posY);
            currentTileSetData.Add(tempData);
            MapTileDataToGrid(posX, posY, false);

            if (!PlayerLost())
            {
                currentTileSetData.Clear();
            }
        }

        private bool PlayerLost()
        {
            bool canMove = false;
            if (!currentTileSetData.Any()) return false;
            for (int i = 0; i < currentTileSetData.Count; i++)
            {
                canMove = CheckIfTileCanMoveDown(currentTileSetData[i].posY, currentTileSetData[i].posX, false);
            }
            
            return canMove;
        }
        public List<TilesData> GetCurrentTileSetData()
        {
            return currentTileSetData;
        }

        private void MoveTileSetDownwards()
        {
            if (!currentTileSetData.Any()) return;
            for (int i = currentTileSetData.Count - 1; i >= 0; i--)
            {
                if (CheckIfTileCanMoveDown(currentTileSetData[i].posY, currentTileSetData[i].posX, true))
                {
                    board[currentTileSetData[i].posY][currentTileSetData[i].posX] = 0;
                    currentTileSetData[i].SetPosition(currentTileSetData[i].posX, ++currentTileSetData[i].posY);
                    MapTileDataToGrid(currentTileSetData[i].posX, currentTileSetData[i].posY, false);
                }
                else
                {
                    break;
                }
            }

            UpdateGraphics.Invoke(currentTileSetData);
        }

        private void MoveSideToSide(EMoveTiles eMoveTiles)
        {
            if (!currentTileSetData.Any()) return;
            int shiftLeftOrRight = eMoveTiles == EMoveTiles.ERight ? 1 : -1;

            for (int i = currentTileSetData.Count - 1; i >= 0; i--)
            {
                if (CheckIfTileCanMoveSideWays(currentTileSetData[i].posY, currentTileSetData[i].posX, eMoveTiles))
                {
                    board[currentTileSetData[i].posY][currentTileSetData[i].posX] = 0;
                    currentTileSetData[i].SetPosition(currentTileSetData[i].posX += shiftLeftOrRight,
                        currentTileSetData[i].posY);
                    MapTileDataToGrid(currentTileSetData[i].posX, currentTileSetData[i].posY, false);
                }
                else
                {
                    break;
                }
            }

            UpdateGraphics.Invoke(currentTileSetData);
        }

        private void ClearCurrentTileSet()
        {
            currentTileSetData.Clear();
        }

        public List<int[]> GetGrid()
        {
            return board;
        }

        public void MapTileDataToGrid(int posX, int posY, bool isEmpty)
        {
            board[posY][posX] = isEmpty ? 0 : 1;
        }

        public void PrintAllDataFromArray()
        {
            string someBuilder = "";
            for (int i = 0; i < rows; i++)
            {
                someBuilder += "# ";
                for (int j = 0; j < cols; j++)
                {
                    someBuilder += $"{board[i][j]} ";
                }

                someBuilder += "#";
                someBuilder += "\n";
            }

            Debug.Log(someBuilder);
        }

        public bool CheckForRowPair()
        {
            bool foundRowPair = false;
            int sameValues = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int nextNum = j + 1;
                    if (nextNum >= cols)
                    {
                        break;
                    }

                    if (board[i][j] == board[i][nextNum] && board[i][j] != 0)
                    {
                        sameValues++;
                    }

                    //If there are same values all across one of the rows. Add it as a pair.
                    //Make that row zero
                    if (sameValues == cols - 1)
                    {
                        for (int k = 0; k < cols; k++)
                        {
                            board[i][k] = 0;
                            UpdateRunTimeList?.Invoke(i, k);
                        }

                        foundRowPair = true;
                    }
                }

                sameValues = 0;
            }

            MoveAllTilesDown();
            return foundRowPair;
        }

        private bool CheckIfTileCanMoveDown(int rowPos, int columnPos, bool playerInput)
        {
            if (rowPos + 1 >= rows)
            {
                if (playerInput)
                {
                    GotBlocked?.Invoke();
                }

                return false;
            }

            if (board[rowPos + 1][columnPos] != 0)
            {
                if (playerInput)
                {
                    GotBlocked?.Invoke();
                }

                return false;
            }

            return true;
        }

        private bool CheckIfTileCanMoveSideWays(int rowPos, int columnPos, EMoveTiles eMoveTiles)
        {
            switch (eMoveTiles)
            {
                case EMoveTiles.ELeft:
                    if (columnPos - 1 < 0) return false;
                    for (int i = 0; i < currentTileSetData.Count; i++)
                    {
                        if (columnPos - 1 == currentTileSetData[i].posX && columnPos - 1 > 0) return true;
                    }
                    return board[rowPos][columnPos - 1] == 0;
                case EMoveTiles.ERight:
                    if (columnPos + 1 >= cols) return false;
                    for (int i = 0; i < currentTileSetData.Count; i++)
                    {
                        if (columnPos + 1 == currentTileSetData[i].posX && columnPos + 1 > cols) return true;
                    }
                    return board[rowPos][columnPos + 1] == 0;
                default: return false;
            }
        }

        private void MoveAllTilesDown()
        {
            //Move all elements down starting from the top aka the top of the list
            for (int i = rows - 1; i > 0; i--)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i + 1 >= rows) break;
                    if (board[i][j] == 0) continue;
                    if (CheckIfTileCanMoveDown(i, j, false))
                    {
                        int newPos = CalculateStepsTillTileIsAtBottom(i, j);
                        board[i][j] = 0;
                        board[newPos][j] = 1;
                    }
                }
            }
        }

        private int CalculateStepsTillTileIsAtBottom(int rowStartPos, int columnPos)
        {
            if (board[rowStartPos][columnPos] == 0)
            {
                Debug.LogWarning("The specified tile is currently empty!");
                return 0;
            }

            int stepsToMove = rowStartPos;
            for (int i = rowStartPos; i < rows; i++)
            {
                if (i + 1 >= rows) break;
                if (board[i + 1][columnPos] == 0)
                {
                    stepsToMove++;
                }
            }

            return stepsToMove;
        }
    }
    public enum EMoveTiles
    {
        ERight,
        ELeft
    }
}
