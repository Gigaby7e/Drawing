using UnityEngine;

namespace Services.SaveLoadService
{
    public interface ISaveLoadService
    {
        void Initialize();
        void Save(Texture2D filePath = null);
        Texture2D Load(string filePath = null);
    }
}