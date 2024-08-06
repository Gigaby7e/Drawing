using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Modules.SaveLoad.Scripts
{
    public class SaveLoadController : MonoBehaviour
    {
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _clearButton;
    
        private void OnEnable()
        {
            _saveButton.onClick.AddListener(() => DrawingEvents.SaveDrawingProgress?.Invoke());
            _loadButton.onClick.AddListener(() => DrawingEvents.LoadDrawingProgress?.Invoke());
            _clearButton.onClick.AddListener(() => DrawingEvents.ClearDrawingProgress?.Invoke());
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveAllListeners();
            _loadButton.onClick.RemoveAllListeners();
        }

        public class Factory : PlaceholderFactory<SaveLoadController>
        {
        }
    }
}
