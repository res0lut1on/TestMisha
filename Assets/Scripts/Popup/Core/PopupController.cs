using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : IPopupController
{
    public event Action OnAllPopupClosed;
    public event Action OnPopupOpened;

    private readonly List<Popup.Popup> _activePopups = new();
    private readonly Dictionary<Type, Popup.Popup> _registry = new();

    public void RegisterPopup(Popup.Popup popup)
    {
        if (popup == null)
        {
            Debug.LogError("[PopupController] Popup is null and cannot be registered.");
            return;
        }

        popup.Initialize(this);
        _registry[popup.GetType()] = popup;
    }

    public T GetPopup<T>() where T : Popup.Popup
    {
        if (_registry.TryGetValue(typeof(T), out var popup))
            return (T)popup;

        Debug.LogError($"[PopupController] {typeof(T).Name} is not registered");
        return null;
    }

    public void AddActivePopup(Popup.Popup popup)
    {
        if (_activePopups.Contains(popup)) return;
        _activePopups.Add(popup);
        OnPopupOpened?.Invoke();
    }

    public void RemoveActivePopup(Popup.Popup popup)
    {
        if (!_activePopups.Remove(popup)) return;

        if (_activePopups.Count == 0)
            OnAllPopupClosed?.Invoke();
    }

    public async UniTask HideLastPopup()
    {
        if (_activePopups.Count == 0) return;
        await _activePopups[^1].Hide();
    }

    public async UniTask HideAllPopups()
    {
        var copy = new List<Popup.Popup>(_activePopups);
        foreach (var popup in copy)
            await popup.Hide();
    }
}
