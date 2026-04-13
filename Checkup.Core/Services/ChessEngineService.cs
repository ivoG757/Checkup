using Checkup.Core.Models;
using Checkup.Core.Models;
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
            return piece.GetValidMoves(GameState, row, col);
        }
        private (int x, int y) _selectedPiece { get; set; }

        public BasePiece? GetPiece(int row, int col)
        {
            return GameState.BoardState.Squares[row, col];
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

                GameState.Moves.Add(new Move((fromX, fromY), (toX, toY))
                {
                    MovedPiece = currentPiece,
                });

                return true;
            }
            return false;
        }
        internal bool IsFirstMove(BasePiece piece, GameState state)
        {
            return !state.Moves.Any(m => m.MovedPiece == piece);
        }

        public void CapturePiece(int x, int y)
        {
           
        }

        internal void PlacePiece(int x, int y, BasePiece? piece)
        {
            GameState.BoardState.Squares[x, y] = piece;
        }
    }
}
