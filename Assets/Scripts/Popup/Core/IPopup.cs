using Cysharp.Threading.Tasks;

public interface IPopup
{
    IPopupController Controller { get; }
    UniTask Show();
    UniTask Hide();
    void Initialize(IPopupController controller);
}