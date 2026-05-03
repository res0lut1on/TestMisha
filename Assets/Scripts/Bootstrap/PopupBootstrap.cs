using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

public class PopupBootstrap : MonoBehaviour
{
    [Inject] private IPopupController _popupController;
    
    [SerializeField] private Sprite _sideImage;
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
        
        var vm = new TaskPopupVM(
            title: "Title",
            someText: "Some text",
            sideImage: _sideImage,
            items: _allItems,
            onConfirm: OnItemConfirmed
        );

        var popup = _popupController.GetPopup<Popup.TaskPopup>();
        await _popupController.ShowPopup(popup, vm);
    }

    private void OnItemConfirmed(ItemData selected)
    {
        Debug.Log($"[Bootstrap] Selected ID: {selected.Id}");
    }
}