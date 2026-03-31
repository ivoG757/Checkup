using Checkup.Core.Models.Interfaces;
using System.ComponentModel;
using Microsoft.Maui.Graphics;

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
                OnPropertyChanged(nameof(Symbol));
            }
        }
    }

    public string Symbol => Piece?.Symbol.ToString() ?? "";

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
    public Color BackgroundColor => IsSelected ? Colors.Orange : ((Row + Col) % 2 == 0 ? Colors.DarkGray : Colors.Black);

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}