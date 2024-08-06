using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Modules.ColorPalette.Scripts
{
    public class BrushController : MonoBehaviour
    {
        public Action<Color> ColorChanged;
        public Action<float> SizeChanged;

        [Header("Size settings")]
        [SerializeField] private Slider _sizeSlider;
        
        [ Space(10), Header("Color settings")]
        [SerializeField] private Image _currentColorImage;
        [SerializeField] private Image _colorPaletteImage;

        private RectTransform _colorPaletteTransform;
        private float _textureHeight;
        private float _textureWidth;

        private void Start()
        {
            _colorPaletteTransform = _colorPaletteImage.GetComponent<RectTransform>();
            _textureHeight = _colorPaletteImage.sprite.texture.height;
            _textureWidth = _colorPaletteImage.sprite.texture.width;
        }
        
        public void PickColorFromPalette()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_colorPaletteTransform, Input.mousePosition, null, out var localCursor);

            if (!RectTransformUtility.RectangleContainsScreenPoint(_colorPaletteTransform, Input.mousePosition, null)) 
                return;

            var rect = _colorPaletteTransform.rect;
            var x = (localCursor.x - rect.x) * _textureWidth / rect.width;
            var y = (localCursor.y - rect.y) * _textureHeight / rect.height;

            var texture = _colorPaletteImage.sprite.texture;
            var pixelColor = texture.GetPixel((int)x, (int)y);
                    
            if(pixelColor.a == 0) 
                return;

            _currentColorImage.color = pixelColor;
            ColorChanged?.Invoke(pixelColor);
        }
        
        private void OnEnable()
        {
            _sizeSlider.onValueChanged.AddListener(OnSizeChanged);
        }

        private void OnDisable()
        {
            _sizeSlider.onValueChanged.RemoveAllListeners();
        }

        private void OnSizeChanged(float size)
        {
            SizeChanged?.Invoke(size);
        }
        
        public class Factory : PlaceholderFactory<BrushController>
        {
        }
    }
}
