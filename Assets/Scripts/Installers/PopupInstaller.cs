using UnityEngine;
using Zenject;

public class PopupInstaller : MonoInstaller
{
    [SerializeField] private Popup.TaskPopup _taskPopupPrefab;
    [SerializeField] private Canvas _popupCanvas;

    public override void InstallBindings()
    {
        DontDestroyOnLoad(_popupCanvas.gameObject);
        
        var taskPopup = Container
            .InstantiatePrefabForComponent<Popup.TaskPopup>(
                _taskPopupPrefab,
                _popupCanvas.transform
            );
        
        var controller = Container.Resolve<IPopupController>();
        controller.RegisterPopup(taskPopup);
    }
}