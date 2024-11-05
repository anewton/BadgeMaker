using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Avalonia.Media;

namespace BadgeMaker;

public partial class BadgeInfo
{
    private const int TextPadding = 6;
    private const double TextWidthScaleFactor = 0.1;

    [GeneratedRegex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
    private static partial Regex ValidHexRegex();

    private static string s_namedColorsJson =
        @"{""blue"": ""#007ec6"",
          ""brightgreen"": ""#4c1"",
          ""green"": ""#97ca00"",
          ""yellow"": ""#dfb317"",
          ""yellowgreen"": ""#a4a61d"",
          ""orange"": ""#fe7d37"",
          ""red"": ""#e05d44"",
          ""grey"": ""#555"",
          ""lightgrey"": ""#9f9f9f""}";

    private readonly Dictionary<string, string> _namedColors = JsonSerializer.Deserialize<Dictionary<string, string>>(s_namedColorsJson);

    public BadgeInfo(string leftText, string rightText, string rightForegroundColor)
    {
        LeftText = leftText;
        RightText = rightText;
        RightForegroundColor = ParseColor(rightForegroundColor);
    }

    internal string LeftText { get; private set; }
    internal string RightText { get; private set; }
    internal string RightForegroundColor { get; private set; }
    internal double LeftTextWidth => GetTextWidth(LeftText);
    internal double RightTextWidth => GetTextWidth(RightText);

    internal string GetBadgeSvgString()
    {
        Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BadgeMaker.Assets.badgeTemplate.svg");
        using StreamReader streamReader = new StreamReader(resourceStream);
        string resourceString = streamReader.ReadToEnd();
        string svgBadgeString = resourceString
            .Replace("{LeftText}", LeftText)
            .Replace("{LeftTextWidth}", LeftTextWidth.ToString())
            .Replace("{RightText}", RightText)
            .Replace("{RightTextWidth}", RightTextWidth.ToString())
            .Replace("{RightForegroundColor}", RightForegroundColor)
            .Replace("{Width}", GetTotalBadgeWidth().ToString())
            .Replace("{LeftTextOffset}", GetLeftTextCenteringOffset().ToString())
            .Replace("{RightTextOffset}", GetRightTextCenteringOffset().ToString())
            .Replace("{LeftBadgeWidth}", GetLeftBadgeWidth().ToString())
            .Replace("{RightBadgeWidth}", GetRightBadgeWidth().ToString());
        return svgBadgeString;
    }

    internal double GetTotalBadgeWidth() => GetLeftBadgeWidth() + GetRightBadgeWidth();

    internal double GetLeftBadgeWidth()
    {
        double leftScaledWidth = LeftTextWidth * TextWidthScaleFactor;
        double leftWidth = TextPadding * 2 + leftScaledWidth;
        return leftWidth;
    }

    internal double GetRightBadgeWidth()
    {
        double rightScaledWidth = RightTextWidth * TextWidthScaleFactor;
        double rightWidth = TextPadding * 2 + rightScaledWidth;
        return rightWidth;
    }

    internal double GetLeftTextCenteringOffset()
    {
        double leftScaledWidth = LeftTextWidth * TextWidthScaleFactor;
        double leftWidth = TextPadding * 2 + leftScaledWidth;
        double upscaledLeftWidth = leftWidth / TextWidthScaleFactor;
        double rectCenter = upscaledLeftWidth / 2;
        return rectCenter;
    }

    internal double GetRightTextCenteringOffset()
    {
        double rightScaledWidth = RightTextWidth * TextWidthScaleFactor;
        double rightWidth = TextPadding * 2 + rightScaledWidth;

        double upscaledRightWidth = rightWidth / TextWidthScaleFactor;
        double rectCenter = upscaledRightWidth / 2;

        double leftScaledWidth = LeftTextWidth * TextWidthScaleFactor;
        double leftWidth = TextPadding * 2 + leftScaledWidth;

        double upscaledLeftWidth = leftWidth / TextWidthScaleFactor;

        return upscaledLeftWidth + rectCenter; ;
    }

    private string ParseColor(string hexColor)
    {
        string result = _namedColors["blue"];

        if (string.IsNullOrWhiteSpace(hexColor))
            return result;
        else if (_namedColors.ContainsKey(hexColor.ToLowerInvariant()))
            result = _namedColors[hexColor.ToLowerInvariant()];
        else if (ValidHexRegex().IsMatch(hexColor))
            result = hexColor;

        return result;
    }

    private static double GetTextWidth(string text)
    {
        FormattedText formattedText = GetFormattedText(text);
        return formattedText.Width;
    }

    private static FormattedText GetFormattedText(string text)
    {
        Typeface typeFace = new(new FontFamily("Verdana,Geneva,DejaVu Sans,sans-serif"));
        SolidColorBrush foregroundBrush = new(Colors.White);
        FormattedText formattedText = new(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, 110, foregroundBrush);
        return formattedText;
    }
}
