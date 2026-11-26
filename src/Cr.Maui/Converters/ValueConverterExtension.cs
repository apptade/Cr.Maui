using System.Globalization;

namespace Cr.Maui.Converters;

public abstract class ValueConverterExtension : IValueConverter, IMarkupExtension<IValueConverter>
{
    public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);
    public abstract object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);

    public IValueConverter ProvideValue(IServiceProvider serviceProvider) => this;
    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}