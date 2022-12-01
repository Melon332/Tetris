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

        public bool hasHitSomething { private get; set; }

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
            GotBlocked += ResetBoolValue;
        }

        public void SpawnDataTile(eShape shape)
        {
            for (int i = 0; i < 4; i++)
            {
                TilesData tempData = new TilesData(shape, 0, i);
                currentTileSetData.Add(tempData);
                MapGridFromUIGrid(currentTileSetData[i].posX, i, false);
            }
        }

        private void ResetBoolValue()
        {
            hasHitSomething = false;
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
                if (CheckIfTileCanMoveDown(currentTileSetData[i].posY, currentTileSetData[i].posX))
                {
                    board[currentTileSetData[i].posY][currentTileSetData[i].posX] = 0;
                    currentTileSetData[i].SetPosition(currentTileSetData[i].posX, ++currentTileSetData[i].posY);
                    MapGridFromUIGrid(currentTileSetData[i].posX, currentTileSetData[i].posY, false);
                }
            }
            UpdateGraphics.Invoke(currentTileSetData);
            PrintAllDataFromArray();
        }

        private void MoveSideToSide(EMoveTiles eMoveTiles)
        {
            if (!currentTileSetData.Any()) return;
            int shiftLeftOrRight = eMoveTiles == EMoveTiles.ERight ? 1 : -1;
            
            for (int i = currentTileSetData.Count - 1; i >= 0; i--)
            {
                if (CheckIfTileCanMoveSideWays(currentTileSetData[i].posY, currentTileSetData[i].posX, eMoveTiles))
                {
                    Debug.LogWarning("moved to the side!");
                    board[currentTileSetData[i].posY][currentTileSetData[i].posX] = 0;
                    currentTileSetData[i].SetPosition(currentTileSetData[i].posX += shiftLeftOrRight, currentTileSetData[i].posY);
                    MapGridFromUIGrid(currentTileSetData[i].posX, currentTileSetData[i].posY, false);
                }
                else
                {
                    break;
                }
            }
            UpdateGraphics.Invoke(currentTileSetData);
            PrintAllDataFromArray();
        }

        private void ClearCurrentTileSet()
        {
            currentTileSetData.Clear();
        }
        public List<int[]> GetGrid()
        {
            return board;
        }

        public void MapGridFromUIGrid(int posX, int posY, bool isEmpty)
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
                            foundRowPair = true;
                        }
                    }
                }
                sameValues = 0;
            }

            return foundRowPair;
        }

        private bool CheckIfTileCanMoveDown(int rowPos, int columnPos)
        {
            if (rowPos + 1 >= rows)
            {
                GotBlocked?.Invoke();
                Debug.LogWarning("Hit the bottom of the board!");
                hasHitSomething = true;
                return false;
            }

            if (board[rowPos + 1][columnPos] != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        private bool CheckIfTileCanMoveSideWays(int rowPos, int columnPos, EMoveTiles eMoveTiles)
        {
            switch (eMoveTiles)
            {
                case EMoveTiles.ELeft:
                    if (columnPos - 1 < 0) return false;
                    return board[rowPos][columnPos - 1] == 0;
                    break;
                case EMoveTiles.ERight:
                    if (columnPos + 1 >= cols) return false;
                    return board[rowPos][columnPos + 1] == 0;
                default: return false;
            }
        }

        public void MoveAllTilesDownOneStep()
        {
            //Copy all the non zero rows into the temporary list
            for (int i = rows - 1; i > 0; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (CheckIfTileCanMoveDown(i, j))
                    {
                        board[i + 1][j] = board[i][j];
                        board[i][j] = 0;
                    }
                }
            }
        }

        public int CalculateStepsTileCanMove(int rowStartPos, int columnPos, List<Tile[]> tilesList)
        {
            if (tilesList[rowStartPos][columnPos] == null)
            {
                Debug.LogWarning("The specified tile is currently empty!");
                return 0;
            }
            int stepsToMove = 0;
            for (int i = rowStartPos; i > 0; i--)
            {
                if (tilesList[i][columnPos] == null)
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
