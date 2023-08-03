using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [SerializeField] private List<StatusEffectScriptableObject> statusEffectScriptableObjects;
    private List<StatusEffectScriptableObject> activeStatusEffects = new List<StatusEffectScriptableObject>();
    private StatusEffectUIController uiController;
    private StatusEffectBehaviours behaviours;
    private void Start()
    {
        uiController = GetComponent<StatusEffectUIController>();
        behaviours = GetComponent<StatusEffectBehaviours>();
    }

    private StatusEffectScriptableObject GetObjectOfStatusEffect(List<StatusEffectScriptableObject> list, string statusEffectId)
    {
        foreach(StatusEffectScriptableObject statusEffect in list)
        {
            if(statusEffect.statusEffectId == statusEffectId) return statusEffect;
        }

        return null;
    }

    public void AddStatusEffect(string statusEffectId, bool isActive, float statusEffectModifier = 0)
    {
        StatusEffectScriptableObject selectedStatusEffect = GetObjectOfStatusEffect(statusEffectScriptableObjects, statusEffectId);
        if (selectedStatusEffect == null || GetObjectOfStatusEffect(activeStatusEffects, statusEffectId) != null) return;

        activeStatusEffects.Add(selectedStatusEffect);
        uiController.UpdateStatusEffectsUI(activeStatusEffects);

        if (isActive)
        {
            switch (statusEffectId)
            {
                case "Toxicated":
                    behaviours.AddToxication();
                    break;
                case "PowerSurge":
                    behaviours.ModifyFlashlightPower((float)statusEffectModifier);
                    break;
                default:
                    Debug.LogError("Not supposed to be here.");
                    break;
            }
        }
    }

    public void RemoveStatusEffect(string statusEffectId, bool isActive, float statusEffectModifier = 0)
    {
        StatusEffectScriptableObject selectedStatusEffect = GetObjectOfStatusEffect(statusEffectScriptableObjects, statusEffectId);

        if (selectedStatusEffect == null || GetObjectOfStatusEffect(activeStatusEffects, statusEffectId) == null) return;
        activeStatusEffects.Remove(selectedStatusEffect);
        uiController.UpdateStatusEffectsUI(activeStatusEffects);

        if(isActive)
        {
            switch(statusEffectId)
            {
                case "Toxicated":
                    behaviours.RemoveToxication();
                    break;
                case "PowerSurge":
                    behaviours.ModifyFlashlightPower(statusEffectModifier);
                    break;
                default:
                    Debug.LogError("Not supposed to be here.");
                    break;
            }
        }
    }

    public void ResetAllStatusEffects()
    {
        activeStatusEffects.Clear();
        uiController.UpdateStatusEffectsUI(activeStatusEffects);
    }
}
