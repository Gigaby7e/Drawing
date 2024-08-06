using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.ResourcesManagement
{
    public interface IDrawingObjectFactory
    {
        UniTask<GameObject> CreateDrawableObject();
        
        UniTask<GameObject> Create(string prefabName, Transform parent);
    }
}