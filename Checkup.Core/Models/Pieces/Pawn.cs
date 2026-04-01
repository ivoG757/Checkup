using Checkup.Core.Common.Enums;
using Checkup.Core.Models.Pieces;
using System;
using System.Collections.Generic;

namespace Checkup.Core.Models.Pieces
{
    public class Pawn : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public bool HasMoved { get; set; } = false;
        public override PieceType Type => PieceType.Pawn;
        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var moves = new List<(int, int)>();
            int direction = IsBlack ? 1 : -1;
            int nextRow = currentX + direction;

            //Move forward if the square is empty 
            if (IsInBounds(nextRow, currentY) && board.Squares[nextRow, currentY] == null)
            {
                moves.Add((nextRow, currentY));

                //First move: move two squares forward if both are empty
                int doubleStepRow = currentX + 2 * direction;
                if (!HasMoved && IsInBounds(doubleStepRow, currentY) && board.Squares[doubleStepRow, currentY] == null)
                {
                    moves.Add((doubleStepRow, currentY));
                }
            }

            //Capture diagonally
            int[] diagCols = { currentY - 1, currentY + 1 };
            foreach (int col in diagCols)
            {
                if (IsInBounds(nextRow, col) && board.Squares[nextRow, col] != null && board.Squares[nextRow, col].IsBlack != IsBlack)
                {
                    moves.Add((nextRow, col));
                }
            }

            return moves;
        }
    }
}