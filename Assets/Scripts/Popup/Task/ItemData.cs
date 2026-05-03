using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Popups/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
}
