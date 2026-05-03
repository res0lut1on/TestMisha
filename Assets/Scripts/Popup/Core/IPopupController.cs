using Cysharp.Threading.Tasks;
using System;

public interface IPopupController
{
    event Action OnAllPopupClosed;
    event Action OnPopupOpened;
    
    UniTask HideLastPopup();
    UniTask HideAllPopups();
    
    T GetPopup<T>() where T : Popup.Popup;
    void AddActivePopup(Popup.Popup popup);
    void RemoveActivePopup(Popup.Popup popup);
    void RegisterPopup(Popup.Popup popup);
    UniTask ShowPopup(Popup.TaskPopup popup, TaskPopupVM vm);
}