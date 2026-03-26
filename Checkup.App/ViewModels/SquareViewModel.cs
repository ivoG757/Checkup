
using Checkup.Core.Models.Interfaces;
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
                OnPropertyChanged(nameof(Symbol));
            }
        }
    }

    public string Symbol => Piece?.Symbol.ToString() ?? "";

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}