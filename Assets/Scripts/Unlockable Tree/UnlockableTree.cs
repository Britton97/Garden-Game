using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlockable Tree", menuName = "ScriptableObjects/Unlockable Tree", order = 1)]
public class UnlockableTree : SerializedScriptableObject
{
    public int currentLevel = 0;
    public int accumulatedExperience = 0;
    public int maxLevel = 10;
    public GameAction newLevelAction;

    [Button("Populate Player Levels")]
    public void PopulatePlayerLevels()
    {
        playerLevels = new List<PlayerLevel>();
        playerLevelsDictionary = new Dictionary<int, PlayerLevel>();
        for (int i = 0; i < maxLevel + 1; i++)
        {
            PlayerLevel newLevel = new PlayerLevel();
            newLevel.level = i;
            playerLevels.Add(newLevel);
            playerLevelsDictionary.Add(i, newLevel);
        }
    }
    [Button("Unlock Next Level")]
    public void UnlockNextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            UnlockLevel(currentLevel);
            newLevelAction.InvokeAction();
        }
    }
    [Button("Reset Level")]
    public void ResetLevel()
    {
        currentLevel = 0;
        accumulatedExperience = 0;

        //clear the master list
        masterAnimalSpawnRequirementsList.animalSpawnRequirements.Clear();
        //add the always unlocked animals to the master list
        foreach (GardenObject_SO unlockable in alwaysUnlocked)
        {
            if (unlockable is Animal_SO)
            {
                masterAnimalSpawnRequirementsList.animalSpawnRequirements.Add(unlockable as Animal_SO);
            }
            unlockable.ResetProgess();
        }

        //foreach garden object so in player levels reset progress
        foreach (PlayerLevel playerLevel in playerLevels)
        {
            foreach (GardenObject_SO unlockable in playerLevel.unlockables)
            {
                unlockable.ResetProgess();
            }
        }
    }
    [Button("Reset Unlock Achievements")]
    public void ResetUnlockAchievements()
    {
        //call iFirstTime.FirstTime on all unlockables
        foreach (GardenObject_SO unlockable in alwaysUnlocked)
        {
            if (unlockable is iFirstTimeTame)
            {
                (unlockable as iFirstTimeTame).FirstTimeTame = false;
            }
        }
    }
    [Button("Repopulate Dictionary")]
    public void RepopulateDictionary()
    {
        playerLevelsDictionary.Clear();
        foreach (PlayerLevel playerLevel in playerLevels)
        {
            playerLevelsDictionary.Add(playerLevel.level, playerLevel);
        }
    }
    public AnimalSpawnRequirementsList masterAnimalSpawnRequirementsList;
    public List<GardenObject_SO> alwaysUnlocked;
    public List<PlayerLevel> playerLevels;
    //make a dictionary of the player levels for easy access
    [HideInInspector] public Dictionary<int, PlayerLevel> playerLevelsDictionary = new Dictionary<int, PlayerLevel>();

    public void UnlockLevel(int level)
    {
        //use the player level dictionary to get the player level then foreach through the unlockables
        foreach (GardenObject_SO unlockable in playerLevelsDictionary[level].unlockables)
        {
            //if the unlockable is an animal_so
            if (unlockable is Animal_SO)
            {
                //add the animal to the master list
                masterAnimalSpawnRequirementsList.animalSpawnRequirements.Add(unlockable as Animal_SO);
            }
        }
    }

    public void AddExperience(int experience)
    {
        accumulatedExperience += experience;
        Debug.Log($"Adding {experience} experience. Total: {accumulatedExperience}");
        try
        {
            while (accumulatedExperience >= playerLevelsDictionary[currentLevel].experienceRequired)
            {
                Debug.Log($"Accumulated Experience: {accumulatedExperience} testing against {playerLevelsDictionary[currentLevel].experienceRequired}");
                UnlockNextLevel();

                // Check if the current level is the maximum level in the dictionary
                if (!playerLevelsDictionary.ContainsKey(currentLevel))
                {
                    break;
                }
            }
        }
        catch
        {
            Debug.LogWarning("Error adding experience. Probably not content in the next level");
        }
    }
}

[System.Serializable]
public class PlayerLevel
{
    public int level;
    public int experienceRequired;
    public List<GardenObject_SO> unlockables;
}
