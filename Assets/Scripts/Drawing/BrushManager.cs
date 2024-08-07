using Modules.ColorPalette.Scripts;
using UnityEngine;
using Zenject;

namespace Drawing
{
    public class BrushManager : IBrushManager, ITickable
    {
        private BrushController _brushController;

        public void Initialize(BrushController brushController)
        {
            _brushController = brushController;

            _brushController.SizeChanged += OnSizeChanged;
            _brushController.ColorChanged += OnColorChanged;
        }

        private void OnColorChanged(Color color) => DrawingEvents.BrushColorChanged(color);

        private void OnSizeChanged(float size) => DrawingEvents.BrushSizeChanged(size);

        public void Tick() => _brushController.PickColorFromPalette();
    }
}