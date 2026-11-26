namespace Cr.Maui.Converters;

[AcceptEmptyServiceProvider]
public sealed class NullableIntToStringConverter : NullableNumberToStringConverter<int>
{
    protected override bool TryParse(string text, CultureInfo culture, out int result)
    {
        return int.TryParse(text, NumberStyles.Integer, culture, out result);
    }
}