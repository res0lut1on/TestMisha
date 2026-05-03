using Zenject;

public class TaskPopupRegistrationInitializer : IInitializable
{
    private readonly PopupController _popupController;
    private readonly Popup.TaskPopup _taskPopup;

    public TaskPopupRegistrationInitializer(PopupController popupController, Popup.TaskPopup taskPopup)
    {
        _popupController = popupController;
        _taskPopup = taskPopup;
    }

    public void Initialize()
    {
        _popupController.RegisterPopup(_taskPopup);
    }
}
