using Checkup.Core.Models.Enums;
using Checkup.Core.Models;
using System.ComponentModel;
using ChessColor = Checkup.Core.Models.Enums.Color;

namespace Checkup.App.ViewModels;

public class BoardViewModel : INotifyPropertyChanged
{
    public List<SquareViewModel> Squares { get; } = new();
    public BoardViewModel()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Squares.Add(new SquareViewModel(row, col));
            }
        }
    }
    public Board Board { get; set; } = new Board();

    private Piece _selectedPiece = Piece.Pawn;
    private ChessColor _selectedColor = ChessColor.White;

    public Piece SelectedPiece
    {
        get => _selectedPiece;
        set { _selectedPiece = value; OnPropertyChanged(nameof(SelectedPiece)); }
    }

    public ChessColor SelectedColor
    {
        get => _selectedColor;
        set { _selectedColor = value; OnPropertyChanged(nameof(SelectedColor)); }
    }

    public void PlacePiece(int x, int y)
    {
        Board.Squares[x, y].Piece = SelectedPiece;
        Board.Squares[x, y].Color = SelectedColor;
        OnPropertyChanged(nameof(Board));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}