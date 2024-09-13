using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SeedSelectionMenu : MonoBehaviour
{
    public static SeedSelectionMenu Instance { get; private set; }

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
    }
    public GameObject currentPlant;
    [ReadOnly][SerializeField] private float scrollValue = 0;
    public GameObject UIPrefab;
    public ScrollRect scrollRect;
    public GameObject spawnUnder;
    public UnlockableTree unlockableTree;
    public List<Plant_SO> plantList;
    public List<SeedSelectionUI> seedSelectionUIs;
    //curent plant seed select 

    void Start()
    {
        PopulateUIInfo();
    }

    /*
    public void PopulateUIInfo()
    {
        //instantiate a new UI prefab for each plant in the plant list
        foreach (Plant_SO plant in plantList)
        {
            GameObject newUI = Instantiate(UIPrefab, spawnUnder.transform);
            newUI.gameObject.name = plant.GardenObjectName;
            newUI.GetComponent<SeedSelectionUI>().SetPlantInfo(plant);
            seedSelectionUIs.Add(newUI.GetComponent<SeedSelectionUI>());
        }
        scrollValue = 0;
        currentPlant = seedSelectionUIs[0].gameObject;
    }
    */

    public void PopulateUIInfo()
    {
        //add the always unlocked plants to the UI
        foreach (GardenObject_SO unlockable in unlockableTree.alwaysUnlocked)
        {
            if (unlockable is Plant_SO)
            {
                GameObject newUI = Instantiate(UIPrefab, spawnUnder.transform);
                newUI.gameObject.name = unlockable.GardenObjectName;
                newUI.GetComponent<SeedSelectionUI>().SetPlantInfo(unlockable as Plant_SO);
                seedSelectionUIs.Add(newUI.GetComponent<SeedSelectionUI>());
                plantList.Add(unlockable as Plant_SO);
            }
        }

        //foreach through the unlockable tree's player levels
        foreach (PlayerLevel playerLevel in unlockableTree.playerLevels)
        {
            //if the player level is greater than the current level then break
            if (playerLevel.level > unlockableTree.currentLevel)
            {
                break;
            }
            //foreach through the unlockables in the player level
            foreach (GardenObject_SO unlockable in playerLevel.unlockables)
            {
                //if the unlockable is a plant_so
                if (unlockable is Plant_SO)
                {
                    //instantiate a new UI prefab for each plant in the plant list
                    GameObject newUI = Instantiate(UIPrefab, spawnUnder.transform);
                    newUI.gameObject.name = unlockable.GardenObjectName;
                    newUI.GetComponent<SeedSelectionUI>().SetPlantInfo(unlockable as Plant_SO);
                    seedSelectionUIs.Add(newUI.GetComponent<SeedSelectionUI>());
                    plantList.Add(unlockable as Plant_SO);
                }
            }
        }

        ScrollToSpecificPlant(0);
    }

    public void PopulateUIInfoForNewLevel()
    {
        int newLevel = unlockableTree.currentLevel;
        Debug.Log("New Level: " + newLevel);
        //use the player level dictionary to get the player level then foreach through the unlockables
        foreach (GardenObject_SO unlockable in unlockableTree.playerLevelsDictionary[newLevel].unlockables)
        {
            Debug.Log("Unlockable: " + unlockable.GardenObjectName);
            //if the unlockable is a plant_so
            if (unlockable is Plant_SO)
            {
                //instantiate a new UI prefab for each plant in the plant list
                GameObject newUI = Instantiate(UIPrefab, spawnUnder.transform);
                newUI.gameObject.name = unlockable.GardenObjectName;
                newUI.GetComponent<SeedSelectionUI>().SetPlantInfo(unlockable as Plant_SO);
                seedSelectionUIs.Add(newUI.GetComponent<SeedSelectionUI>());
                plantList.Add(unlockable as Plant_SO);
            }
        }

        ScrollToSpecificPlant(0);
    }

    public void ScrollToSpecificPlant(int slotValue)
    {
        scrollValue = slotValue;
        scrollRect.horizontalNormalizedPosition = scrollValue / (plantList.Count - 1);
        currentPlant = seedSelectionUIs[(int)scrollValue].gameObject;
    }

    public void ScrollOneLeft()
    {
        if (scrollValue > 0)
        {
            scrollValue -= 1;
            scrollRect.horizontalNormalizedPosition = scrollValue / (plantList.Count - 1);
            currentPlant = seedSelectionUIs[(int)scrollValue].gameObject;
        }
    }

    public void ScrollOneRight()
    {
        if (scrollValue < plantList.Count - 1)
        {
            scrollValue += 1;
            scrollRect.horizontalNormalizedPosition = scrollValue / (plantList.Count - 1);
            currentPlant = seedSelectionUIs[(int)scrollValue].gameObject;
        }
    }

    public GameObject GetCurrentPlant()
    {
        return currentPlant.GetComponent<SeedSelectionUI>().plantInfo.objectPrefab;
    }

    public int GetCurrentPlantCost()
    {
        return (int)currentPlant.GetComponent<SeedSelectionUI>().plantInfo.cost;
    }
}
