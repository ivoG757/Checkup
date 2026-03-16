
using Checkup.Core.Models.Interfaces;
using System.ComponentModel;


namespace Checkup.App.ViewModels;

public class SquareViewModel : INotifyPropertyChanged
{
    public int Row { get; }
    public int Column { get; }

    private IPiece _piece;
   

    public IPiece Piece
    {
        get => _piece;
        set
        {
            _piece = value;
            OnPropertyChanged(nameof(Piece));
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