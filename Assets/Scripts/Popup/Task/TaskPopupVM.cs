using System;
using System.Linq;

public class TaskPopupVM
{
    public const int ItemsCount = 3;

    public string Title { get; }
    public ItemData[] Items { get; }
    public Action<ItemData> OnConfirm { get; }

    public TaskPopupVM(
        string title,
        ItemData[] items,
        Action<ItemData> onConfirm)
    {
        if (items == null || items.Length != ItemsCount)
            throw new ArgumentException($"[TaskPopupVM] items must contain exactly {ItemsCount} items.");

        if (items.Any(item => item == null))
            throw new ArgumentException("[TaskPopupVM] items cannot contain null values.");

        if (items.Distinct().Count() != ItemsCount)
            throw new ArgumentException("[TaskPopupVM] items must be different.");

        Title = title ?? string.Empty;
        Items = items.ToArray();
        OnConfirm = onConfirm;
    }
}
