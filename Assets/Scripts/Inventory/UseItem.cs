using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void ActionNeedItem(Item item)
    {
        switch (item.ActionT)
        {
            case Item.ActionType.Regeneration:

                _player.CurrentHealth += 10;

                if (_player.CurrentHealth > _player.MaxHealth)
                {
                    _player.CurrentHealth = _player.MaxHealth;
                }
                break;
        }
    }
}
