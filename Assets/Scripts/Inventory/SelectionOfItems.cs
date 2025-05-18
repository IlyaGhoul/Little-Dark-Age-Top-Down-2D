using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionOfItems : MonoBehaviour
{
    public InventoryManager InventoryManager;
    public Item[] ItemsToPickup;

    private GameObject _itemInRange;
    private readonly string[] _itemsToPickup = new string[1] { "Health" };

    [SerializeField] private InventoryManager _inventoryManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _itemInRange != null)
        {
            PlayerSounds.Instance.PlayTakeItemSound();
            PickupItem(_itemInRange, 0);
        }

        _inventoryManager.InventoryChoiceAndAoolicationTheItem(0);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.CompareTag(_itemsToPickup[0]))
        {
            ShowActionTakeItem.Instance.ShowActionForTakeItem(0);

            _itemInRange = other.gameObject;
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag(_itemsToPickup[0]) && _itemInRange == other.gameObject)
        {
            ShowActionTakeItem.Instance.CloseActionForTakeItem();

            _itemInRange = null;

        }
    }

    private void PickupItem(GameObject item, int id)
    {
        if (ItemsToPickup[id].ActionT == Item.ActionType.Regeneration)
        {
            InventoryManager.AddItem(ItemsToPickup[id]);

            Sprite itemIcon = ItemsToPickup[id].Image;

            if (ItemPickUpUIController.Instance != null) ItemPickUpUIController.Instance.ShowItemPickup(ItemsToPickup[id].Name, itemIcon);

            Destroy(item);
        }
    }



}
