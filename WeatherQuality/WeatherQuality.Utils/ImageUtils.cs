using System.Drawing;
using System.Globalization;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Brushes = SixLabors.ImageSharp.Drawing.Processing.Brushes;
using Color = SixLabors.ImageSharp.Color;
using Font = SixLabors.Fonts.Font;
using FontFamily = SixLabors.Fonts.FontFamily;
using FontStyle = SixLabors.Fonts.FontStyle;
using PointF = SixLabors.ImageSharp.PointF;
using SystemFonts = SixLabors.Fonts.SystemFonts;

namespace WeatherQuality;

public static class ImageUtils
{
    private static readonly IImageEncoder PngEncoder = new PngEncoder
    {
        BitDepth = PngBitDepth.Bit8,
        CompressionLevel = PngCompressionLevel.BestCompression,
        TransparentColorMode = PngTransparentColorMode.Preserve
    };

    public static byte[] PlaceText(string srcImagePath, string text, float size, Color color, int xOffset, int yOffset)
    {
        using var stream = new MemoryStream();
        using var img = SixLabors.ImageSharp.Image.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            srcImagePath));

        FontCollection collection = new();
        collection.Add("Fonts/ArialRegular.ttf");
        var font = collection.Families.FirstOrDefault().CreateFont(size);
 
        img.Mutate(x => x.DrawText(text, font, color, new PointF
        {
            X = xOffset,
            Y = yOffset
        }));

        img.Save(stream, PngEncoder);

        return stream.ToArray();
    }
}