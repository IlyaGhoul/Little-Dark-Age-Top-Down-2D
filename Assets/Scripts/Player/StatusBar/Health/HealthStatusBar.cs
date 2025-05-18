using UnityEngine;
using UnityEngine.UI;

public class HealthStatusBar : MonoBehaviour
{
    [SerializeField] private Image _healthBar;  

    private float _maxHealth;

    private void Start()
    {
        _maxHealth = Player.Instance.MaxHealth;
    }

    private void Update()
    {
        HealthBarStatus(Player.Instance.CurrentHealth);
    }

    private void HealthBarStatus(float currentHealth)
    {
        if (currentHealth >= 0)
        {
            float health = currentHealth / _maxHealth;
            _healthBar.fillAmount = health;
        }      
    }
}
