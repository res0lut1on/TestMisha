using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskPopupVM
{
    public string Title { get; }
    public string SomeText { get; }
    public Sprite SideImage { get; }
    public ItemData[] Items { get; }
    public Action<ItemData> OnConfirm { get; }

    public TaskPopupVM(
        string title,
        string someText,
        Sprite sideImage,
        ItemData[] items,
        Action<ItemData> onConfirm)
    {
        if (items == null || items.Length < 3)
            throw new ArgumentException("[TaskPopupVM] items must contain 3 items.");
        
        Title = title;
        SomeText = someText;
        SideImage = sideImage;
        Items = items;
        OnConfirm = onConfirm;

        Items = items
            .OrderBy(_ => Random.value)
            .Take(3)
            .ToArray();
    }
}