using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DossierDisplay : MonoBehaviour
{
    [SerializeField] public Animal_SO animal;
    [SerializeField] private Image animalSprite;
    [SerializeField] private Sprite questionMark;

    public void SetDisplay(Animal_SO animal)
    {
        this.animal = animal;
        animalSprite.sprite = animal.gardenObjectSprite;

        if (!animal.alreadySeenOnce)
        {
            animalSprite.sprite = questionMark;
            animalSprite.color = Color.black;
        }
        else
        {
            if (animal.alreadyTamedOnce)
            {
                animalSprite.color = Color.white;
            }
            else
            {
                animalSprite.color = Color.black;
            }
        }

    }
}
