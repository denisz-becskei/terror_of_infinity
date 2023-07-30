using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUIController : MonoBehaviour
{
    [SerializeField] GameObject[] statusEffectUIElements;

    public void UpdateStatusEffectsUI(List<StatusEffectScriptableObject> currentlyActiveStatusEffects)
    {
        ResetUI();
        int amountOfUIElemsToActivate = currentlyActiveStatusEffects.Count;
        for(int i = 0; i < amountOfUIElemsToActivate; i++)
        {
            statusEffectUIElements[i].SetActive(true);

            statusEffectUIElements[i].GetComponentsInChildren<Image>()[1].sprite = currentlyActiveStatusEffects[i].statusEffectIcon;
            statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[0].text = currentlyActiveStatusEffects[i].statusEffectName;
            statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[1].text = currentlyActiveStatusEffects[i].statusEffectDescription;
        }
    }

    private void ResetUI()
    {
        for(int i = 0; i < statusEffectUIElements.Length; i++)
        {
            statusEffectUIElements[i].SetActive(false);
        }
    } 
}
