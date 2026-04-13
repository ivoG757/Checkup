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
        internal override List<(int x, int y)> GetAttackedSquares(GameState state, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }
        internal override List<(int x, int y)> GetValidMoves(GameState state, int currentX, int currentY)
        {
            var board = state.BoardState;

            return GetSlidingMoves(state, currentX, currentY, new (int, int)[]
             {
                (1, 0),   // Down
                (-1, 0),  // Up
                (0, 1),   // Right
                (0, -1)   // Left
             });
        }
        
    }
}
