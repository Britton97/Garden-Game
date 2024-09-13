using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedSelectionUI : MonoBehaviour
{
    [SerializeField] public Plant_SO plantInfo;
    [SerializeField] private Image plantImageSlot;
    [SerializeField] private TextMeshProUGUI plantCostSlot;
    public void SetPlantInfo(Plant_SO plant)
    {
        plantInfo = plant;
        plantImageSlot.sprite = plantInfo.fullyGrownSprite;
        plantCostSlot.text = plantInfo.cost.ToString();
    }
}
