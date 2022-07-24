using FreeDraw;
using UnityEngine;

namespace Games.Utils.Paint.PaintTools
{
    public class EraserTool : PaintTool
    {
        public override void Initialize(DrawingSettings settings)
        {
            settings.SetMarkerColour(Color.white);
        }
    }
}