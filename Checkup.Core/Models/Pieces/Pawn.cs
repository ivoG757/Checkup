using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Pieces;
using System;
using System.Collections.Generic;

namespace Checkup.Core.Models.Pieces
{
    internal class Pawn : BasePiece
    {
        public Pawn(bool isBlack) : base(isBlack) { }
        public override PieceType Type => PieceType.Pawn;

        internal override List<(int x, int y)> GetAttackedSquares(GameState state, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }

        internal override List<(int x, int y)> GetValidMoves(GameState state, int currentX, int currentY)
        {
            var board = state.BoardState;

            var moves = new List<(int, int)>();
            int direction = IsBlack ? 1 : -1;
            int nextRow = currentX + direction;

            //Move forward if the square is empty 
            if (IsInBounds(nextRow, currentY) && board.Squares[nextRow, currentY] == null)
            {
                moves.Add((nextRow, currentY));
                //First move: move two squares forward if both are empty
                int doubleStepRow = currentX + 2 * direction;

                if (IsInBounds(doubleStepRow, currentY))
                {
                    if (board.Squares[doubleStepRow, currentY] == null)
                    {
                        if (state.IsFirstMove(this))
                        {
                            moves.Add((doubleStepRow, currentY));
                        }
                    }
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