using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

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

        protected void Awake()
        {
            _yesButton.onClick.AddListener(OnYesClicked);
            _noButton.onClick.AddListener(OnNoClicked);

            for (int i = 0; i < _selectorButtons.Length; i++)
            {
                int index = i;
                _selectorButtons[i].onClick.AddListener(() => SelectItem(index));
            }
        }

        private void OnDestroy()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
            foreach (var btn in _selectorButtons)
                btn.onClick.RemoveAllListeners();
        }

        public void Setup(TaskPopupVM vm)
        {
            _vm = vm;
            _titleText.text = vm.Title;
            
            SelectItem(0);
        }

        private void SelectItem(int index)
        {
            _selectedIndex = index;
            var item = _vm.Items[index];
            
            _itemIcon.sprite = item.Icon;
            _itemDescription.text = item.Description;
            _yesPriceText.text = $"x{item.Price}";
        }

        private void OnYesClicked()
        {
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