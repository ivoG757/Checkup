using Checkup.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    internal class Rook : BasePiece
    {
        public Rook(bool isBlack) : base(isBlack) { }
        public override PieceType Type => PieceType.Rook;
        private (int x, int y)[] Directions => new (int, int)[]
        {
            (1, 0),   // Down
            (-1, 0),  // Up
            (0, 1),   // Right
            (0, -1)   // Left
        };
        internal override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            return GetSlidingAttacks(board, currentX, currentY, Directions);
        }
        internal override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {

            return GetSlidingMoves(board, currentX, currentY, Directions);
        }
        
    }
}
