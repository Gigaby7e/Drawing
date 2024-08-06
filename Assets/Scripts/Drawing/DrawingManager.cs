using Cysharp.Threading.Tasks;
using Services.ResourcesManagement;
using Services.SaveLoadService;
using UnityEngine;

namespace Drawing
{
    public class DrawingManager : IDrawingManager
    {
        private readonly IDrawingObjectFactory _drawingObjectFactory;
        private readonly ISaveLoadService _saveLoadService;
        private TexturePainter _texturePainter;
        
        public DrawingManager(IDrawingObjectFactory drawingObjectFactory, ISaveLoadService saveLoadService)
        {
            _drawingObjectFactory = drawingObjectFactory;
            _saveLoadService = saveLoadService;
        }
        
        public async UniTaskVoid Initialize()
        {
            Subscribe();
            _texturePainter = await CreateDrawableObject();
            _texturePainter.Initialize(_saveLoadService);
        }
        
        private void Subscribe()
        {
            DrawingEvents.BrushSizeChanged += OnBrushSizeChanged;
            DrawingEvents.BrushColorChanged += OnBrushColorChanged;
        }

        private void Unsubscribe()
        {
            DrawingEvents.BrushSizeChanged -= OnBrushSizeChanged;
            DrawingEvents.BrushColorChanged -= OnBrushColorChanged;
        }

        private void OnBrushColorChanged(UnityEngine.Color color) => _texturePainter.SetBrushColor(color);

        private void OnBrushSizeChanged(float size) => _texturePainter.SetBrushSize(size);
        
        private async UniTask<TexturePainter> CreateDrawableObject()
        {
            var drawableObject = await _drawingObjectFactory.CreateDrawableObject();
            return drawableObject.GetComponent<TexturePainter>();
        }
    }
}