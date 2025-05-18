using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Image _image;
    [SerializeField] private Color _selectedColor, _notSelectedColor;

    private const string TRASH_BIT = "TrashBin";

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        _image.color = _selectedColor;
    }

    public void Deselect()
    {
        _image.color = _notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
        if (draggableItem == null) return;

        if (gameObject.CompareTag(TRASH_BIT) )
        {
            Destroy(dropped);
            return;
        }

        if (transform.childCount == 0)
        {      
            draggableItem.ParentAfterDrag = transform;
        }
    }
}
