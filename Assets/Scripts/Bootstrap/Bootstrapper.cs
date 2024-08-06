using Drawing;
using Modules.ColorPalette.Scripts;
using Modules.SaveLoad.Scripts;
using Services.SaveLoadService;
using Zenject;

namespace Bootstrap
{
    public class Bootstrapper : IInitializable
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IDrawingManager _drawingManager;
        private readonly IBrushManager _brushManager;
        private readonly BrushController.Factory _brushControllerFactory;
        private readonly SaveLoadController.Factory _saveLoadControllerFactory;

        public Bootstrapper(
            ISaveLoadService saveLoadService,
            IDrawingManager drawingManager,
            IBrushManager brushManager,
            BrushController.Factory brushControllerFactory,
            SaveLoadController.Factory saveLoadControllerFactory)
        {
            _saveLoadService = saveLoadService;
            _drawingManager = drawingManager;
            _brushManager = brushManager;
            _brushControllerFactory = brushControllerFactory;
            _saveLoadControllerFactory = saveLoadControllerFactory;
        }

        public void Initialize()
        {
            InitializeServices();
            InitializeManagers();
        }
        
        private void InitializeServices()
        {
            _saveLoadService.Initialize();
        }

        private void InitializeManagers()
        {
            _drawingManager.Initialize().Forget();
            
            var brushController = _brushControllerFactory.Create();
            _brushManager.Initialize(brushController);
            
            var saveLoadController = _saveLoadControllerFactory.Create();
        }
    }
}