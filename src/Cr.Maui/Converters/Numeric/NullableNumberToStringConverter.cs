using System.Globalization;

namespace Cr.Maui.Converters;

[AcceptEmptyServiceProvider]
public abstract class NullableNumberToStringConverter<TNumber> : ValueConverterExtension where TNumber : struct, IFormattable
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TNumber number)
        {
            return string.Empty;
        }

        return number.ToString(format: null, culture);
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text)
        {
            return Binding.DoNothing;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        return TryParse(text, culture, out var result) ? result : Binding.DoNothing;
    }

    protected abstract bool TryParse(string text, CultureInfo culture, out TNumber result);
}