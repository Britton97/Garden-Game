using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu_UI : MonoBehaviour
{
    public UI_Tweener UITweener;
    public TextMeshProUGUI nameText;
    public Image objectSprite;
    public GameObject priceParent;
    public TextMeshProUGUI priceText;
    [ReadOnly] public bool isSelected = false;
    [SerializeField] [ReadOnly] private GameObject currentUISelection; 

    [SerializeField] private GameAction_Bool YUIButtonState;
    [SerializeField] private DataGameObject_SO hoveringOver;
    [SerializeField] private GameObject requirementParent;
    [SerializeField] private RequirementDisplay requirementDisplayPrefab;
    [SerializeField] private List<RequirementDisplay> activeRequirementDisplays = new List<RequirementDisplay>();

    public void NewUISelection(iSelectable item, GameObject gameObject)
    {
        //uitweener is currently tweening then return
        if(UITweener.tweener.CurrentlyTweening) return;
        if(currentUISelection == gameObject) return;
        //Debug.Log("New UI Selection");
        currentUISelection = gameObject;
        UpdateUI(item);
        UITweener.tweener.TweenA();
        //priceParent.SetActive(false);
        CheckIfSellable(gameObject);
        CheckForIRequirements(gameObject);
        isSelected = true;
    }

    private void UpdateUI(iSelectable item)
    {
        nameText.text = item.GetName();
        objectSprite.sprite = item.GetSprite();
        objectSprite.SetNativeSize();
    }

    public void CheckIfSellable(GameObject obj)
    {
        if(obj.TryGetComponent(out iSellable sellable))
        {
            if(sellable.IsSellable() == false)
            {
                priceParent.SetActive(false);
                return;
            }
            priceParent.SetActive(true);
            priceText.text = sellable.GetSellPrice().ToString();
        }
        else
        {
            priceParent.SetActive(false);
        }
    }

    public void CheckForIRequirements(GameObject item)
    {
        //Debug.Log($"Checking for requirements on {item.name}");
        if(item.TryGetComponent(out iRequirements requirements))
        {
            if(requirements.ReportRequirements() == false) return;
            //requirements.ReportRequirements();
            List<AnimalChecklist> requirementsList = requirements.GetRequirements();

            if(activeRequirementDisplays.Count > 0)
            {
                foreach(RequirementDisplay display in activeRequirementDisplays)
                {
                    Destroy(display.gameObject);
                }
                activeRequirementDisplays.Clear();
            }
            
            foreach(AnimalChecklist requirement in requirementsList)
            {
                GameObject newDisplay = Instantiate(requirementDisplayPrefab.gameObject, requirementParent.transform);
                RequirementDisplay display = newDisplay.GetComponent<RequirementDisplay>();
                display.SetDisplay(requirement);
                activeRequirementDisplays.Add(display);     
            }
        }
    }

    public void Deselect()
    {
        //if(UITweener.tweener.CurrentlyTweening) return;
        isSelected = false;
        //Debug.Log("Selection UI should be deselected");
        currentUISelection = null;
        UITweener.tweener.TweenB();
        //clear and destroy all active requirement displays
        if(activeRequirementDisplays.Count > 0)
        {
            foreach(RequirementDisplay display in activeRequirementDisplays)
            {
                Destroy(display.gameObject);
            }
            activeRequirementDisplays.Clear();
        }
    }

    public void CloseUIWindow()
    {
        if(isSelected && hoveringOver.data == null)
        {
            Deselect();
            YUIButtonState.InvokeAction(false);
        }
    }

    public bool CurrentlyTweening()
    {
        return UITweener.tweener.CurrentlyTweening;
    }
}
