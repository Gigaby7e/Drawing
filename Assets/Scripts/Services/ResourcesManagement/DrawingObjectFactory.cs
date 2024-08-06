using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.ResourcesManagement
{
    public class DrawingObjectFactory : IDrawingObjectFactory
    {
        private const string SpawnPointTag = "Respawn";
        private const string DrawingPrefabKey = "DrawableObject";
        
        private readonly IAddressablesService _addressablesService;

        public DrawingObjectFactory(IAddressablesService addressablesService)
        {
            _addressablesService = addressablesService;
        }

        public async UniTask<GameObject> CreateDrawableObject()
        {
            var spawnPoint = GameObject.FindGameObjectWithTag(SpawnPointTag);
            var drawingObject = await Create(DrawingPrefabKey, spawnPoint.transform);
            
            drawingObject.transform.localPosition = Vector3.zero;
            drawingObject.transform.localRotation = Quaternion.identity;
            
            return drawingObject;
        }

        public async UniTask<GameObject> Create(string prefabPath, Transform parent)
        {
            var gameObject = await _addressablesService.InstantiatePrefabAsync(prefabPath, Vector3.zero, Quaternion.identity);
            gameObject.transform.SetParent(parent);
            return gameObject;
        }
    }
}