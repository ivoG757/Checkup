using Checkup.Core.Models;
using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public ChessEngineService()
        {
            SetPiecesOnBoard();
        }
        internal GameState GameState { get; set; } = new GameState();

        public void SetPiecesOnBoard()
        {
            PlacePiece(0, 0, new Rook(true));
            PlacePiece(0, 1, new Knight(true));
            PlacePiece(0, 2, new Bishop(true));
            PlacePiece(0, 3, new Queen(true));
            PlacePiece(0, 4, new King(true));
            PlacePiece(0, 5, new Bishop(true));
            PlacePiece(0, 6, new Knight(true));
            PlacePiece(0, 7, new Rook(true));

            // Black pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(1, c, new Pawn(true));
            // White pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(6, c, new Pawn(false));

            // White back rank
            PlacePiece(7, 0, new Rook(false));
            PlacePiece(7, 1, new Knight(false));
            PlacePiece(7, 2, new Bishop(false));
            PlacePiece(7, 3, new Queen(false));
            PlacePiece(7, 4, new King(false));
            PlacePiece(7, 5, new Bishop(false));
            PlacePiece(7, 6, new Knight(false));
            PlacePiece(7, 7, new Rook(false));
        }
        public List<(int x, int y)> GetValidMoves(int row, int col)
        {
            var piece = GameState.BoardState.Squares[row, col];
            return piece.GetValidMoves(GameState.BoardState, row, col);
        }

        public BasePiece? GetPiece(int row, int col)
        {
            return GameState.BoardState.Squares[row, col];
        }

        public void UndoLastMove()
        {
            if (GameState.Moves.Count == 0)
            {
                return;
            }
            var lastMove = GameState.Moves.Last();
            PlacePiece(lastMove.From.x, lastMove.From.y, lastMove.MovedPiece);
            PlacePiece(lastMove.To.x, lastMove.To.y, lastMove.CapturedPiece);
            GameState.Moves.RemoveAt(GameState.Moves.Count - 1);
            GameState.IsBlackTurn = !GameState.IsBlackTurn;
        }

        public bool MovePiece(int fromX, int fromY, int toX, int toY)
        {
            var currentPiece = GameState.BoardState.Squares[fromX, fromY];

            if (currentPiece == null)
            {
                return false;
            }

            if (currentPiece.IsBlack != GameState.IsBlackTurn)
            {
                return false;
            }
            var targetPiece = GameState.BoardState.Squares[toX, toY];

            (bool flowControl, bool value) = TryHandleCastling(fromX, fromY, toX, toY, currentPiece, targetPiece);
            if (!flowControl)
            {
                return value;
            }

            var targetMove = (toX, toY);

            var successfulMove = false;

            var validMoves = currentPiece.GetValidMoves(GameState.BoardState, fromX, fromY);

            if (validMoves.Contains(targetMove))
            {
                if (WouldLeaveKingInCheck(fromX, fromY, toX, toY))
                {
                    return false;
                }

                if (GameState.BoardState.Squares[toX, toY] != null) // Capture piece
                {
                    GameState.Moves.Add(new Move((fromX, fromY), (toX, toY))
                    {
                        MovedPiece = currentPiece,
                        CapturedPiece = GameState.BoardState.Squares[toX, toY]
                    });
                }
                else
                {
                    GameState.Moves.Add(new Move((fromX, fromY), (toX, toY)) // No capture, just move
                    {
                        MovedPiece = currentPiece,
                    });
                }

                PlacePiece(toX, toY, currentPiece);
                PlacePiece(fromX, fromY, null);

                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                successfulMove = true;
                switch (currentPiece.Type)
                {
                    case PieceType.King:
                        if (currentPiece.IsBlack)
                        {
                            GameState.Flags.BlackKingMoved = true;
                        }
                        else
                        {
                            GameState.Flags.WhiteKingMoved = true;
                        }
                        break;
                    case PieceType.Rook:
                        if (currentPiece.IsBlack)
                        {
                            if (fromX == 0 && fromY == 0)
                            {
                                GameState.Flags.BlackRightRookMoved = true;
                            }
                            else if (fromX == 0 && fromY == 7)
                            {
                                GameState.Flags.BlackLeftRookMoved = true;
                            }
                        }
                        else
                        {
                            if (fromX == 7 && fromY == 0)
                            {
                                GameState.Flags.WhiteRightRookMoved = true;
                            }
                            else if (fromX == 7 && fromY == 7)
                            {
                                GameState.Flags.WhiteLeftRookMoved = true;
                            }
                        }
                        break;
                }
            }
            return successfulMove;
        }

        private (bool flowControl, bool value) TryHandleCastling(
    int fromX,
    int fromY,
    int toX,
    int toY,
    BasePiece currentPiece,
    BasePiece targetPiece)
        {
            // Not a castling attempt
            if (currentPiece.Type != PieceType.King ||
                targetPiece == null ||
                targetPiece.Type != PieceType.Rook ||
                targetPiece.IsBlack != currentPiece.IsBlack)
            {
                return (true, false);
            }

            // King already moved
            if (currentPiece.IsBlack)
            {
                if (GameState.Flags.BlackKingMoved)
                    return (false, false);
            }
            else
            {
                if (GameState.Flags.WhiteKingMoved)
                    return (false, false);
            }

            // King cannot castle while in check
            if (IsInCheck(currentPiece.IsBlack))
            {
                return (false, false);
            }

            // WHITE QUEENSIDE
            if (!currentPiece.IsBlack && toX == 7 && toY == 0)
            {
                if (GameState.Flags.WhiteRightRookMoved)
                    return (false, false);

                // Squares between rook and king must be empty
                if (GameState.BoardState.Squares[7, 1] != null ||
                    GameState.BoardState.Squares[7, 2] != null ||
                    GameState.BoardState.Squares[7, 3] != null)
                {
                    return (false, false);
                }

                // King cannot pass through check
                if (WouldLeaveKingInCheck(7, 4, 7, 3) ||
                    WouldLeaveKingInCheck(7, 4, 7, 2))
                {
                    return (false, false);
                }

                // Move king
                PlacePiece(7, 2, currentPiece);

                // Move rook
                PlacePiece(7, 3, targetPiece);

                // Clear old squares
                PlacePiece(7, 4, null);
                PlacePiece(7, 0, null);

                GameState.Moves.Add(new Move((7, 4), (7, 2))
                {
                    MovedPiece = currentPiece,
                    IsCastling = true
                });

                GameState.Flags.WhiteKingMoved = true;
                GameState.Flags.WhiteRightRookMoved = true;

                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                return (false, true);
            }

            // WHITE KINGSIDE
            if (!currentPiece.IsBlack && toX == 7 && toY == 7)
            {
                if (GameState.Flags.WhiteLeftRookMoved)
                    return (false, false);

                if (GameState.BoardState.Squares[7, 5] != null ||
                    GameState.BoardState.Squares[7, 6] != null)
                {
                    return (false, false);
                }

                if (WouldLeaveKingInCheck(7, 4, 7, 5) ||
                    WouldLeaveKingInCheck(7, 4, 7, 6))
                {
                    return (false, false);
                }

                PlacePiece(7, 6, currentPiece);
                PlacePiece(7, 5, targetPiece);

                PlacePiece(7, 4, null);
                PlacePiece(7, 7, null);

                GameState.Moves.Add(new Move((7, 4), (7, 6))
                {
                    MovedPiece = currentPiece,
                    IsCastling = true
                });

                GameState.Flags.WhiteKingMoved = true;
                GameState.Flags.WhiteLeftRookMoved = true;

                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                return (false, true);
            }

            // BLACK QUEENSIDE
            if (currentPiece.IsBlack && toX == 0 && toY == 0)
            {
                if (GameState.Flags.BlackRightRookMoved)
                    return (false, false);

                if (GameState.BoardState.Squares[0, 1] != null ||
                    GameState.BoardState.Squares[0, 2] != null ||
                    GameState.BoardState.Squares[0, 3] != null)
                {
                    return (false, false);
                }

                if (WouldLeaveKingInCheck(0, 4, 0, 3) ||
                    WouldLeaveKingInCheck(0, 4, 0, 2))
                {
                    return (false, false);
                }

                PlacePiece(0, 2, currentPiece);
                PlacePiece(0, 3, targetPiece);

                PlacePiece(0, 4, null);
                PlacePiece(0, 0, null);

                GameState.Moves.Add(new Move((0, 4), (0, 2))
                {
                    MovedPiece = currentPiece,
                    IsCastling = true
                });

                GameState.Flags.BlackKingMoved = true;
                GameState.Flags.BlackRightRookMoved = true;

                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                return (false, true);
            }


            // BLACK KINGSIDE
            if (currentPiece.IsBlack && toX == 0 && toY == 7)
            {
                if (GameState.Flags.BlackLeftRookMoved)
                    return (false, false);

                if (GameState.BoardState.Squares[0, 5] != null ||
                    GameState.BoardState.Squares[0, 6] != null)
                {
                    return (false, false);
                }

                if (WouldLeaveKingInCheck(0, 4, 0, 5) ||
                    WouldLeaveKingInCheck(0, 4, 0, 6))
                {
                    return (false, false);
                }

                PlacePiece(0, 6, currentPiece);
                PlacePiece(0, 5, targetPiece);

                PlacePiece(0, 4, null);
                PlacePiece(0, 7, null);

                GameState.Moves.Add(new Move((0, 4), (0, 6))
                {
                    MovedPiece = currentPiece,
                    IsCastling = true
                });

                GameState.Flags.BlackKingMoved = true;
                GameState.Flags.BlackLeftRookMoved = true;

                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                return (false, true);
            }

            return (true, false);
        }

        private bool Castle(BasePiece to, BasePiece from)
        {
            var successfulMove = false;
            return successfulMove;
        }
        private bool WouldLeaveKingInCheck(int fromX, int fromY, int toX, int toY)
        {
            var piece = GameState.BoardState.Squares[fromX, fromY];
            var captured = GameState.BoardState.Squares[toX, toY];

            // apply
            GameState.BoardState.Squares[toX, toY] = piece;
            GameState.BoardState.Squares[fromX, fromY] = null;

            var inCheck = IsInCheck(piece.IsBlack);

            // revert 
            GameState.BoardState.Squares[fromX, fromY] = piece;
            GameState.BoardState.Squares[toX, toY] = captured;

            return inCheck;
        }
        private (int x, int y)? FindKing(bool isBlack)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = GameState.BoardState.Squares[x, y];
                    if (piece is King && piece.IsBlack == isBlack)
                    {
                        return (x, y);
                    }
                }
            }
            return null;
        }
        public bool IsInCheck(bool isBlack)
        {
            (int kingX, int kingY)? kingsPosition = FindKing(isBlack);

            if (kingsPosition == null)
            {
                return false;
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = GameState.BoardState.Squares[x, y];
                    if (piece != null && piece.IsBlack != isBlack)
                    {
                        var opponentMoves = piece.GetValidMoves(GameState.BoardState, x, y);
                        if (opponentMoves.Contains(kingsPosition.Value))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal bool IsFirstMove(BasePiece piece, GameState state)
        {
            return !state.Moves.Any(m => m.MovedPiece == piece);
        }

        internal void PlacePiece(int x, int y, BasePiece piece)
        {
            GameState.BoardState.Squares[x, y] = piece;
        }

        public List<(int x, int y)> GetValidAttacks(int row, int col)
        {
            var piece = GameState.BoardState.Squares[row, col];
            return piece.GetAttackedSquares(GameState.BoardState, row, col);
        }
    }
}
