using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackPreset", menuName = "Combat/Attack Preset")]
public class AttackColliderPreset : ScriptableObject
{
    public string AnimationName;
    public Vector2[] ColliderPoints;
}
