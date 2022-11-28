using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TetrisBoard
{
    public class Board
    {
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


        public List<int[]> GetGrid()
        {
            return board;
        }

        public void MapGridToUIGrid(int posX, int posY, int value)
        {
            board[posX][posY] = value;
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

            //Copy all the non zero rows into the temporary list
            for (int i = 0; i < rows; i++)
            {
                if (board[i][0] != 0)
                {
                    tempList.Add(board[i]);
                }

                if (board[i][0] == 0)
                {
                    rowsDeleted.Add(i);
                }
            }
            board = tempList;
        }

    }
}
