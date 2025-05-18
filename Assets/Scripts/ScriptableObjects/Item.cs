using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string ID;
    public TileBase Tile;
    public Sprite Image;
    public string Name;

    public ItemType ItemT;
    public ActionType ActionT;

    public Vector2Int Range = new Vector2Int(5, 4);

    public bool Stackable = true;

    public enum ItemType
    {
        Tool,
        Buff
    }

    public enum ActionType
    {
        Regeneration
    }   
}
