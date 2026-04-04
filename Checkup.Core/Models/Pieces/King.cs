using Checkup.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    public class King : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public bool HasMoved { get; set; } = false;
        public override PieceType Type => PieceType.King;

        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var validMoves = new List<(int x, int y)>();

            var kingsMoves = new List<(int x, int y)>
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
        //public bool CanCastle(Board board, int currentX, int currentY, int targetX, int targetY)
        //{
        //    // Castling logic: King moves two squares towards the rook, and the rook moves to the square next to the king
        //    if (currentX != targetX || Math.Abs(currentY - targetY) != 2)
        //        return false; // Not a valid castling move
        //    int rookY = targetY > currentY ? 7 : 0; // Determine which rook is involved
        //    var rook = board.Squares[currentX, rookY] as Rook;
        //    if (rook == null || rook.HasMoved || this.HasMoved)
        //        return false; // Rook or King has already moved
        //    // Check if squares between king and rook are empty
        //    int step = targetY > currentY ? 1 : -1;
        //    for (int y = currentY + step; y != rookY; y += step)
        //    {
        //        if (board.Squares[currentX, y] != null)
        //            return false; // Squares between are not empty
        //    }
        //    // Additional checks for check conditions can be implemented here
        //    return true; // Castling is possible
        //}

        public override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }
    }
}
