using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace Popup
{
    public class TaskPopup : Popup
    {
        [Header("Header")]
        [SerializeField] private TextMeshProUGUI _titleText;

        [Header("Item Display")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemDescription;

        [Header("Selector Buttons")]
        [SerializeField] private Button[] _selectorButtons;

        [Header("Bottom Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private TextMeshProUGUI _yesPriceText;
        [SerializeField] private Button _noButton;

        private TaskPopupVM _vm;
        private int _selectedIndex;
        private UnityAction[] _selectorListeners;

        protected void Awake()
        {
            _yesButton.onClick.AddListener(OnYesClicked);
            _noButton.onClick.AddListener(OnNoClicked);

            _selectorListeners = new UnityAction[_selectorButtons.Length];

            for (int i = 0; i < _selectorButtons.Length; i++)
            {
                int index = i;
                _selectorListeners[i] = () => SelectItem(index);
                _selectorButtons[i].onClick.AddListener(_selectorListeners[i]);
            }
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveListener(OnYesClicked);
            _noButton.onClick.RemoveListener(OnNoClicked);

            if (_selectorListeners == null) return;

            for (int i = 0; i < _selectorButtons.Length; i++)
                _selectorButtons[i].onClick.RemoveListener(_selectorListeners[i]);
        }

        public void Setup(TaskPopupVM vm)
        {
            _vm = vm;
            _titleText.text = vm.Title;
            
            SelectItem(0);
        }

        private void SelectItem(int index)
        {
            if (_vm == null || index < 0 || index >= _vm.Items.Length)
                return;

            _selectedIndex = index;
            var item = _vm.Items[index];
            
            _itemIcon.sprite = item.Icon;
            _itemIcon.enabled = item.Icon != null;
            _itemDescription.text = item.Description;
            _yesPriceText.text = $"x{item.Price}";
        }

        private void OnYesClicked()
        {
            if (_vm == null) return;

            _vm.OnConfirm?.Invoke(_vm.Items[_selectedIndex]);
            HideAsync().Forget();
        }

        private void OnNoClicked()
        {
            HideAsync().Forget();
        }

        private async UniTaskVoid HideAsync()
        {
            await Hide();
        }
    }
}
