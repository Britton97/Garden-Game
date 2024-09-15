using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DossierRequirementDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _requirementText;
    [SerializeField] Image _requirementImage;
    [SerializeField] Image _requirementImageParent;
    [SerializeField] Image bannerImage;
    [SerializeField] string unknownText;
    [SerializeField] Sprite unknownSprite;
    [SerializeField] Color defaultGreenColor, defaultDarkerGreenColor;


    public void SetDisplay(AnimalChecklist checklist, Color darkerColor, Color lighterColor, DisplayState displayState)
    {
        if (checklist.itemRequirement.requirementDiscovered == false)
        {
            _requirementText.text = unknownText;
            _requirementImage.sprite = unknownSprite;
            _requirementImageParent.color = darkerColor;
            bannerImage.color = lighterColor;
            return;
        }

        switch (displayState)
        {
            case DisplayState.Appear:
                _requirementText.text = checklist.DossierAppearRequirementText;
                _requirementImageParent.color = darkerColor;
                bannerImage.color = lighterColor;
                break;
            case DisplayState.Tame:
                _requirementText.text = checklist.DossierTameRequirementText;
                _requirementImageParent.color = defaultDarkerGreenColor;
                bannerImage.color = defaultGreenColor;
                break;
        }
        //_requirementText.text = checklist.DossierTameRequirementText;
        _requirementImage.sprite = checklist.itemRequirement.GetSprite();
    }

    public void SetDisplayNotSeen(Color darkerColor, Color lighterColor)
    {
        _requirementText.text = unknownText;
        _requirementImage.sprite = unknownSprite;
        _requirementImageParent.color = darkerColor;
        bannerImage.color = lighterColor;
    }
}
