using System;
using UnityEngine;

namespace Drawing
{
    public static class DrawingEvents
    {
        public static Action<Color> BrushColorChanged;
        public static Action<float> BrushSizeChanged;
    
        public static Action SaveDrawingProgress;
        public static Action LoadDrawingProgress;
        public static Action ClearDrawingProgress;
    }
}