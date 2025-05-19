using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UseItem))]
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUIBasePanel; // панель инвентаря
    [SerializeField] private GameObject _inventoryUIFastPanel; // панель инвентаря

    [Header("Inventory Settings")]
    public int maxStackedItems = 4;
    public InventorySlot[] InventorySlots;
    public GameObject InventoryItemPrefab;

    private int _selectedSlot = -1;

    [Header("Item Database")]
    [SerializeField] private Item[] _itemDatabase;

    private Dictionary<string, Item> _itemDictionary = new Dictionary<string, Item>(); // словарь для быстрого поиска
    private UseItem _useItem;

    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // делаем объект неуничтожаемым при загрузке новой сцены

        InitializeItemDictionary();
        if (_useItem == null) _useItem = GetComponent<UseItem>();
    }

    private void Start()
    {

        ChangeSelectedSlot(0);
        _inventoryUIFastPanel.SetActive(true);
    }

    private void Update()
    {
        OpenBaseInventoryInATab();
        
    }

    public bool AddItem(Item item)
    {
        if (item == null) return false;

        // попытка добавить в существующий стак
        if (item.Stackable)
        {
            for (int i = 0; i < InventorySlots.Length; i++)
            {
                InventoryItem itemInSlot = InventorySlots[i].GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && 
                    itemInSlot.ItemInventory.ID == item.ID && 
                    itemInSlot.Count < maxStackedItems)
                {
                    itemInSlot.Count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
            }
        }

        // ищем пустой слот
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i].GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnNewItem(item, InventorySlots[i]);
                return true;
            }
        }

        Debug.LogWarning("Инвентарь полон!");
        return false;
    }

    public Item GetSelectedItem(bool use)
    {
        if (_selectedSlot < 0 || _selectedSlot >= InventorySlots.Length) return null;

        InventorySlot slot = InventorySlots[_selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot == null) return null;

        Item item = itemInSlot.ItemInventory;

        if (use)
        {
            itemInSlot.Count--;
            if (itemInSlot.Count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.RefreshCount();
            }
        }

        return item;

    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (_selectedSlot >= 0 && _selectedSlot < InventorySlots.Length) InventorySlots[_selectedSlot].Deselect();

        if (newValue >= 0 && newValue < InventorySlots.Length)
        {
            _selectedSlot = newValue;
            InventorySlots[_selectedSlot].Select();
        }
    }

    public void InventoryChoiceAndAoolicationTheItem(int id)
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);

            if (isNumber && number > 0 && number <= 7)
            {
                ChangeSelectedSlot(number - 1);
            }

            // Использование предмета
            if (Input.GetKeyDown(KeyCode.R) && InventorySlots[number] != null)
            {
                _useItem.ActionNeedItem(_itemDatabase[id]);
                GetSelectedItem(true);
            }
        }
    }
    public void InitializeItemDictionary()
    {
        _itemDictionary.Clear();
        
        foreach (Item item in _itemDatabase)
        {
            if (item != null && !string.IsNullOrEmpty(item.ID))
            {
                if (!_itemDictionary.ContainsKey(item.ID))
                {
                    _itemDictionary.Add(item.ID, item);
                }
                else
                {
                    Debug.LogWarning($"Duplicate item ID found: {item.ID}"); // если ID дублируется
                }
            }   
        }
    }

    public InventorySaveData GetSaveData()
    {
        InventorySaveData saveData = new InventorySaveData
        {
            selectedSlot = _selectedSlot,
            isBasePanelOpen = _inventoryUIBasePanel.activeSelf
        };

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = InventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                saveData.items.Add(new InventoryItemSaveData
                {
                    itemID = itemInSlot.ItemInventory.ID,
                    slotIndex = i,
                    count = itemInSlot.Count
                });
            }
        }

        return saveData;
    }

    public void LoadSaveData(InventorySaveData saveData)
    {
        if (_itemDictionary == null || _itemDictionary.Count == 0)
        {
            InitializeItemDictionary(); // Повторная инициализация
        }

        if (saveData == null) return;

        // очищаем инвентарь
        foreach (InventorySlot slot in InventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                Destroy(itemInSlot.gameObject);
            }
        }

        // восстанавливаем предметы
        foreach (var itemData in saveData.items)
        {
            if (_itemDictionary.TryGetValue(itemData.itemID, out Item item))
            {
                if (itemData.slotIndex < InventorySlots.Length)
                {
                    SpawnNewItem(item, InventorySlots[itemData.slotIndex], itemData.count);
                }
            }
        }

        // восстанавливаем UI
        ChangeSelectedSlot(saveData.selectedSlot);
        _inventoryUIBasePanel.SetActive(saveData.isBasePanelOpen);
    }
    public int GetItemDictionaryCount()
    {
        return _itemDictionary.Count;
    }

    private void SpawnNewItem(Item item, InventorySlot slot, int count = 1)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();

        inventoryItem.InitialliseItem(item);
        inventoryItem.Count = count;
        inventoryItem.RefreshCount();
    }

    private void OpenBaseInventoryInATab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _inventoryUIBasePanel.SetActive(!_inventoryUIBasePanel.activeSelf);
        }
    }

}

