using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public ChessEngineService()
        {
            SetPiecesOnBoard();
        }
        public Board Board { get; set; } = new Board();

        public void SetPiecesOnBoard()
        {
            PlacePiece(0, 0, new Rook { IsBlack = true });
            //PlacePiece(0, 1, new Knight { IsBlack = true });
            //PlacePiece(0, 2, new Bishop { IsBlack = true });
            //PlacePiece(0, 3, new Queen { IsBlack = true });
            //PlacePiece(0, 4, new King { IsBlack = true });
            //PlacePiece(0, 5, new Bishop { IsBlack = true });
            //PlacePiece(0, 6, new Knight { IsBlack = true });
            PlacePiece(0, 7, new Rook { IsBlack = true });

            // Black pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(1, c, new Pawn { IsBlack = true });

            // White pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(6, c, new Pawn { IsBlack = false });

            // White back rank
            PlacePiece(7, 0, new Rook { IsBlack = false });
            //PlacePiece(7, 1, new Knight { IsBlack = false });
            //PlacePiece(7, 2, new Bishop { IsBlack = false });
            //PlacePiece(7, 3, new Queen { IsBlack = false });
            //PlacePiece(7, 4, new King { IsBlack = false });
            //PlacePiece(7, 5, new Bishop { IsBlack = false });
            //PlacePiece(7, 6, new Knight { IsBlack = false });
            PlacePiece(7, 7, new Rook { IsBlack = false });
        }
        public bool MovePiece(int fromX, int fromY, int toX, int toY)
        {
            var currentPiece = Board.Squares[fromX, fromY];
            if (currentPiece == null) { return false; }

            var targetMove = (toX, toY);

            var validMoves = currentPiece.GetValidMoves(Board, fromX, fromY);

            if (currentPiece != null && validMoves.Contains(targetMove))
            {
                if (Board.Squares[toX, toY] != null)
                {
                    //CapturePiece
                }
                PlacePiece(toX, toY, currentPiece);
                PlacePiece(fromX, fromY, null);
                return true;
            }
            return false;
        }

        private void PlacePiece(int x, int y, IPiece? piece)
        {
            Board.Squares[x, y] = piece;
        }
    }
}
