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
        }

        public void SpawnDataTile(eShape shape)
        {
            for (int i = 0; i < 4; i++)
            {
                TilesData tempData = new TilesData(shape, 0, i);
                currentTileSetData.Add(tempData);
                MapGridFromUIGrid(currentTileSetData[i].posX, i, false);
            }
            //MoveTileSetDownwards(currentTileSetData);
        }

        public List<TilesData> GetCurrentTileSetData()
        {
            return currentTileSetData;
        }

        private void MoveTileSetDownwards(List<TilesData> tilesData)
        {
            for (int i = tilesData.Count - 1; i >= 0; i--)
            {
                if (CheckIfTileCanMoveDown(tilesData[i].posY, tilesData[i].posX))
                {
                    board[tilesData[i].posY][tilesData[i].posX] = 0;
                    tilesData[i].SetPosition(tilesData[i].posX, ++tilesData[i].posY);
                    MapGridFromUIGrid(tilesData[i].posX, tilesData[i].posY, false);
                }
            }
        }

        /// <summary>
        /// left is -1 and right is 1 anything beside that will be discarded and the function will return
        /// </summary>
        /// <param name="tilesData"></param>
        /// <param name="leftOrRight"></param>
        private void MoveSideToSide(List<TilesData> tilesData, EMoveTiles eMoveTiles)
        {
            for (int i = tilesData.Count - 1; i >= 0; i--)
            {
                if (CheckIfTileCanMoveSideWays(tilesData[i].posY, tilesData[i].posX, eMoveTiles))
                {
                    Debug.LogWarning("moved to the side!");
                    board[tilesData[i].posY][tilesData[i].posX] = 0;
                    tilesData[i].SetPosition(++tilesData[i].posX, tilesData[i].posY);
                    MapGridFromUIGrid(tilesData[i].posX, tilesData[i].posY, false);
                }
            }
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

        public void CheckForRowPair(out List<int> pairNumbers)
        {
            pairNumbers = new List<int>();
            if (!IsEmpty()) return;
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
                        pairNumbers.Add(i);
                        for (int k = 0; k < cols; k++)
                        {
                            board[i][k] = 0;
                        }
                    }
                }
                sameValues = 0;
            }
        }

        public bool CheckIfTileCanMoveDown(int lastPosDestroyed)
        {
            bool spaceUnder = false;
            for (int i = 0; i < rows; i++)
            {
                if (i >= lastPosDestroyed) break;
                int nextRow = i + 1;
                if(nextRow >= rows) break;
                //If the row is empty, return true
                if (board[nextRow][0]== 0)
                {
                    spaceUnder = true;
                }
                else
                {
                    spaceUnder = false;
                }
            }
            return spaceUnder;
        }
        
        private bool CheckIfTileCanMoveDown(int rowPos, int columnPos)
        {
            if (rowPos + 1 >= rows) return false;
            return board[rowPos + 1][columnPos] == 0;
        }
        
        private bool CheckIfTileCanMoveSideWays(int rowPos, int columnPos, EMoveTiles eMoveTiles)
        {
            switch (eMoveTiles)
            {
                case EMoveTiles.ELeft:
                    if (columnPos - 1 <= cols) return false;
                    return board[rowPos][columnPos - 1] == 0;
                    break;
                case EMoveTiles.ERight:
                    if (columnPos + 1 >= cols) return false;
                    return board[rowPos][columnPos + 1] == 0;
                    break;
                default: return false;
                    break;
            }
        }

        public void MoveAllTilesDownOneStep(int rowsEmpty, out List<int> rowsDeleted)
        {
            List<int[]> tempList = new List<int[]>();
            rowsDeleted = new List<int>();

            if (rowsEmpty == 1)
            {
                tempList.Add(new int[cols]);
            }
            else
            {
                for (int i = 0; i < rowsEmpty; i++)
                {
                    tempList.Add(new int[cols]);
                }
            }

            bool found = false;

            //Copy all the non zero rows into the temporary list
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    found = board[i][j] != 0;
                    if (found)
                    {
                        break;
                    }
                }
                if (found)
                {
                    tempList.Add(board[i]);
                }
                else
                {
                    rowsDeleted.Add(i);
                }
            }
            board = tempList;
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

        public bool IsEmpty()
        {
            if (board.Any(p => p[0] > 0) )
            {
                return false;
            }
            Debug.Log("The board is empty");
            return true;
        }
        enum EMoveTiles
        {
            ERight,
            ELeft
        }
    }
}
