using Checkup.App.Services;
using Checkup.Core.Common;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Microsoft.Maui.Graphics;
using System.ComponentModel;

namespace Checkup.App.ViewModels;

public class SquareViewModel : INotifyPropertyChanged
{
    public SquareViewModel(int row, int col)
    {
        Row = row;
        Col = col;
    }
    public int Row { get; }
    public int Col { get; }

    private IPiece? _piece;
    public IPiece? Piece
    {
        get => _piece;
        set
        {
            if (_piece != value)
            {
                _piece = value;
                OnPropertyChanged(nameof(Piece));
                OnPropertyChanged(nameof(PieceImage));
            }
        }
    }

    public string? PieceImage => Piece == null ? null : PieceImageProvider.GetImagePath(Piece);

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
    }

    // Computed background color used by the UI binding
    public Color BackgroundColor => IsSelected ? Colors.Orange : ((Row + Col) % 2 == 0 ? Colors.Gray : Colors.Beige);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}