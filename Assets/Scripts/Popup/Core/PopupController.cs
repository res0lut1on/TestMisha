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
    private readonly Queue<(Popup.TaskPopup popup, TaskPopupVM vm)> _queue = new();
    private bool _isShowing;

    public void RegisterPopup(Popup.Popup popup)
    {
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

    public UniTask ShowPopup(Popup.TaskPopup popup, TaskPopupVM vm)
    {
        if (_isShowing)
        {
            _queue.Enqueue((popup, vm));
            return UniTask.CompletedTask;
        }

        return ShowNext(popup, vm);
    }

    private async UniTask ShowNext(Popup.TaskPopup popup, TaskPopupVM vm)
    {
        _isShowing = true;
        popup.Setup(vm);
        await popup.Show();

        var tcs = new UniTaskCompletionSource();
        OnAllPopupClosed += OnClosed;

        void OnClosed()
        {
            OnAllPopupClosed -= OnClosed;
            tcs.TrySetResult();
        }

        await tcs.Task;
        _isShowing = false;

        if (_queue.Count > 0)
        {
            var next = _queue.Dequeue();
            await ShowNext(next.popup, next.vm);
        }
    }

    public void AddActivePopup(Popup.Popup popup)
    {
        if (_activePopups.Contains(popup)) return;
        _activePopups.Add(popup);
        OnPopupOpened?.Invoke();
    }

    public void RemoveActivePopup(Popup.Popup popup)
    {
        _activePopups.Remove(popup);
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