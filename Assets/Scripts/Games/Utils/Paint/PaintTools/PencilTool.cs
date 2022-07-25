using FreeDraw;

namespace Games.Utils.Paint.PaintTools
{
    public class PencilTool : PaintTool
    {
        public override void Initialize(DrawingSettings settings)
        {
            settings.SetMarkerWidth(1);
            settings.SetMarkerColour(ColorPaletteExtension.GetColorFromCode(ColorPalette.Black));
        }
    }
}