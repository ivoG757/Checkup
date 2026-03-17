using Android.Hardware.Lights;
using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
using System.ComponentModel;
using System.Net.NetworkInformation;
using static Android.Provider.DocumentsContract;

namespace Checkup.App.ViewModels;

public class BoardViewModel : INotifyPropertyChanged
{
    public IChessEngineService ChessEngine { get; }
    public List<SquareViewModel> Squares { get; } = new();
    public BoardViewModel(IChessEngineService service)
    {
        ChessEngine = service;
        
        //for (int row = 0; row < 8; row++)
        //{
        //    for (int col = 0; col < 8; col++)
        //    {
        //        Squares.Add(new SquareViewModel(row, col));
        //    }
        //}

        SetPiecesOnBoard();
    }
    public void SetPiecesOnBoard() // TODO: This should be moved to the ChessEngine and the ViewModel should interact with the ChessEngine to get the board state.
    {
        IPiece whitePawn = new Pawn { IsBlack = false };
        // Black back rank
        //PlacePiece(0, 0, new Rook { IsBlack = true });
        //PlacePiece(0, 1, new Knight { IsBlack = true });
        //PlacePiece(0, 2, new Bishop { IsBlack = true });
        //PlacePiece(0, 3, new Queen { IsBlack = true });
        //PlacePiece(0, 4, new King { IsBlack = true });
        //PlacePiece(0, 5, new Bishop { IsBlack = true });
        //PlacePiece(0, 6, new Knight { IsBlack = true });
        //PlacePiece(0, 7, new Rook { IsBlack = true });

        //// Black pawns
        //for (int c = 0; c < 8; c++)
        //    PlacePiece(1, c, new Pawn { IsBlack = true });

        //// White pawns
        //for (int c = 0; c < 8; c++)
        //    PlacePiece(6, c, new Pawn { IsBlack = false });

        //// White back rank
        //PlacePiece(7, 0, new Rook { IsBlack = false });
        //PlacePiece(7, 1, new Knight { IsBlack = false });
        //PlacePiece(7, 2, new Bishop { IsBlack = false });
        //PlacePiece(7, 3, new Queen { IsBlack = false });
        //PlacePiece(7, 4, new King { IsBlack = false });
        //PlacePiece(7, 5, new Bishop { IsBlack = false });
        //PlacePiece(7, 6, new Knight { IsBlack = false });
        //PlacePiece(7, 7, new Rook { IsBlack = false });
    }
    public Board Board { get; set; } = new Board(); // TODO: This should be moved to the ChessEngine and the ViewModel should interact with the ChessEngine to get the board state.

    private IPiece _selectedPiece { get; set; }

    public IPiece SelectedPiece
    {
        get => _selectedPiece;
        set { _selectedPiece = value; OnPropertyChanged(nameof(SelectedPiece)); }
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
        OnPropertyChanged(nameof(Board));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}