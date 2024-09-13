using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementDisplay : MonoBehaviour
{
    [BoxGroup("Requirement Data")]
    public string itemName;
    [BoxGroup("Requirement Data")]
    public int currentQuantity;
    [BoxGroup("Requirement Data")]
    public int neededQuantity;
    [BoxGroup("Requirement Data")]
    public bool isMet;

    [BoxGroup("Requirement GameObjects")]
    [SerializeField] private Image bannerImage;
    [BoxGroup("Requirement GameObjects")]
    [SerializeField] private Image _requirementImage;
    [BoxGroup("Requirement GameObjects")]
    [SerializeField] private TextMeshProUGUI _requirementText;
    [BoxGroup("Requirement GameObjects")]
    [SerializeField] private GameObject _requirementCheckmark;
    [BoxGroup("Requirement GameObjects")]
    [SerializeField] private GameObject _requirementX;

    [SerializeField] private Color greenColor, darkGreenColor, redColor, darkRedColor, unknownColor, darkUnknownColor;
    [SerializeField] Sprite unknownSprite;
    [SerializeField] Image objectSpriteParent, objectPassOrFailParent, requirementTextParent;
    public void SetDisplay(AnimalChecklist checklist)
    {
        if(checklist.itemRequirement.requirementDiscovered == false)
        {
            _requirementText.text = "???";
            _requirementImage.sprite = unknownSprite;
            //_requirementImage.color = unknownColor;
            bannerImage.color = darkUnknownColor;
            objectSpriteParent.color = unknownColor;
            objectPassOrFailParent.color = unknownColor;
            requirementTextParent.color = unknownColor;
            return;
        }

        itemName = checklist.itemRequirement.GetName();
        currentQuantity = checklist.currentQuantity;
        neededQuantity = checklist.requiredQuantity;
        //isMet = checklist.isMet;
        if(currentQuantity >= neededQuantity)
        {
            _requirementCheckmark.SetActive(true);
            _requirementX.SetActive(false);
            //_requirementImage.color = greenColor;
            bannerImage.color = greenColor;
            objectSpriteParent.color = darkGreenColor;
            objectPassOrFailParent.color = darkGreenColor;
            requirementTextParent.color = darkGreenColor;
        }
        else
        {
            _requirementCheckmark.SetActive(false);
            _requirementX.SetActive(true);
            //_requirementImage.color = redColor;
            bannerImage.color = redColor;
            objectSpriteParent.color = darkRedColor;
            objectPassOrFailParent.color = darkRedColor;
            requirementTextParent.color = darkRedColor;
        }

        _requirementImage.sprite = checklist.itemRequirement.GetSprite();
        _requirementText.text = checklist.InfoRequirementText;
    }
}
