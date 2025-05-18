using Examples.AbstractFactoryExample.Unit.Warrior;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    // ������ ������
    public Vector2 PlayerPosition;
    public int Health;
    public int MaxHealth;

    // ���������
    public InventorySaveData inventoryData;

    // ��������� ������
    //public EnemiesSaveData enemiesData;
    //public WeakWarrior weakWarrior;

    // ��������� ���������� �����, ������� ���������� ������ � ������ �������
    // public int HealthEnemy;
    // public Vector2 EnemyPosition;

    public List<EnemySaveData> EnemiesData;

}

[System.Serializable]
public class InventoryItemSaveData
{
    public string itemID;       // ���������� ������������� ��������
    public int slotIndex;       // ������ ����� (0-15)
    public int count;           // ���������� ��������� � �����
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventoryItemSaveData> items = new List<InventoryItemSaveData>();
    public int selectedSlot;    // ������� ��������� ����
    public bool isBasePanelOpen;
}

// ��������
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




