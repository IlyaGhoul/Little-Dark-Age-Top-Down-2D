using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string enemyID;    
    public string enemyName;
    public int enemyHealth;
    public int enemyMaxHealth;
    public int enemyDamageAmount;
}
