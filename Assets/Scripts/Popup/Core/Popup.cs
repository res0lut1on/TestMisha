using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popup
{
    public abstract class Popup : MonoBehaviour, IPopup
    {
        public IPopupController Controller { get; private set; }

        public void Initialize(IPopupController controller)
        {
            Controller = controller;
            gameObject.SetActive(false);
        }

        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            Controller.AddActivePopup(this);
            
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Hide()
        {
            gameObject.SetActive(false);
            Controller.RemoveActivePopup(this);
            
            await UniTask.CompletedTask;
        }
    }
}