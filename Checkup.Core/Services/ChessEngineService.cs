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
            }
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
