using System;
using System.Collections.Generic;
using TetrisBoard;


namespace Tiles
{
    public class TileManager
    {
        private Board currentBoard;

        public TileManager(Board _board)
        {
            currentBoard = _board;
        }
    }
}