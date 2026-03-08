using Checkup.Core.Models.Enums;
using System.ComponentModel;
using ChessColor = Checkup.Core.Models.Enums.Color;

namespace Checkup.App.ViewModels;

public class SquareViewModel : INotifyPropertyChanged
{
    public int Row { get; }
    public int Column { get; }

    private Piece _piece;
    private ChessColor _color;

    public Piece Piece
    {
        get => _piece;
        set
        {
            _piece = value;
            OnPropertyChanged(nameof(Piece));
        }
    }

    public ChessColor Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged(nameof(Color));
        }
    }

    public SquareViewModel(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}