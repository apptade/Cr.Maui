namespace Cr.Maui.Controls;

public abstract partial class LineWrapper : ContentView
{
    public static readonly BindableProperty LineColorProperty =
        BindableProperty.Create(
            propertyName: nameof(LineColor),
            returnType: typeof(Color),
            declaringType: typeof(LineWrapper),
            defaultValue: Colors.Gray.WithAlpha(0.1f));

    public static readonly BindableProperty LineThicknessProperty =
        BindableProperty.Create(
            propertyName: nameof(LineThickness),
            returnType: typeof(double),
            declaringType: typeof(LineWrapper),
            defaultValue: 0.5d);

    protected LineWrapper()
    {
        ControlTemplate = new(CreateGridTemplate);
    }

    public Color LineColor
    {
        get => (Color)GetValue(LineColorProperty);
        set => SetValue(LineColorProperty, value);
    }

    public double LineThickness
    {
        get => (double)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    protected abstract BindableProperty LineLengthProperty { get; }

    protected abstract void ConfigureGridLayout(Grid grid, IView firstLine, IView secondLine, IView presenter);

    private Grid CreateGridTemplate()
    {
        var grid = new Grid();
        var firstLine = new BoxView();
        var secondLine = new BoxView();
        var presenter = new ContentPresenter();

        ConfigureLineBindings(firstLine);
        ConfigureLineBindings(secondLine);
        ConfigureGridLayout(grid, firstLine, presenter, secondLine);

        return grid;
    }

    private void ConfigureLineBindings(BoxView line)
    {
        line.SetBinding(BoxView.ColorProperty, new Binding(path: nameof(LineColor), source: RelativeBindingSource.TemplatedParent));
        line.SetBinding(LineLengthProperty, new Binding(path: nameof(LineThickness), source: RelativeBindingSource.TemplatedParent));
    }
}