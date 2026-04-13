using System;
using System.Collections.Generic;
using System.Text;
using Checkup.Core.Models;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Models
{
    internal class Board
    {
        internal BasePiece[,] Squares { get; set; } = new BasePiece[8, 8];

        public Board()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Squares[x, y] = null; 
                }
            }
        }
    }
}
