using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    [SerializeField] private Image _imageInventoryItem;
    public Text _countText;

    [SerializeField] Canvas _canvas;
    [SerializeField] Camera _uiCamera;  

    [HideInInspector] public Transform ParentAfterDrag;
    [HideInInspector] public Item ItemInventory;
    [HideInInspector] public int Count = 1;

    public void InitialliseItem(Item newItem)
    {
        ItemInventory = newItem;
        
        _imageInventoryItem.sprite = newItem.Image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        if (_countText != null)
        {
            _countText.text = Count.ToString();

            bool textActive = Count > 1;
            _countText.gameObject.SetActive(textActive);
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _imageInventoryItem.raycastTarget = false;             
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _canvas.transform as RectTransform,
            Input.mousePosition,
            _canvas.worldCamera,
            out Vector3 worldPoint
        );
        
        transform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        _imageInventoryItem.raycastTarget = true;
    }
    
}
