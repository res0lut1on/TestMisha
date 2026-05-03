using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PopupBootstrap : MonoBehaviour
{
    [Inject] private ITaskPopupService _taskPopupService;

    [SerializeField] private ItemData[] _allItems;

    private void Start() => ShowTaskPopup().Forget();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowTaskPopup().Forget();
    }

    private async UniTaskVoid ShowTaskPopup()
    {
        await UniTask.Yield();

        var items = GetRandomItems();
        if (items.Length != TaskPopupVM.ItemsCount) return;

        var vm = new TaskPopupVM(
            title: "Title",
            items: items,
            onConfirm: OnItemConfirmed
        );

        await _taskPopupService.Show(vm);
    }

    private ItemData[] GetRandomItems()
    {
        var items = (_allItems ?? Array.Empty<ItemData>())
            .Where(item => item != null)
            .OrderBy(_ => Random.value)
            .Take(TaskPopupVM.ItemsCount)
            .ToArray();

        if (items.Length == TaskPopupVM.ItemsCount)
            return items;

        Debug.LogError($"[PopupBootstrap] At least {TaskPopupVM.ItemsCount} items are required to show TaskPopup.");
        return Array.Empty<ItemData>();
    }

    private void OnItemConfirmed(ItemData selected)
    {
        Debug.Log($"[Bootstrap] Selected ID: {selected.Id}");
    }
}
