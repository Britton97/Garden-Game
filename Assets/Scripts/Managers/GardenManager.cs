using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance { get; private set; }
    public GameAction_GameState gameState;
    public float spawnRadius = 5f;

    public GardenItemDictionary gardenItemQuantities = new GardenItemDictionary();
    public AnimalSpawnRequirementsList masterAnimalSpawnRequirements;
    public AnimalSpawnRequirementsList animalSpawnRequirements;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        animalSpawnRequirements.animalSpawnRequirements.Clear();
    }
    // on game ended set game state to start
    public void OnDestroy()
    {
        gameState.SwitchGameStates(GameState.Start);
    }

    void Start()
    {
        gameState.IgnoreStateChange(false);
        gameState.enterAction.Invoke(gameState._gameState);
    }

    public void AddGardenItem(GardenObject_MonoBehavior item)
    {
        //Debug.Log($"{item.GetType().Name} called");
        string itemName = "Null";

        itemName = item.GetName();

        //Debug.Log($"Adding {itemName} to gardenItemQuantities dictionary");

        if (gardenItemQuantities.ContainsKey(itemName))
        {
            //gardenItemQuantities[item.gardenObject.gardenObjectName]++;
            gardenItemQuantities[itemName].Add(item);
        }
        else //if the item is not in the garden item quantities dictionary, add it
        {
            gardenItemQuantities.Add(itemName, new List<GardenObject_MonoBehavior> { item });
            //get a list of all the animals that require this item
            List<string> animalsThatRequireItem = masterAnimalSpawnRequirements.GetAnimalsThatRequireItem(itemName);
            //if any animals in animalsThatRequireItem are not in the animalSpawnRequirements list, add them
            foreach (string animalName in animalsThatRequireItem)
            {
                if (!animalSpawnRequirements.CheckForSpawnRequirements(animalName))
                {
                    animalSpawnRequirements.animalSpawnRequirements.Add(masterAnimalSpawnRequirements.animalSpawnRequirements.Find(x => x.GardenObjectName == animalName));
                }
            }
        }
    }

    public void RemoveGardenItem(GardenObject_MonoBehavior item)
    {
        string itemName = "Null";
        if (item.GetType() == typeof(Animal_MonoBehavior))
        {
            itemName = ((Animal_MonoBehavior)item).animalObject.GardenObjectName;
        }
        else if (item.GetType() == typeof(Plant_MonoBehavior))
        {
            itemName = ((Plant_MonoBehavior)item).GetName();
        }

        if (!gardenItemQuantities.ContainsKey(itemName)) { return; }
        gardenItemQuantities[itemName].Remove(item);
        if (gardenItemQuantities[itemName].Count > 0) { return; }

        //Debug.Log($"Removing {itemName} from gardenItemQuantities dictionary");

        gardenItemQuantities.Remove(itemName);
        //if any animals in the animalSpawnRequirements list require this item, remove them if they no other items that are required are also not in the garden
        List<string> animalsThatRequireItem = animalSpawnRequirements.GetAnimalsThatRequireItem(itemName);
        foreach (string animalName in animalsThatRequireItem)
        {
            if (!animalSpawnRequirements.CheckIfAnyRequirementMet(animalName))
            {
                animalSpawnRequirements.animalSpawnRequirements.Remove(animalSpawnRequirements.animalSpawnRequirements.Find(x => x.GardenObjectName == animalName));
            }
        }
    }

    public void CheckForSpawnRequirements()
    {
        if (GetUntamedCount() > 1) { return; }
        List<GameObject> possibleSpawns = new List<GameObject>();
        foreach (Animal_SO requirement in animalSpawnRequirements.animalSpawnRequirements)
        {
            bool requirementsMet = true;

            //if requirement is not of type Animal_SO, skip the requirement
            if (requirement.GetType() != typeof(Animal_SO)) { continue; }
            //Debug.LogWarning($"Checking for {requirement.GardenObjectName}");
            string animalName = requirement.GardenObjectName;

            foreach (ItemRequirement_Abs itemRequirement in requirement.appearRequirements)
            {
                //Debug.LogWarning($"Checking for {itemRequirement.ItemName} for the {animalName}");
                if (itemRequirement is GardenItemRequirement)
                {
                    if (!gardenItemQuantities.ContainsKey(itemRequirement.ItemName) ||
                        gardenItemQuantities[itemRequirement.ItemName].Count < itemRequirement.requiredQuantity)
                    { //if the itemRequirement is not in the garden, or the quantity of the item is less than the required quantity, set requirementsMet to false
                      //if the itemRequirement is an animal, check if the animal is tamed
                      //Debug.LogWarning($"Requirements not met for {itemRequirement.ItemName} the count is {gardenItemQuantities[itemRequirement.ItemName].Count} and the required quantity is {itemRequirement.requiredQuantity}");
                        requirementsMet = false;
                        break;
                    }
                }

                if (itemRequirement is TileRequirement)
                {
                    if (itemRequirement.GetCount() < itemRequirement.requiredQuantity)
                    {
                        //Debug.LogWarning($"Requirements not met for {itemRequirement.ItemName} the count is {TileMap_Manager.Instance.GetTileAmount(itemRequirement.ItemName)} and the required quantity is {itemRequirement.requiredQuantity}");
                        requirementsMet = false;
                        break;
                    }
                }

                //check if the item is an animal and if it is tamed
                if (gardenItemQuantities.ContainsKey(itemRequirement.ItemName))
                {
                    List<GardenObject_MonoBehavior> gardenItems = gardenItemQuantities[itemRequirement.ItemName];
                    foreach (GardenObject_MonoBehavior gardenItem in gardenItems)
                    {
                        if (gardenItem.GetType() == typeof(Animal_MonoBehavior))
                        {
                            Animal_MonoBehavior animal = (Animal_MonoBehavior)gardenItem;
                            if (animal.animalObject.GardenObjectName == itemRequirement.ItemName && animal.isTamed == false)
                            {
                                requirementsMet = false;
                                break;
                            }
                        }
                    }
                }
            }

            //this is just checking to see if there is already a untamed version of the animal in the garden
            //if there is, dont spawn another one
            int currentQuantity = 0;
            if (gardenItemQuantities.ContainsKey(requirement.GardenObjectName))
            {
                //Debug.LogWarning($"Found {requirement.GardenObjectName}");
                //currentQuantity = gardenItemQuantities[requirement.gardenObjectName];
                currentQuantity = gardenItemQuantities[requirement.GardenObjectName].Count;

                if (currentQuantity > 0)
                {
                    List<GardenObject_MonoBehavior> gardenItems = gardenItemQuantities[requirement.GardenObjectName];
                    foreach (GardenObject_MonoBehavior gardenItem in gardenItems)
                    {
                        if (gardenItem.GetType() == typeof(Animal_MonoBehavior))
                        {
                            Animal_MonoBehavior animal = (Animal_MonoBehavior)gardenItem;
                            //Debug.LogWarning($"Wanting to spawn {animalName} and there are {currentQuantity} {requirement.GardenObjectName} in the garden {animal.isTamed}");
                            if (animal.animalObject.GardenObjectName == requirement.GardenObjectName && animal.isTamed == false)
                            {
                                requirementsMet = false;
                                break;
                            }
                        }
                    }
                }
            }

            //if current quantity is greater than zero then check that animal to see if any untamed versions of it are in the garden
            //i dont want to spawn more untamed animals if there are already some in the garden

            //if check the current quantity of the item in the garden and if the spawn chance is met, spawn the animal
            if (requirementsMet && UnityEngine.Random.value < requirement.spawnChance && currentQuantity < requirement.maxQuantity)
            {
                possibleSpawns.Add(requirement.objectPrefab);
            }
        }

        if (possibleSpawns.Count > 0)
        {
            //Vector3 spawnPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0);
            Vector3 spawnPosition = TileMap_Manager.Instance.GetRandomTileGroundTile();
            int randomIndex = UnityEngine.Random.Range(0, possibleSpawns.Count);
            GameObject spawnedAnimal = Instantiate(possibleSpawns[randomIndex], spawnPosition, Quaternion.identity);

            //make sure at least one tame requirement is discovered on the spawn animal
            //if one isnt discovered, discover one
            Animal_MonoBehavior animal = spawnedAnimal.GetComponent<Animal_MonoBehavior>();
            animal.animalObject.UnlockFirstTameRequirement();
        }
    }

    public int GetUntamedCount()
    {
        int untamedCount = 0;
        foreach (KeyValuePair<string, List<GardenObject_MonoBehavior>> item in gardenItemQuantities)
        {
            foreach (GardenObject_MonoBehavior gardenItem in item.Value)
            {
                if (gardenItem.GetType() == typeof(Animal_MonoBehavior))
                {
                    Animal_MonoBehavior animal = (Animal_MonoBehavior)gardenItem;
                    if (animal.isTamed == false)
                    {
                        untamedCount++;
                    }
                }
            }
        }
        return untamedCount;
    }

    public int GetQuantityOfItem(string itemName)
    {
        //Debug.Log($"Searching for {itemName} in gardenItemQuantities dictionary");
        if (!gardenItemQuantities.ContainsKey(itemName)) { return 0; }
        return gardenItemQuantities[itemName].Count;
    }

    //function that takes a string and returns one gameobject that fits that string from the garden item quantities dictionary
    //returns a random gameobject that fits the string
    public GardenObject_MonoBehavior GetGardenItem(string itemName)
    {
        //Debug.Log($"Searching for {itemName} in gardenItemQuantities dictionary");
        if (!gardenItemQuantities.ContainsKey(itemName)) { return null; }
        return gardenItemQuantities[itemName][UnityEngine.Random.Range(0, gardenItemQuantities[itemName].Count)];
    }

    public GardenObject_MonoBehavior GetFoodRequirementFromInterests(Animal_MonoBehavior animal)
    {
        //foreach thing in animal localGardenItemRequirements
        List<GardenObject_MonoBehavior> possibleFood = new List<GardenObject_MonoBehavior>();
        foreach (AnimalChecklist item in animal.localGardenItemRequirements)
        {
            //if the item is met, continue
            if (item.isMet) { continue; }
            //if the item is not met, check if the item is in the garden item quantities dictionary
            if (gardenItemQuantities.ContainsKey(item.itemRequirement.ItemName) && gardenItemQuantities[item.itemRequirement.ItemName].Count > 0)
            {
                //if the item is in the garden item quantities dictionary, return the first item in the list
                //if the item is an animal, check if the animal is tamed
                //if it is not tamed then do not return the animal
                //if it is tamed then return the animal
                if (gardenItemQuantities[item.itemRequirement.ItemName][0].GetType() == typeof(Animal_MonoBehavior))
                {
                    Animal_MonoBehavior animalItem = (Animal_MonoBehavior)gardenItemQuantities[item.itemRequirement.ItemName][0];
                    if (animalItem.isTamed == true)
                    {
                        possibleFood.Add(animalItem);
                    }
                }
                else
                {
                    possibleFood.Add(gardenItemQuantities[item.itemRequirement.ItemName][0]);
                }
            }
        }

        if (possibleFood.Count > 0)
        {
            return possibleFood[UnityEngine.Random.Range(0, possibleFood.Count)];
        }
        else
        {
            return null;
        }
    }
    //function that returns a random mature plant from the garden item quantities dictionary
    public GardenObject_MonoBehavior GetRandomMaturePlant()
    {
        GardenObject_MonoBehavior food = null;
        foreach (KeyValuePair<string, List<GardenObject_MonoBehavior>> item in gardenItemQuantities)
        {
            if (item.Value.Count > 0 && item.Value[0].GetType() == typeof(Plant_MonoBehavior))
            {
                Plant_MonoBehavior plant = (Plant_MonoBehavior)item.Value[0];
                if (plant.plantState == PlantState.Mature)
                {
                    return plant;
                }
            }
        }
        return food;
    }
}

[Serializable]
public class GardenItemDictionary : UnitySerializedDictionary<string, List<GardenObject_MonoBehavior>> { }