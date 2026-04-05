using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using System.IO.Pipelines;

namespace Checkup.Core.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public ChessEngineService()
        {
            SetPiecesOnBoard();
        }
        public GameState GameState { get; set; } = new GameState();

        public void SetPiecesOnBoard()
        {
            PlacePiece(0, 0, new Rook { IsBlack = true });
            PlacePiece(0, 1, new Knight { IsBlack = true });
            PlacePiece(0, 2, new Bishop { IsBlack = true });
            PlacePiece(0, 3, new Queen { IsBlack = true });
            PlacePiece(0, 4, new King { IsBlack = true });
            PlacePiece(0, 5, new Bishop { IsBlack = true });
            PlacePiece(0, 6, new Knight { IsBlack = true });
            PlacePiece(0, 7, new Rook { IsBlack = true });

            // Black pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(1, c, new Pawn { IsBlack = true });

            // White pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(6, c, new Pawn { IsBlack = false });

            // White back rank
            PlacePiece(7, 0, new Rook { IsBlack = false });
            PlacePiece(7, 1, new Knight { IsBlack = false });
            PlacePiece(7, 2, new Bishop { IsBlack = false });
            PlacePiece(7, 3, new Queen { IsBlack = false });
            PlacePiece(7, 4, new King { IsBlack = false });
            PlacePiece(7, 5, new Bishop { IsBlack = false });
            PlacePiece(7, 6, new Knight { IsBlack = false });
            PlacePiece(7, 7, new Rook { IsBlack = false });
        }
        public List<(int x, int y)> GetValidMoves(int row, int col) 
        {
            var piece = GameState.BoardState.Squares[row, col];
            return piece.GetValidMoves(GameState, row, col);
        }
        public bool MovePiece(int fromX, int fromY, int toX, int toY)
        {
            var currentPiece = GameState.BoardState.Squares[fromX, fromY];

            if (currentPiece == null)
            { 
                return false;
            }

            if (currentPiece.IsBlack != GameState.IsBlackTurn || !currentPiece.IsBlack != GameState.IsWhiteTurn)
            {
                return false;
            }
            var targetMove = (toX, toY);

            var validMoves = currentPiece.GetValidMoves(GameState, fromX, fromY);

            if (currentPiece != null && validMoves.Contains(targetMove))
            {
                if (GameState.BoardState.Squares[toX, toY] != null)
                {
                    CapturePiece(toX, toY);
                }
                PlacePiece(toX, toY, currentPiece);
                PlacePiece(fromX, fromY, null);

                GameState.IsBlackTurn = !GameState.IsBlackTurn;
                GameState.IsWhiteTurn = !GameState.IsWhiteTurn;

                GameState.Moves.Add(new Move
                {
                    MovedPiece = currentPiece,
                    From = (fromX, fromY),
                    To = (toX, toY)
                });

                return true;
            }
            return false;
        }
        public bool IsFirstMove(IPiece piece, GameState state)
        {
            return !state.Moves.Any(m => m.MovedPiece == piece);
        }

        private void CapturePiece(int x, int y)
        {
            // Handle piece capture logic here (e.g., add to captured pieces list, update score, etc.)
        }

        private void PlacePiece(int x, int y, IPiece? piece)
        {
            GameState.BoardState.Squares[x, y] = piece;
        }
    }
}
