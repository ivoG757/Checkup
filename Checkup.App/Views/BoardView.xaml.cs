using Checkup.App.ViewModels;

namespace Checkup.App.Views;

public partial class BoardView : ContentView
{
    private readonly BoardViewModel _viewModel;

    public BoardView()
    {
        InitializeComponent();

        _viewModel = new BoardViewModel();

        BuildGrid();
    }

    private void BuildGrid()
    {
        for (int i = 0; i < 8; i++)
        {
            BoardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var square = new BoxView();

                if ((row + col) % 2 == 0)
                {
                    square.Color = Colors.Beige;
                }
                else
                {
                    square.Color = Colors.Brown;
                }

                Grid.SetRow(square, row);
                Grid.SetColumn(square, col);

                BoardGrid.Children.Add(square);
            }
        }
    }
}