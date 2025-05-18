using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StaminaStatusBar : MonoBehaviour
{
    [SerializeField] private Image _staminaBar;
    [SerializeField] private Player _player;

    private void Update()
    {
        _staminaBar.fillAmount = _player.CurrentStamina / _player.MaxStamina;
    }
}
