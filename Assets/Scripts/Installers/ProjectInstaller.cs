using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPopupController>()
            .To<PopupController>()
            .AsSingle();
    }
}