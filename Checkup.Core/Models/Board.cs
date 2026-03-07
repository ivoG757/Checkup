using System;
using System.Collections.Generic;
using Checkup.Core.Models.Enums;
using System.Text;

namespace Checkup.Core.Models
{

    public class Square
    {
        public Piece Piece { get; set; } = Piece.None;
        public Color? Color { get; set; } = null;
    }

    public class Board
    {
        public Square[,] Squares { get; set; } = new Square[8, 8];

        public Board()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Squares[x, y] = new Square();
                }
            }
        }
    }
}
