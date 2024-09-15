using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[InlineProperty]
[CreateAssetMenu(fileName = "Animal Info", menuName = "ScriptableObjects/Animal Info", order = 1)]
public class Animal_SO : GardenObject_SO, iFirstTimeTame, iFirstTimeSeen
{
    //[VerticalGroup("Animal/Info")]
    //[LabelWidth(100)]
    [Range(0, 1)]
    public float spawnChance;
    public int sellPrice;
    public Color untamedColor = new Color(0, 0, 0, 1);
    public Color tamedColor = new Color(1, 1, 1, 1);
    public List<ColorChange> colorChanges;

    [SerializeReference] public List<ItemRequirement_Abs> appearRequirements;
    [SerializeReference] public List<ItemRequirement_Abs> tameRequirements;
    //public List<TileRequirement> tileRequirements;

    public UnlockableTree unlockableTree;
    #region First Time Tame Variables
    public bool alreadyTamedOnce = false;
    public bool FirstTimeTame { get => alreadyTamedOnce; set => alreadyTamedOnce = value; }
    public float firstTimeTamingExperience = 10;
    public float FirstTimeTameExperience { get => firstTimeTamingExperience; }
    #endregion
    #region First Time Seen Variables
    public bool alreadySeenOnce = false;
    public bool FirstTimeSeen { get => alreadySeenOnce; set => alreadySeenOnce = value; }
    public float firstTimeSeeingExperience = 10;
    public float FirstTimeSeenExperience { get => firstTimeSeeingExperience; }
    [SerializeField] private AnimalSpawnRequirementsList seenAnimalList;
    #endregion

    public override void ResetProgess()
    {
        alreadySeenOnce = false;
        alreadyTamedOnce = false;
    }
    public void AddToSeenAnimalList(Animal_SO animal)
    {
        //add animal to seen animal list if it is not already in the list
        if (!seenAnimalList.CheckForSpawnRequirements(animal.GardenObjectName))
        {
            seenAnimalList.animalSpawnRequirements.Add(animal);
        }
    }
    public void ChangeColor(string plantName, Animal_MonoBehavior animal)
    {
        if (animal.isTamed == false) { return; }
        foreach (ColorChange colorChange in colorChanges)
        {
            if (colorChange.plant.GardenObjectName == plantName)
            {
                animal._spriteRenderer.color = colorChange.color;
            }
        }
    }

    public void EatFood(Animal_MonoBehavior context, GameObject food)
    {
        if (food.TryGetComponent<iEdible>(out iEdible edible))
        {
            if (edible.IsEdible() && food.TryGetComponent<GardenObject_MonoBehavior>(out GardenObject_MonoBehavior gardenObject))
            {
                context.hunger = context.maxHunger;
                SelectionManager.Instance.IsSelectionManagerUsingObject(food);
                if (context.isTamed == false)
                {
                    context.CheckTamingRequirements(gardenObject.GetName()); //this appears to be the issue
                }
                else
                {
                    ChangeColor(gardenObject.GetName(), context);
                }
                context.animator.SetTrigger("Attack");
                Destroy(food); //destroying food is not the issue causing the bug
                context.interest = null;
                context._GifPlayer.PlayGif("Happy", 2f);

                //check to see if the food the animal just ate is a requirement for taming or appearing 
                //if it is then check to see if the requirement is discovered
                foreach (ItemRequirement_Abs requirement in appearRequirements)
                {
                    if (requirement.ItemName == gardenObject.GetName())
                    {
                        requirement.requirementDiscovered = true;
                        break;
                    }
                }

                foreach (ItemRequirement_Abs requirement in tameRequirements)
                {
                    if (requirement.ItemName == gardenObject.GetName())
                    {
                        requirement.requirementDiscovered = true;
                        break;
                    }
                }
            }
        }
    }

    public void UnlockFirstTameRequirement()
    {
        //unlock the first tame requirement
        tameRequirements[0].requirementDiscovered = true;
    }

    public void UnlockNextTameRequirement()
    {
        //if no tame requirements are discovered then unlock the first tame requirement
        foreach (ItemRequirement_Abs requirement in tameRequirements)
        {
            if (requirement.requirementDiscovered == false)
            {
                requirement.requirementDiscovered = true;
                break;
            }
        }
    }

    public void UnlockAllTameAndAppearRequirements()
    {
        foreach (ItemRequirement_Abs requirement in tameRequirements)
        {
            requirement.requirementDiscovered = true;
        }

        foreach (ItemRequirement_Abs requirement in appearRequirements)
        {
            requirement.requirementDiscovered = true;
        }
    }

