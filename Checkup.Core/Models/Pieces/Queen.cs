using Checkup.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    internal class Queen : BasePiece
    {
        public Queen(bool isBlack) : base(isBlack) { }
        public override PieceType Type => PieceType.Queen;

        internal override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }

        internal override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            return GetSlidingMoves(board, currentX, currentY, new (int, int)[]
            {
                (-1, -1), // Up-Left
                (-1, 1),  // Up-Right
                (1, -1),  // Down-Left
                (1, 1),   // Down-Right
                (1, 0),   // Down
                (-1, 0),  // Up
                (0, 1),   // Right
                (0, -1)   // Left
            });
        }
    }
}
