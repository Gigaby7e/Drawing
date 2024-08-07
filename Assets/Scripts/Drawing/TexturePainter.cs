using Services.SaveLoadService;
using UnityEngine;

namespace Drawing
{
    public class TexturePainter : MonoBehaviour
    {
        private const string BrushPos = "_BrushPos";
        private const string BrushSize = "_BrushSize";
        private const string BrushColor = "_BrushColor";
        private const int TextureWidth = 512;
        private const int TextureHeight = 512;
    
        [SerializeField] private Shader brushShader;
        [SerializeField] private float brushSize = 1f;
        [SerializeField] private Color brushColor = Color.red;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private float steps = 0;

        private Camera _cam;
        private Material _brushMaterial;
        private RenderTexture _tempRenderTexture;
        private Vector2 _lastUV;
        private bool _isPainting = false;
        private float _drawingSharpness = 100;
        
        private ISaveLoadService _saveLoadService;

        private void Start()
        {
            _cam = Camera.main;
        
            if (renderTexture == null)
            {
                renderTexture = new RenderTexture(TextureWidth, TextureHeight, 0, RenderTextureFormat.ARGB32);
                renderTexture.Create();
            }
        
            OnClearTexture();

            _brushMaterial = new Material(brushShader);
            _brushMaterial.SetColor(BrushColor, brushColor);

            _tempRenderTexture = new RenderTexture(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGB32);
            _tempRenderTexture.Create();

            GetComponent<Renderer>().material.mainTexture = renderTexture;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider != null)
                    {
                        Renderer rend = hit.collider.GetComponent<Renderer>();
                        MeshCollider meshCollider = hit.collider as MeshCollider;

                        if (rend != null && rend.sharedMaterial != null && rend.sharedMaterial.mainTexture != null && meshCollider != null)
                        {
                            Vector2 uv = hit.textureCoord;

                            if (!_isPainting)
                            {
                                _lastUV = uv;
                                _isPainting = true;

                                DrawCircle(uv);
                            }
                            else
                            {
                                DrawLine(_lastUV, uv);
                                _lastUV = uv;
                            }
                        }
                    }
                }
            }
            else if (_isPainting)
            {
                _isPainting = false;
            }
        }
        
        public void Initialize(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        
            DrawingEvents.SaveDrawingProgress += OnSaveTexture;
            DrawingEvents.LoadDrawingProgress += OnLoadedTexture;
            DrawingEvents.ClearDrawingProgress += OnClearTexture;
        }
        
        public void SetBrushSize(float size)
        {
            brushSize = size;
            _brushMaterial.SetFloat(BrushSize, brushSize);
        }

        public void SetBrushColor(Color color)
        {
            brushColor = color;
            _brushMaterial.SetColor(BrushColor, brushColor);
        }
        
        private void DrawLine(Vector2 start, Vector2 end)
        {
            var distance = Vector2.Distance(start, end);
            steps = Mathf.CeilToInt(distance / (brushSize / renderTexture.width)) / _drawingSharpness;

            for (var i = 0; i <= steps; i++)
            {
                var interpolated = Vector2.Lerp(start, end, (float)i / steps);
                DrawCircle(interpolated);
            }
        }

        private void DrawCircle(Vector2 uv)
        {
            _brushMaterial.SetVector(BrushPos, new Vector4(uv.x, uv.y, 0, 0));
            _brushMaterial.SetFloat(BrushSize, brushSize);

            Graphics.Blit(renderTexture, _tempRenderTexture, _brushMaterial);
            Graphics.Blit(_tempRenderTexture, renderTexture);
        }

        private void OnSaveTexture()
        {
            var texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
        
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            RenderTexture.active = null;
        
            _saveLoadService.Save(texture2D);
        }
    
        private void OnLoadedTexture()
        {
            var texture = _saveLoadService.Load();
            Graphics.Blit(texture, renderTexture);
        }
    
        private void OnClearTexture()
        {
            RenderTexture.active = renderTexture;

            GL.Clear(true, true, Color.white);
        
            RenderTexture.active = null;
        }
    }
}
