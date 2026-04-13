using Checkup.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    internal class King : BasePiece
    {
        public King(bool isBlack) : base(isBlack) { }
        public override PieceType Type => PieceType.King;

        internal override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var validMoves = new List<(int x, int y)>();

            var kingsMoves = new List<(int x, int y)>
            {
                (currentX + 1, currentY),       // Move down
                (currentX + 1, currentY + 1),   // Move down-right
                (currentX + 1, currentY - 1),   // Move down-left
                (currentX - 1, currentY),       // Move up
                (currentX - 1, currentY + 1),   // Move up-right
                (currentX - 1, currentY - 1),   // Move up-left
                (currentX, currentY + 1),       // Move right
                (currentX, currentY - 1)        // Move left
            };
            foreach (var m in kingsMoves)
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

        internal override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }
    }
}
