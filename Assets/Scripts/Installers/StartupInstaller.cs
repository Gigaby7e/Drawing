using Zenject;
using Bootstrap;
using Drawing;
using Modules.ColorPalette.Scripts;
using Modules.SaveLoad.Scripts;
using Services.ResourcesManagement;
using Services.SaveLoadService;
using UnityEngine;

namespace Installers
{
    public class StartupInstaller : MonoInstaller
    {
        [SerializeField] private GameObject SaveLoadUIPrefab;
        [SerializeField] private GameObject BrushControllerPrefab;
        [SerializeField] private GameObject DrawableObjectPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<IAddressablesService>().To<AddressablesService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            
            Container.Bind<IDrawingObjectFactory>().To<DrawingObjectFactory>().AsSingle();
            Container.BindFactory<BrushController, BrushController.Factory>()
                .FromComponentInNewPrefab(BrushControllerPrefab);
            Container.BindFactory<SaveLoadController, SaveLoadController.Factory>()
                .FromComponentInNewPrefab(SaveLoadUIPrefab);

            Container.BindInterfacesAndSelfTo<BrushManager>().AsSingle();

            Container.Bind<IDrawingManager>().To<DrawingManager>().AsSingle();
            Container.Bind<IInitializable>().To<Bootstrapper>().AsSingle().NonLazy();
            
        }
    }
}