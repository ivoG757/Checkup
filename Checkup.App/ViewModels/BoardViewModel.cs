//using Android.Hardware.Lights;
using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
using System.ComponentModel;
using System.Net.NetworkInformation;
//using static Android.Provider.DocumentsContract;

namespace Checkup.App.ViewModels;

public class BoardViewModel : INotifyPropertyChanged
{
    public Board Board => ChessEngine.Board;
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
    }
    public void ClickedSquare(int x, int y)
    {
        this.ChessEngine.ClickedSquare(x, y);
        OnPropertyChanged(nameof(this.Board));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}