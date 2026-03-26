//using Checkup.Core.Common;
//using Checkup.Core.Models.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Checkup.Core.Models.Pieces
//{
//    public class Rook : BasePiece
//    {
//        public override char Symbol => IsBlack ? '♜' : '♖';

//        public override List<(int x, int y)> GetValidMoves(Board board, int x, int y)
//        {
//            var moves = new List<(int, int)>();

//            // Up
//            for (int i = x - 1; i >= 0; i--)
//            {
//                if (board.Squares[i, y] == null)
//                    moves.Add((i, y));
//                else
//                {
//                    if (board.Squares[i, y].IsBlack != IsBlack)
//                        moves.Add((i, y)); // can capture
//                    break; // blocked
//                }
//            }

//            // Down
//            for (int i = x + 1; i < 8; i++)
//            {
//                if (board.Squares[i, y] == null)
//                    moves.Add((i, y));
//                else
//                {
//                    if (board.Squares[i, y].IsBlack != IsBlack)
//                        moves.Add((i, y));
//                    break;
//                }
//            }

//            // Left
//            for (int j = y - 1; j >= 0; j--)
//            {
//                if (board.Squares[x, j] == null)
//                    moves.Add((x, j));
//                else
//                {
//                    if (board.Squares[x, j].IsBlack != IsBlack)
//                        moves.Add((x, j));
//                    break;
//                }
//            }

//            // Right
//            for (int j = y + 1; j < 8; j++)
//            {
//                if (board.Squares[x, j] == null)
//                    moves.Add((x, j));
//                else
//                {
//                    if (board.Squares[x, j].IsBlack != IsBlack)
//                        moves.Add((x, j));
//                    break;
//                }
//            }

//            return moves;
//        }
//    }
//}
