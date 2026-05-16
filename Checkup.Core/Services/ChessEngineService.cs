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
            var moves = piece.GetValidMoves(GameState.BoardState, row, col);
            return moves.Where(move => !WouldLeaveKingInCheck(row, col, move.x, move.y) && GetPiece(move.x, move.y) == null).ToList();
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
        private void ExecuteMove(Move move)
        {
            PlacePiece(move.To.x, move.To.y, move.MovedPiece);
            PlacePiece(move.From.x, move.From.y, null);

            if (move.IsCastling)
            {
                bool isKingside = move.To.y == 6;

                int rookFromY = isKingside ? 7 : 0;
                int rookToY = isKingside ? 5 : 3;

                var rook = GameState.BoardState.Squares[move.To.x, rookFromY];

                PlacePiece(move.To.x, rookToY, rook);
                PlacePiece(move.To.x, rookFromY, null);
            }

            GameState.Moves.Add(move);

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

                var move = new Move((fromX, fromY), (toX, toY))
                {
                    MovedPiece = currentPiece,
                    CapturedPiece = GameState.BoardState.Squares[toX, toY]
                };

                ExecuteMove(move);

                successfulMove = true;
                GameState.IsBlackTurn = !GameState.IsBlackTurn;

                HandleEndOfTurn(successfulMove, move);

            }
            return successfulMove;
        }

        private void HandleEndOfTurn(bool successfulMove, Move move)
        {
            var currentPiece = move.MovedPiece;
            var fromX = move.From.x;
            var fromY = move.From.y;
            var toX = move.To.x;
            var toY = move.To.y;

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
            if (successfulMove)
            {
                if (!HasAnyLegalMoves(GameState.IsBlackTurn))
                {
                    if (IsInCheck(GameState.IsBlackTurn))
                    {
                        checkmate();
                    }
                    else
                    {
                        stalemate();
                    }
                }
            }
        }

        private void stalemate()
        {
            GameState.EndReason = GameEndReason.Stalemate;
            GameState.Result = GameResult.Draw;
        }

        private void checkmate()
        {
            GameState.EndReason = GameEndReason.Checkmate;

            GameState.Result = GameState.IsBlackTurn
                ? GameResult.WhiteWon : GameResult.BlackWon;
        }

        private bool HasAnyLegalMoves(bool isBlack)
        {
            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    var piece = GameState.BoardState.Squares[fromX, fromY];

                    if (piece == null || piece.IsBlack != isBlack)
                        continue;

                    var moves = piece.GetValidMoves(GameState.BoardState, fromX, fromY);

                    foreach (var move in moves)
                    {
                        if (!WouldLeaveKingInCheck(fromX, fromY, move.x, move.y))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private (bool flowControl, bool value) TryHandleCastling(
            int fromX,
            int fromY,
            int toX,
            int toY,
            BasePiece currentPiece,
            BasePiece targetPiece)
        {
            if (currentPiece.Type != PieceType.King)
            {
                return (true, false);
            }

            if (fromX != toX || Math.Abs(toY - fromY) != 2)
            {
                return (true, false);
            }

            bool isBlack = currentPiece.IsBlack;


            if (isBlack && GameState.Flags.BlackKingMoved)
                return (false, false);

            if (!isBlack && GameState.Flags.WhiteKingMoved)
                return (false, false);


            if (IsInCheck(isBlack))
            {
                return (false, false);
            }

            bool isKingside = toY > fromY;

            int rookY = isKingside ? 7 : 0;
            int rookTargetY = isKingside ? 5 : 3;
            int kingTargetY = isKingside ? 6 : 2;

            var rook = GameState.BoardState.Squares[fromX, rookY];

            if (rook == null ||
                rook.Type != PieceType.Rook ||
                rook.IsBlack != isBlack)
            {
                return (false, false);
            }

            if (isBlack)
            {
                if (isKingside && GameState.Flags.BlackLeftRookMoved)
                    return (false, false);

                if (!isKingside && GameState.Flags.BlackRightRookMoved)
                    return (false, false);
            }
            else
            {
                if (isKingside && GameState.Flags.WhiteLeftRookMoved)
                    return (false, false);

                if (!isKingside && GameState.Flags.WhiteRightRookMoved)
                    return (false, false);
            }

            int start = Math.Min(fromY, rookY) + 1;
            int end = Math.Max(fromY, rookY) - 1;

            for (int y = start; y <= end; y++)
            {
                if (GameState.BoardState.Squares[fromX, y] != null)
                {
                    return (false, false);
                }
            }

            int step = isKingside ? 1 : -1;

            if (WouldLeaveKingInCheck(fromX, fromY, fromX, fromY + step) ||
                WouldLeaveKingInCheck(fromX, fromY, fromX, kingTargetY))
            {
                return (false, false);
            }

            var move = new Move((fromX, fromY), (fromX, kingTargetY))
            {
                MovedPiece = currentPiece,
                IsCastling = true
            };

            ExecuteMove(move);

            if (isBlack)
            {
                GameState.Flags.BlackKingMoved = true;

                if (isKingside)
                    GameState.Flags.BlackLeftRookMoved = true;
                else
                    GameState.Flags.BlackRightRookMoved = true;
            }
            else
            {
                GameState.Flags.WhiteKingMoved = true;

                if (isKingside)
                    GameState.Flags.WhiteLeftRookMoved = true;
                else
                    GameState.Flags.WhiteRightRookMoved = true;
            }

            GameState.IsBlackTurn = !GameState.IsBlackTurn;

            return (false, true);
        }
        private bool WouldLeaveKingInCheck(int fromX, int fromY, int toX, int toY)
        {
            var piece = GameState.BoardState.Squares[fromX, fromY];
            var captured = GameState.BoardState.Squares[toX, toY];

            GameState.BoardState.Squares[toX, toY] = piece;
            GameState.BoardState.Squares[fromX, fromY] = null;

            var inCheck = IsInCheck(piece.IsBlack);

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
        private bool IsInCheck(bool isBlack)
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
                        var opponentMoves = piece.GetAttackedSquares(GameState.BoardState, x, y);
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

        private void PlacePiece(int x, int y, BasePiece piece)
        {
            GameState.BoardState.Squares[x, y] = piece;
        }

        public List<(int x, int y)> GetValidAttacks(int row, int col)
        {
            var piece = GetPiece(row, col);

            if (piece == null)
            {
                return new();
            }

            var moves = piece.GetValidMoves(GameState.BoardState, row, col);

            return moves
                .Where(move =>
                {
                    var target = GetPiece(move.x, move.y);

                    return target != null &&
                           target.IsBlack != piece.IsBlack &&
                           !WouldLeaveKingInCheck(row, col, move.x, move.y);
                })
                .ToList();
        }

        public bool IsPlayerInCheck()
        {
            return IsInCheck(GameState.IsBlackTurn);
        }

        public GameResult GetGameResult()
        {
            return GameState.Result;
        }

        public bool IsBlackTurn()
        {
            return GameState.IsBlackTurn;
        }
    }
}
