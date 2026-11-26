using XmlnsPrefixAttribute = Microsoft.Maui.Controls.XmlnsPrefixAttribute;

[assembly: XmlnsDefinition(Cr.Maui.XmlConfig.Namespace, "Cr.Maui.Controls")]
[assembly: XmlnsDefinition(Cr.Maui.XmlConfig.Namespace, "Cr.Maui.Converters")]

[assembly: XmlnsPrefix(Cr.Maui.XmlConfig.Namespace, "cr")]

namespace Cr.Maui;

internal static class XmlConfig
{
    internal const string Namespace = "http://schemas.crcore.com/dotnet/maui";
}