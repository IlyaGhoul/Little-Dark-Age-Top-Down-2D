using Examples.AbstractFactoryExample.Unit.Warrior;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    // данные игрока
    public Vector2 PlayerPosition;
    public int Health;
    public int MaxHealth;

    // Инвентарь
    public InventorySaveData inventoryData;

    // Состояние врагов
    //public EnemiesSaveData enemiesData;
    //public WeakWarrior weakWarrior;

    // временное сохранение врага, который преследует игрока в первой локации
    // public int HealthEnemy;
    // public Vector2 EnemyPosition;

    public List<EnemySaveData> EnemiesData;

}

[System.Serializable]
public class InventoryItemSaveData
{
    public string itemID;       // Уникальный идентификатор предмета
    public int slotIndex;       // Индекс слота (0-15)
    public int count;           // Количество предметов в стаке
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemSaveData> items = new List<InventoryItemSaveData>();
    public int selectedSlot;    // Текущий выбранный слот
    public bool isBasePanelOpen;
}

// возможно
[System.Serializable]
public class EnemySaveData
{
    public string EnemyId;  
    public int Health;
    public Vector3 Position;
    public bool IsDefeated;
}

//[System.Serializable]
//public class EnemiesSaveData
//{
//    public List<EnemySaveData> enemies = new List<EnemySaveData>();
//}




