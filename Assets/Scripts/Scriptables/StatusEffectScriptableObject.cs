using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "ScriptableObjects/StatusEffectScriptableObject")]
public class StatusEffectScriptableObject : ScriptableObject
{
    public string statusEffectId;
    public string statusEffectName;
    public string statusEffectDescription;
    public Sprite statusEffectIcon;

}
