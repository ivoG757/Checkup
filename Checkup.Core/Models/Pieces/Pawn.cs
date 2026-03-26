using Checkup.Core.Common;
using Checkup.Core.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Checkup.Core.Models.Pieces
{
    public class Pawn : IPiece
    {
        public bool HasMoved { get; set; } = false;
        public bool IsBlack { get; set; } = false;
        public char Symbol => IsBlack ? (char)ChessPiecesEmojisBlack.Pawn : (char)ChessPiecesEmojisWhite.Pawn;

        public List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
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

        /// <summary>
        /// Helper to ensure coordinates are on the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
    }
}