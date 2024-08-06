using Services.SaveLoadService;
using UnityEngine;

public class TexturePainter : MonoBehaviour
{
    private const string BrushPos = "_BrushPos";
    private const string BrushSize = "_BrushSize";
    private const string BrushColor = "_BrushColor";
    
    public RenderTexture renderTexture;
    public Color brushColor = Color.red;
    public float brushSize = 1f;
    public Shader brushShader;

    private Camera cam;
    private Material brushMaterial;
    private RenderTexture tempRenderTexture;
    private Vector2 lastUV;
    private bool isPainting = false;
    
    private ISaveLoadService _saveLoadService;

    void Start()
    {
        cam = Camera.main;
        
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
            renderTexture.Create();
        }
        
        OnClearTexture();

        brushMaterial = new Material(brushShader);
        brushMaterial.SetColor(BrushColor, brushColor);

        tempRenderTexture = new RenderTexture(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGB32);
        tempRenderTexture.Create();

        GetComponent<Renderer>().material.mainTexture = renderTexture;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    Renderer rend = hit.collider.GetComponent<Renderer>();
                    MeshCollider meshCollider = hit.collider as MeshCollider;

                    if (rend != null && rend.sharedMaterial != null && rend.sharedMaterial.mainTexture != null && meshCollider != null)
                    {
                        Vector2 uv = hit.textureCoord;

                        if (!isPainting)
                        {
                            lastUV = uv;
                            isPainting = true;

                            DrawCircle(uv);
                        }
                        else
                        {
                            DrawLine(lastUV, uv);
                            lastUV = uv;
                        }
                    }
                }
            }
        }
        else if (isPainting)
        {
            isPainting = false;
        }
    }

    private void DrawLine(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        int steps = Mathf.CeilToInt(distance / (brushSize / renderTexture.width));

        for (int i = 0; i <= steps; i++)
        {
            Vector2 interpolated = Vector2.Lerp(start, end, (float)i / steps);
            DrawCircle(interpolated);
        }
    }

    private void DrawCircle(Vector2 uv)
    {
        brushMaterial.SetVector(BrushPos, new Vector4(uv.x, uv.y, 0, 0));
        brushMaterial.SetFloat(BrushSize, brushSize);

        Graphics.Blit(renderTexture, tempRenderTexture, brushMaterial);
        Graphics.Blit(tempRenderTexture, renderTexture);
    }

    public void SetBrushSize(float size)
    {
        brushSize = size;
        brushMaterial.SetFloat(BrushSize, brushSize);
    }

    public void SetBrushColor(Color color)
    {
        brushColor = color;
        brushMaterial.SetColor(BrushColor, brushColor);
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

    public void Initialize(ISaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
        
        DrawingEvents.SaveDrawingProgress += OnSaveTexture;
        DrawingEvents.LoadDrawingProgress += OnLoadedTexture;
        DrawingEvents.ClearDrawingProgress += OnClearTexture;
    }

}
