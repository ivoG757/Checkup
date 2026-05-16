
using Checkup.Core.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;         

namespace Checkup.Core.Models.Pieces
{
    internal class Knight : BasePiece
    {
        public Knight(bool isBlack) : base(isBlack)
        {
        }
        public override PieceType Type => PieceType.Knight;

        internal override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            return Knightmoves(board, currentX, currentY, true);
        }

        internal override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            return Knightmoves(board, currentX, currentY, false);
        }
        private List<(int x, int y)> Knightmoves(Board board, int currentX, int currentY, bool includeOccupiedSquares)
        {
            var knightsMovements = new List<(int x, int y)>
            {
                (x: currentX - 1, y: currentY - 2), // up left
                (x: currentX + 1, y: currentY - 2), // up right
                (x: currentX - 1, y: currentY + 2), // down left
                (x: currentX + 1, y: currentY + 2), // down right
                (x: currentX - 2, y: currentY - 1), // left up
                (x: currentX - 2, y: currentY + 1), // left down
                (x: currentX + 2, y: currentY - 1), // right up
                (x: currentX + 2, y: currentY + 1)  // right down
            };
            var validMoves = new List<(int x, int y)>();
            foreach (var square in knightsMovements)
            {
                var (x, y) = square;

                if (IsInBounds(x, y))
                {
                    if (board.Squares[x, y] == null)
                    {
                        validMoves.Add((x, y));
                        continue;
                    }
                    if (includeOccupiedSquares)
                    {
                        validMoves.Add((x, y));
                        continue;
                    }
                    if (board.Squares[x, y].IsBlack != IsBlack)
                    {
                        // Can capture opponent's piece
                        validMoves.Add((x, y));
                    }
                }
            }
            return validMoves;
        }
    }
}
