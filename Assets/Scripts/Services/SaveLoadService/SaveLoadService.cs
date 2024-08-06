using System.IO;
using UnityEngine;

namespace Services.SaveLoadService
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string SavedDrawingProgressPath = "SavedDrawingProgress";

        public void Initialize()
        {
            //check saved texture for enabling load button at start
        }

        public void Save(Texture2D texture)
        {
            var path = Application.persistentDataPath + SavedDrawingProgressPath;
            
            File.WriteAllBytes(path, texture.EncodeToPNG());
        }

        public Texture2D Load(string filePath = null)
        {
            var path = Application.persistentDataPath + SavedDrawingProgressPath;

            var bytes = File.ReadAllBytes(path);
            return ConvertBytesToTexture(bytes);
        }

        private Texture2D ConvertBytesToTexture(byte[] bytes)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            return texture;
        }
    }
}
