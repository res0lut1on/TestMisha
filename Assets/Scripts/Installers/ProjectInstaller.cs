using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private Popup.TaskPopup _taskPopupPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PopupController>()
            .AsSingle();

        Container.Bind<ITaskPopupService>()
            .To<TaskPopupService>()
            .AsSingle();

        var popupCanvas = CreatePopupCanvas();
        var taskPopup = Container.InstantiatePrefabForComponent<Popup.TaskPopup>(
            _taskPopupPrefab,
            popupCanvas.transform
        );

        Container.Bind<Popup.TaskPopup>()
            .FromInstance(taskPopup)
            .AsSingle();

        Container.BindInterfacesTo<TaskPopupRegistrationInitializer>()
            .AsSingle();
    }

    private static Canvas CreatePopupCanvas()
    {
        var popupCanvasObject = new GameObject(
            "PopupCanvas",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );

        Object.DontDestroyOnLoad(popupCanvasObject);

        var canvas = popupCanvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;

        var scaler = popupCanvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }
}
