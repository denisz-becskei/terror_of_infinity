using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    [SerializeField] private List<StatusEffectScriptableObject> statusEffectScriptableObjects;
    public static List<StatusEffectScriptableObject> activeStatusEffects = new();

    public static Dictionary<StatusEffectScriptableObject, int> activeStackableEffects = new();
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

    public void AddStatusEffect(string statusEffectId, bool isActive, bool isStackable = false, int modifyStack = 0, float statusEffectModifier = 0)
    {
        StatusEffectScriptableObject selectedStatusEffect = GetObjectOfStatusEffect(statusEffectScriptableObjects, statusEffectId);
        if (selectedStatusEffect == null || GetObjectOfStatusEffect(activeStatusEffects, statusEffectId) != null) return;

        if (!isStackable)
        {
            activeStatusEffects.Add(selectedStatusEffect);
        }
        else
        {
            if (modifyStack > 0)
            {
                IncreaseStatusEffectStack(selectedStatusEffect);
            }
            else if (modifyStack < 0)
            {
                DecreaseStatusEffectStack(selectedStatusEffect);
            }
        }
        uiController.UpdateStatusEffectsUI(activeStatusEffects, activeStackableEffects);

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
                case "Darkness":
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
        try
        {
            activeStatusEffects.Remove(selectedStatusEffect);
            activeStackableEffects.Remove(selectedStatusEffect);
        }
        catch (Exception ignore)
        {
            Debug.LogError(ignore);
        }
        uiController.UpdateStatusEffectsUI(activeStatusEffects, activeStackableEffects);

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
                case "Darkness":
                    break;
                default:
                    Debug.LogError("Not supposed to be here.");
                    break;
            }
        }
    }

    public void IncreaseStatusEffectStack(StatusEffectScriptableObject statusEffect)
    {
        activeStackableEffects.TryAdd(statusEffect, 1);
        behaviours.TriggerDarkness(true, statusEffect);
    }

    public void DecreaseStatusEffectStack(StatusEffectScriptableObject statusEffect)
    {
        if (activeStackableEffects.TryGetValue(statusEffect, out var effect))
        {
            if (effect <= 0)
            {
                RemoveStatusEffect(statusEffect.statusEffectId, true);
            }
            behaviours.TriggerDarkness(false, statusEffect);
        }
    }

    public void ResetAllStatusEffects()
    {
        activeStatusEffects.Clear();
        activeStackableEffects.Clear();
        uiController.UpdateStatusEffectsUI(activeStatusEffects, activeStackableEffects);
    }
}
