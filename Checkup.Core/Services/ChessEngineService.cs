using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public ChessEngineService()
        {
            SetPiecesOnBoard();
        }
        public Board Board { get; set; } = new Board(); 

        private IPiece _selectedPiece { get; set; }

        public IPiece SelectedPiece
        {
            get => _selectedPiece;
            set { _selectedPiece = value; }
        }
        public void SetPiecesOnBoard()
        {
            IPiece whitePawn = new Pawn { IsBlack = false };
            //PlacePiece(0, 0, new Rook { IsBlack = true });
            //PlacePiece(0, 1, new Knight { IsBlack = true });
            //PlacePiece(0, 2, new Bishop { IsBlack = true });
            //PlacePiece(0, 3, new Queen { IsBlack = true });
            //PlacePiece(0, 4, new King { IsBlack = true });
            //PlacePiece(0, 5, new Bishop { IsBlack = true });
            //PlacePiece(0, 6, new Knight { IsBlack = true });
            //PlacePiece(0, 7, new Rook { IsBlack = true });

            // Black pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(1, c, new Pawn { IsBlack = true });

            // White pawns
            for (int c = 0; c < 8; c++)
                PlacePiece(6, c, new Pawn { IsBlack = false });

            // White back rank
            //PlacePiece(7, 0, new Rook { IsBlack = false });
            //PlacePiece(7, 1, new Knight { IsBlack = false });
            //PlacePiece(7, 2, new Bishop { IsBlack = false });
            //PlacePiece(7, 3, new Queen { IsBlack = false });
            //PlacePiece(7, 4, new King { IsBlack = false });
            //PlacePiece(7, 5, new Bishop { IsBlack = false });
            //PlacePiece(7, 6, new Knight { IsBlack = false });
            //PlacePiece(7, 7, new Rook { IsBlack = false });
        }
        public void ClickedSquare(int x, int y)
        {
            Console.WriteLine($"Clicked {x},{y}");
            var clickedSquare = Board.Squares[x, y];
            if (clickedSquare != null)
            {
                SelectedPiece = clickedSquare;
            }
            if (SelectedPiece != null && clickedSquare == null)
            {
                PlacePiece(x, y, SelectedPiece);
            }

        }
        public void PlacePiece(int x, int y, IPiece piece)
        {
            Console.WriteLine($"Clicked {x},{y}");

            Board.Squares[x, y] = piece;
        }
    }
}
