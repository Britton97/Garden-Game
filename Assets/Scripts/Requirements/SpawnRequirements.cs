using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;



[Serializable]
[CreateAssetMenu(fileName = "AnimalSpawnRequirementsList", menuName = "ScriptableObjects/AnimalSpawnRequirementsList", order = 1)]
public class AnimalSpawnRequirementsList : SerializedScriptableObject
{
    public List<Animal_SO> animalSpawnRequirements;

    //function that searches the list of animal spawn requirements for a specific animal and returns a boolean value
    public bool CheckForSpawnRequirements(string animalName)
    {
        foreach (GardenObject_SO animalSpawnRequirement in animalSpawnRequirements)
        {            
            if (animalSpawnRequirement.GardenObjectName == animalName)
            {
                return true;
            }
        }
        return false;
    }

    //function that checks if any requirement for an animal is met regardless of the quantity
    public bool CheckIfAnyRequirementMet(string animalName)
    {
        foreach (Animal_SO animalSpawnRequirement in animalSpawnRequirements)
        {
            if (animalSpawnRequirement.GardenObjectName == animalName)
            {
                foreach (ItemRequirement_Abs itemRequirement in animalSpawnRequirement.tameRequirements)
                {
                    if (GardenManager.Instance.gardenItemQuantities.ContainsKey(itemRequirement.GetName()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //function that takes in an item name and returns a list of all the animals that require that item
    public List<string> GetAnimalsThatRequireItem(string itemName)
    {
        //Debug.Log($"GetAnimalsThatRequireItem: {itemName}");
        //Debug.Log($"animalSpawnRequirements.Count: {animalSpawnRequirements.Count}");
        List<string> animalsThatRequireItem = new List<string>();
        foreach (Animal_SO animalSpawnRequirement in animalSpawnRequirements)
        {
            foreach (ItemRequirement_Abs itemRequirement in animalSpawnRequirement.tameRequirements)
            {
                if (itemRequirement.GetName() == itemName)
                {
                    animalsThatRequireItem.Add(animalSpawnRequirement.GardenObjectName);
                }
            }
        }
        return animalsThatRequireItem;
    }
}
