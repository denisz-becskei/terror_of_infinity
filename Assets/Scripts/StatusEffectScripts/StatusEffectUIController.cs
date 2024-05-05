using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUIController : MonoBehaviour
{
    [SerializeField] GameObject[] statusEffectUIElements;

    public void UpdateStatusEffectsUI(List<StatusEffectScriptableObject> currentlyActiveStatusEffects, 
        Dictionary<StatusEffectScriptableObject, int> currentlyActiveStackableEffects)
    {
        ResetUI();
        int amountOfUIElemsToActivate = currentlyActiveStatusEffects.Count + currentlyActiveStackableEffects.Count;
        for(int i = 0; i < amountOfUIElemsToActivate; i++)
        {
            if (i < currentlyActiveStatusEffects.Count)
            {
                statusEffectUIElements[i].SetActive(true);

                statusEffectUIElements[i].GetComponentsInChildren<Image>()[1].sprite = currentlyActiveStatusEffects[i].statusEffectIcon;
                statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[0].text = currentlyActiveStatusEffects[i].statusEffectName;
                statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[1].text = currentlyActiveStatusEffects[i].statusEffectDescription;
            }
            else
            {
                statusEffectUIElements[i].SetActive(true);

                statusEffectUIElements[i].GetComponentsInChildren<Image>()[1].sprite = currentlyActiveStackableEffects
                    .ElementAt(i - currentlyActiveStatusEffects.Count).Key.statusEffectIcon;
                statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[0].text = currentlyActiveStackableEffects
                    .ElementAt(i - currentlyActiveStatusEffects.Count).Key.statusEffectName;
                statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[1].text = currentlyActiveStackableEffects
                    .ElementAt(i - currentlyActiveStatusEffects.Count).Key.statusEffectDescription;
                statusEffectUIElements[i].GetComponentsInChildren<TMP_Text>()[2].text = currentlyActiveStackableEffects
                    .ElementAt(i - currentlyActiveStatusEffects.Count).Value.ToString();
            }
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
