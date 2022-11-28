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

        private List<GameObject[]> runtimeTileList;

        //TODO: Make this a list afterwards
        [SerializeField] private Tile tilesList;

        void Start()
        {
            InitalizeTileList();
            int someInt = 1;
            Vector2 startPos = new Vector2(-4, -3);
            tetrisBoard = new Board(rowAmount, colAmount);
            
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                    SpawnGrid(new Vector2(startPos.x+i, startPos.y+j), j, i);
                    if (i == 1 || i == 0 || i == 7 || i == 2)
                    {
                        tetrisBoard.MapGridToUIGrid(i, j,someInt);
                    }
                    else
                    {
                        tetrisBoard.MapGridToUIGrid(i,j, Random.Range(1,9));
                    }
                }
                someInt++;
            }

            tetrisBoard.PrintAllDataFromArray();
            tetrisBoard.CheckForRowPair(out List<int> tileRowBroken);


            
            for (int i = 0; i < tileRowBroken.Count; i++)
            {
                for (int j = 0; j < colAmount; j++)
                {
                   Destroy(runtimeTileList[tileRowBroken[i]][j]); 
                }
            }
            
            
            tetrisBoard.PrintAllDataFromArray();

            
            //^1 is equal to the end of the list so its basically tileRowBroken.Count - 1
            if (tetrisBoard.CheckIfTileCanMoveDown(tileRowBroken[^1]))
            {
                List<int> rowDeleted = new List<int>();
                tetrisBoard.MoveAllTilesDownOneStep(tileRowBroken.Count, out rowDeleted);
                
                MoveUITiles(rowDeleted[^1], rowDeleted.Count);
            }
            tetrisBoard.PrintAllDataFromArray();
        }

        private void SpawnGrid(Vector2 pos, int posX, int posY)
        {
            var tile = Instantiate(tilesList);
            var gameObject = tile.gameObject;
           gameObject.name = $"tile[{posX}][{posY}]";
           gameObject.transform.transform.position = new Vector3(pos.x, pos.y, 0);
           runtimeTileList[posX][posY] = gameObject;
           tile.Init(posX, posY);
        }

        private void InitalizeTileList()
        {
            runtimeTileList = new List<GameObject[]>();
            for (int i = 0; i < rowAmount; i++)
            {
                runtimeTileList.Add(new GameObject[colAmount]);
            }
        }

        private void MoveUITiles(int startPos, int amountOfSteps)
        {
            if (startPos == 0) return;
            for (int i = 0; i < rowAmount; i++)
            {
                if(i < startPos) continue;
                for (int j = 0; j < colAmount; j++)
                {
                    if (runtimeTileList[i][j] == null)
                    {
                        Debug.Log("No more object!");
                        break;
                    }
                    for (int k = 0; k < amountOfSteps; k++)
                    {
                        Debug.Log("Moved!");
                        runtimeTileList[i][j].transform.position = new Vector3(runtimeTileList[i][j].transform.position.x,
                            runtimeTileList[i][j].transform.position.y - 1 , 0);
                    }
                }
            }
        }
    }
}
