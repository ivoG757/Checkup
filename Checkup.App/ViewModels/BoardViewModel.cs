using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
using System.ComponentModel;

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
        PlacePiece(1, 2, whitePawn);
        PlacePiece(2, 2, whitePawn);
        //PlacePiece(3, 2, whitePawn);
        //PlacePiece(4, 2, whitePawn);
        //PlacePiece(5, 2, whitePawn);
        //PlacePiece(6, 2, whitePawn);
        //PlacePiece(7, 2, whitePawn);
        //PlacePiece(8, 2, whitePawn);
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