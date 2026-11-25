namespace Cr.Maui.Controls;

public sealed partial class HorizontalLineWrapper : LineWrapper
{
    protected override BindableProperty LineLengthProperty => HeightRequestProperty;

    protected override void ConfigureGridLayout(Grid grid, IView firstLine, IView secondLine, IView presenter)
    {
        grid.RowDefinitions =
        [
            new(GridLength.Auto),
            new(GridLength.Star),
            new(GridLength.Auto)
        ];

        grid.ColumnDefinitions =
        [
            new(GridLength.Star)
        ];

        grid.Add(firstLine, column: 0, row: 0);
        grid.Add(presenter, column: 0, row: 1);
        grid.Add(secondLine, column: 0, row: 2);
    }
}