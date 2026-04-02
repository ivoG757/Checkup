using Checkup.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    public class King : BasePiece
    {
        public override bool IsBlack { get; set; } = false;

        public override PieceType Type => PieceType.King;

        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var validMoves = new List<(int x, int y)>();
            var kingMoves = new List<(int x, int y)>
            {
                (currentX + 1, currentY), // Move down
                (currentX + 1, currentY + 1), // Move down-right
                (currentX + 1, currentY - 1), // Move down-left
                (currentX - 1, currentY), // Move up
                (currentX - 1, currentY + 1), // Move up-right
                (currentX - 1, currentY - 1), // Move up-left
                (currentX, currentY + 1), // Move right
                (currentX, currentY - 1) // Move left
            };
            foreach (var m in kingMoves)
            {
                if (IsInBounds(m.x, m.y))
                {
                    if (board.Squares[m.x, m.y] == null)
                    {
                        validMoves.Add(m);
                    }
                    else if (board.Squares[m.x, m.y].IsBlack != this.IsBlack)
                    {
                        //Can capture opponent's piece
                        validMoves.Add(m);
                    }
                }
            }
            return validMoves;
        }
    }
}
