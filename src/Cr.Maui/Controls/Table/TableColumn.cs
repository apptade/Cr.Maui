namespace Cr.Maui.Controls;

public partial class TableColumn : BindableObject
{
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(
            propertyName: nameof(Header),
            returnType: typeof(IView),
            declaringType: typeof(TableColumn));

    public static readonly BindableProperty BodyTemplateProperty =
        BindableProperty.Create(
            propertyName: nameof(BodyTemplate),
            returnType: typeof(DataTemplate),
            declaringType: typeof(TableColumn));

    public IView? Header
    {
        get => (IView?)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public DataTemplate? BodyTemplate
    {
        get => (DataTemplate?)GetValue(BodyTemplateProperty);
        set => SetValue(BodyTemplateProperty, value);
    }
}