namespace Cr.Maui.Converters;

[AcceptEmptyServiceProvider]
public sealed class NullableDecimalToStringConverter : NullableNumberToStringConverter<decimal>
{
    protected override bool TryParse(string text, CultureInfo culture, out decimal result)
    {
        return decimal.TryParse(text, NumberStyles.Number, culture, out result);
    }
}