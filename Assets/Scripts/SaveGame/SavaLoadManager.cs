using System;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor.Overlays; // Только для редактора
#endif
using Examples.AbstractFactoryExample.Unit.Warrior;
using Examples.AbstractFactoryExample.Unit;
using System.Collections.Generic;


public class SaveLoadManager : MonoBehaviour
{
    private static string _saveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Little dark age");

    private static string _savePath => Path.Combine(_saveFolder, "gamesave.json");

    private Dictionary<string, Unit> _enemyUnits = new Dictionary<string, Unit>();

    public static SaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    private void Start()
    {
        // Временный тест - вывести всех зарегистрированных врагов
        foreach (var pair in _enemyUnits)
        {
            Debug.Log($"Зарегистрирован враг ID: {pair.Key}, Объект: {pair.Value.name}");
        }

        GameSaveData saveData = new GameSaveData();
    }

    public void RegisterEnemyUnit(Unit unit)
    {
        if (unit == null || unit.EnemySO == null)
        {
            Debug.LogError("Враг или EnemySO не назначены!", unit);
            return;
        }

        string enemyId = unit.EnemySO.enemyID;

        if (string.IsNullOrEmpty(enemyId))
        {
            Debug.LogError($"У врага {unit.name} нет enemyID в EnemySO!", unit);
            return;
        }

        _enemyUnits[enemyId] = unit; // Просто перезаписываем, если уже есть
    }

    public void UnregisterEnemyUnit(string enemyId)
    {
        if (_enemyUnits.ContainsKey(enemyId))
        {
            _enemyUnits.Remove(enemyId);
        }
    }

    public static void SaveGame()
    {
        // Если файла сохранения нет - создаем новое сохранение
        if (!File.Exists(_savePath))
        {
            CreateNewSave();
            return;
        }

        Player player = Player.Instance;
        GameSaveData saveData = new GameSaveData();  
        
        // заполняем данные игрока 
        saveData.PlayerPosition = player.transform.position;
        saveData.Health = player.MaxHealth;
        saveData.MaxHealth = player.MaxHealth;

        // инвентарь
        saveData.inventoryData = InventoryManager.Instance.GetSaveData();

        Debug.Log($"Сохранение врагов. Количество: {Instance._enemyUnits.Count}");

        // Сохраняем врагов
        saveData.EnemiesData = new List<EnemySaveData>();
        foreach (var enemyPair in Instance._enemyUnits)
        {
            if (enemyPair.Value != null)
            {
                var enemyData = new EnemySaveData
                {
                    EnemyId = enemyPair.Key,
                    Health = enemyPair.Value.MaxHealth,
                    MaxHealth = enemyPair.Value.MaxHealth,
                };
                saveData.EnemiesData.Add(enemyData);
                Debug.Log(enemyData);
            }
        }

        saveData.FirstSave = false;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_savePath, json);

        Debug.Log("Игра сохранена");
        Debug.Log("Сохранение в: " + Application.persistentDataPath + "/inventory_save.json");
    }

    public static bool LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Файл сохранения не найден. Создаем новое сохранение...");
            CreateNewSave();
        }

        // Загружаем данные
        string json = File.ReadAllText(_savePath);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        if (saveData == null)
        {
            return false;
        }

        if (Player.Instance == null)
        {
            Debug.LogError("Player не найден после загрузки сцены!");
            return false;
        }

        // Применяем данные к игроку
        Player.Instance.transform.position = saveData.PlayerPosition;
        Player.Instance.CurrentHealth = saveData.Health;
        Player.Instance.MaxHealth = saveData.MaxHealth;



        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager не найден!");
            return false;
        }

        // Загружаем инвентарь
        InventoryManager.Instance.LoadSaveData(saveData.inventoryData);

        // Восстанавливаем врагов
        if (saveData.EnemiesData != null)
        {
            foreach (var enemyData in saveData.EnemiesData)
            {
                if (Instance._enemyUnits.TryGetValue(enemyData.EnemyId, out var unit))
                {
                    Debug.Log(enemyData.EnemyId);

                    unit.CurrentHealth = saveData.MaxHealth;

                    // Особые условия для врага с ID "1"
                    if (enemyData.EnemyId == "1")
                    {                     
                        unit.transform.position = new Vector3(14f, 4f, 0f); // Пример координат
                        unit.DetectLive();

                        try
                        {
                            SlaveSkeletonVisual.Instance.IsDeath = false;
                        }
                        catch 
                        {
                            Debug.Log("First Save (maybe will be fixed)");
                        }
                    }
                                    
                }
                else
                {
                    Debug.LogWarning($"Enemy with ID {enemyData.EnemyId} not found in registry!");
                }
            }
        }

        
        GameManager.Instance.FirstSave = saveData.FirstSave;
        Debug.Log("Сохранение загружено!");
        return true;
    }

    // Метод для создания нового сохранения
    private static void CreateNewSave()
    {
        // Создаем папку для сохранений, если ее нет
        if (!Directory.Exists(_saveFolder))
        {
            Directory.CreateDirectory(_saveFolder);
        }

        GameSaveData newSave = new GameSaveData()
        {
            // Устанавливаем значения по умолчанию
            PlayerPosition = new Vector3(-3.5f, -0.5f, 0f),
            Health = 20,
            MaxHealth = 20,
            inventoryData = new InventorySaveData(), // или InventoryManager.CreateDefaultInventory()
            FirstSave = true
        };

        // Сохраняем врагов
        newSave.EnemiesData = new List<EnemySaveData>();
        foreach (var enemyPair in Instance._enemyUnits)
        {
            if (enemyPair.Value != null)
            {
                var enemyData = new EnemySaveData
                {
                    EnemyId = enemyPair.Key,
                    Health = enemyPair.Value.MaxHealth,
                    MaxHealth = enemyPair.Value.MaxHealth,
                    Position = enemyPair.Value.transform.position,
                };
                newSave.EnemiesData.Add(enemyData);
                Debug.Log(enemyData);
            }
        }


        string json = JsonUtility.ToJson(newSave, true);
        File.WriteAllText(_savePath, json);

        Debug.Log("Создано новое сохранение по умолчанию");
    }
}