    public void CheckForNextTamingUnlock(Animal_MonoBehavior context)
    {
        //check the localrequirements
        //check each requirement to see if it is met
        //if all of the discovered requirements are met then unlock the next requirement
        //using the context to check the requirements of the local requirements
        int discoveredAndMetRequirements = 0;
        int discoveredRequirements = 0;
        int totalRequirements = context.localGardenItemRequirements.Count;
        foreach (AnimalChecklist checklist in context.localGardenItemRequirements)
        {
            if (checklist.isMet)
            {
                discoveredAndMetRequirements++;
                if (checklist.itemRequirement.requirementDiscovered)
                {
                    discoveredRequirements++;
                }
            }
        }

        if (discoveredAndMetRequirements == discoveredRequirements && discoveredAndMetRequirements < totalRequirements)
        {
            UnlockNextTameRequirement();
        }
    }

    public string GetRandomRequirement()
    {
        if (tameRequirements.Count == 0) { return ""; }
        return tameRequirements[UnityEngine.Random.Range(0, tameRequirements.Count)].ItemName;
    }
}

[Serializable]
public class ColorChange
{
    public Plant_SO plant;
    public Color color;
}

[Serializable]
public class AnimalChecklist // animal requirements use ItemRequirement_Abs
{
    [SerializeReference] public ItemRequirement_Abs itemRequirement;
    public bool isMet;
    public int currentQuantity;
    public int requiredQuantity;
    public string DossierTameRequirementText
    {
        get
        {
            switch (itemRequirement)
            {
                case GardenItemRequirement:
                    string returnText = "Null";
                    //return $"Garden has {requiredQuantity} {itemRequirement.ItemName}";
                    if (itemRequirement is GardenItemRequirement gardenItemRequirement)
                    {
                        if (gardenItemRequirement.requirementType == RequirementType.EatItem)
                        {
                            returnText = $"Eat {requiredQuantity} {itemRequirement.ItemName}";
                        }
                        else
                        {
                            returnText = $"Garden has {requiredQuantity} {itemRequirement.ItemName}";
                        }
                    }
                    return returnText;
                case TileRequirement:
                    return $"Garden has {requiredQuantity} {itemRequirement.ItemName} tiles";
                default:
                    return $"Default: Something went wrong";
            }
        }
    }

    public string DossierAppearRequirementText
    {
        get
        {
            switch (itemRequirement)
            {
                case GardenItemRequirement:
                    string returnText = "Null";

                    if (itemRequirement is GardenItemRequirement gardenItemRequirement)
                    {
                        if (gardenItemRequirement.requirementType == RequirementType.EatItem)
                        {
                            returnText = $"Eat {requiredQuantity} {itemRequirement.ItemName}";
                        }
                        else
                        {
                            returnText = $"Garden has {requiredQuantity} {itemRequirement.ItemName}";
                        }
                    }
                    return returnText;

                case TileRequirement:
                    return $"Garden has {requiredQuantity} {itemRequirement.ItemName} tiles";

                default:
                    return $"Default: Something went wrong";
            }
        }
    }



    public string InfoRequirementText
    {
        get
        {
            switch (itemRequirement)
            {
                case GardenItemRequirement:
                    string returnText = "Null";

                    if (itemRequirement is GardenItemRequirement gardenItemRequirement)
                    {
                        if (gardenItemRequirement.requirementType == RequirementType.EatItem)
                        {
                            returnText = $"Eat {currentQuantity}/{requiredQuantity} {itemRequirement.ItemName}";
                        }
                        else
                        {
                            returnText = $"Garden has {requiredQuantity} {itemRequirement.ItemName}";
                        }
                    }
                    return returnText;

                case TileRequirement:
                    return $"Garden has {requiredQuantity} {itemRequirement.ItemName} tiles";

                default:
                    return $"Default: Something went wrong";
            }
        }
    }

    public AnimalChecklist(ItemRequirement_Abs requirement)
    {
        itemRequirement = requirement;
        isMet = false;
        currentQuantity = 0;
        requiredQuantity = requirement.requiredQuantity;
    }
    public void CheckRequirement(string foodName, Animal_MonoBehavior context)
    {
        if (foodName == itemRequirement.ItemName)
        {
            bool beforeMet = isMet;
            currentQuantity++;
            isMet = currentQuantity >= itemRequirement.requiredQuantity;

            if (beforeMet != isMet)
            {
                context.animalObject.CheckForNextTamingUnlock(context);
            }
        }
    }
}