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
    private static string _saveFolder = Path.Combine("D:/StudioGame/W/SAVE");

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
        Player player = Player.Instance;
        GameSaveData saveData = new GameSaveData();  
        
        // заполняем данные игрока 
        saveData.PlayerPosition = player.transform.position;
        saveData.Health = player.MaxHealth;
        saveData.MaxHealth = player.MaxHealth;

        // инвентарь
        saveData.inventoryData = InventoryManager.Instance.GetSaveData();

        // преследуюущий скелет

        //EnemySO enemySO = new EnemySO();
        //if (EnemyAI.Instance.shouldBeSaved)
        //{
        //    saveData.HealthEnemy = enemySO.enemyMaxHealth;
        //}

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
                    Health = enemyPair.Value.CurrentHealth,
                };
                saveData.EnemiesData.Add(enemyData);
                Debug.Log(enemyData);
            }
        }


        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_savePath, json);

        Debug.Log("Игра сохранена");
        Debug.Log("Сохранение в: " + Application.persistentDataPath + "/inventory_save.json");
    }

    public static bool LoadGame()
    {
        if (!File.Exists(_savePath))
        {
            Debug.Log("Файл сохранения не найден");
            return false;
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

        // преследуюущий скелет

        //if (EnemyAI.Instance.shouldBeSaved)
        //{
        //    WeakWarrior weakWarrior = new WeakWarrior();
        //    weakWarrior.CurrentHealth = saveData.HealthEnemy;
        //    EnemyAI.Instance.transform.position = new Vector2(14, 4);
        //}


        // Восстанавливаем врагов
        if (saveData.EnemiesData != null)
        {
            foreach (var enemyData in saveData.EnemiesData)
            {
                if (Instance._enemyUnits.TryGetValue(enemyData.EnemyId, out var unit))
                {
                    Debug.Log(enemyData.EnemyId);

                    // Особые условия для врага с ID "1"
                    if (enemyData.EnemyId == "1")
                    {
                        unit.CurrentHealth = unit.EnemySO.enemyMaxHealth;
                        unit.transform.position = new Vector3(14f, 4f, 0f); // Пример координат
                        unit.DetectLive();

                        SlaveSkeletonVisual.Instance.IsDeath = false;
                    }
                    else
                    {
                        // Обычная загрузка для других врагов
                        unit.CurrentHealth = enemyData.Health;
                    }

                    
                }
                else
                {
                    Debug.LogWarning($"Enemy with ID {enemyData.EnemyId} not found in registry!");
                }
            }
        }

        Debug.Log("Сохранение загружено!");
        return true;
    }
}
