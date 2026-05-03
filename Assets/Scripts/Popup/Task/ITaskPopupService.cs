using Cysharp.Threading.Tasks;

public interface ITaskPopupService
{
    UniTask Show(TaskPopupVM vm);
}
