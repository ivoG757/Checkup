using System;
using System.Collections.Generic;
using System.Text;
using Checkup.Core.Models.Interfaces;

namespace Checkup.Core.Models
{
    public class Board
    {
        public IPiece[,] Squares { get; set; } = new IPiece[8, 8];

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
