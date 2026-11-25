namespace Cr.Maui.Controls;

public sealed partial class VerticalLineWrapper : LineWrapper
{
    protected override BindableProperty LineLengthProperty => WidthRequestProperty;

    protected override void ConfigureGridLayout(Grid grid, IView firstLine, IView secondLine, IView presenter)
    {
        grid.ColumnDefinitions =
        [
            new(GridLength.Auto),
            new(GridLength.Star),
            new(GridLength.Auto)
        ];

        grid.RowDefinitions =
        [
            new(GridLength.Star)
        ];

        grid.Add(firstLine, column: 0, row: 0);
        grid.Add(presenter, column: 1, row: 0);
        grid.Add(secondLine, column: 2, row: 0);
    }
}