using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TaskPopupService : ITaskPopupService
{
    private readonly IPopupController _popupController;
    private readonly Queue<Request> _queue = new();
    private bool _isProcessing;

    public TaskPopupService(IPopupController popupController)
    {
        _popupController = popupController;
    }

    public UniTask Show(TaskPopupVM vm)
    {
        if (vm == null)
            throw new ArgumentNullException(nameof(vm));

        var completionSource = new UniTaskCompletionSource();
        _queue.Enqueue(new Request(vm, completionSource));

        if (!_isProcessing)
            ProcessQueue().Forget();

        return completionSource.Task;
    }

    private async UniTaskVoid ProcessQueue()
    {
        _isProcessing = true;

        while (_queue.Count > 0)
        {
            var request = _queue.Dequeue();

            try
            {
                await ShowInternal(request.Vm);
                request.CompletionSource.TrySetResult();
            }
            catch (Exception exception)
            {
                request.CompletionSource.TrySetException(exception);
            }
        }

        _isProcessing = false;
    }

    private async UniTask ShowInternal(TaskPopupVM vm)
    {
        var popup = _popupController.GetPopup<Popup.TaskPopup>();
        if (popup == null) return;

        var closedSource = new UniTaskCompletionSource();

        try
        {
            _popupController.OnAllPopupClosed += OnClosed;

            popup.Setup(vm);
            await popup.Show();
            await closedSource.Task;
        }
        finally
        {
            _popupController.OnAllPopupClosed -= OnClosed;
        }

        void OnClosed()
        {
            closedSource.TrySetResult();
        }
    }

    private readonly struct Request
    {
        public readonly TaskPopupVM Vm;
        public readonly UniTaskCompletionSource CompletionSource;

        public Request(TaskPopupVM vm, UniTaskCompletionSource completionSource)
        {
            Vm = vm;
            CompletionSource = completionSource;
        }
    }
}
