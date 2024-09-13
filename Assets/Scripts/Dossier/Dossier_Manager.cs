using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public enum DisplayState
{
    Appear,
    Tame
}
public class Dossier_Manager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private DisplayState displayState = DisplayState.Appear;
    [SerializeField] private AnimalSpawnRequirementsList animalSpawnRequirementsList;
    [SerializeField] private RectTransform dossierDisplayParent;
    [SerializeField] private RectTransform dossierDisplayPrefab;
    [SerializeField] private List<RectTransform> dossierDisplays = new List<RectTransform>();
    [SerializeField] private int currentDossierIndex = 0;
    [SerializeField] private RectTransform selectorTransform;
    [SerializeField] private float selectorMoveSpeed = 1;
    [SerializeField] private int columnCount = 2;
    [SerializeField] private Image displayAnimalSprite;
    [SerializeField] private TextMeshProUGUI displayAnimalName;
    [SerializeField] private Color greenColor, darkerGreenColor, redColor, darkerRedColor, unknownColor, darkerUnknownColor;
    [SerializeField] RectTransform requirementDisplay;
    [SerializeField] Image appearButtonImage, tameButtonImage;
    [SerializeField] Color activeButtonColor, idleButtonColor;

    private void Start()
    {
        // Initialize the selector position
        PopulateDossier();
        MoveSelectorToCurrentIndex();
        //if the dossier displays are not empty then update the display animal sprite and name
        if (dossierDisplays.Count > 0)
        {
            UpdateDisplayAnimal();
            MoveSelectorToCurrentIndex();
        }
    }

    private void Update()
    {
        //HandleInput();
    }

    public void PopulateDossier()
    {
        //when called clear the dossier displays and destroy them
        //then foreach through animalSpawnRequirementsList and create a dossier display for each animal
        ClearDossier();
        foreach (Animal_SO animal in animalSpawnRequirementsList.animalSpawnRequirements)
        {
            RectTransform dossierDisplay = Instantiate(dossierDisplayPrefab, dossierDisplayParent);
            dossierDisplay.GetComponent<DossierDisplay>().SetDisplay(animal);
            dossierDisplays.Add(dossierDisplay);
        }
    }

    public void ClearDossier()
    {
        foreach (RectTransform dossierDisplay in dossierDisplays)
        {
            Destroy(dossierDisplay.gameObject);
        }
        dossierDisplays.Clear();
    }
    public void HandleInput()
    {
        Vector2 inputVector = playerInput.actions["Movement"].ReadValue<Vector2>();
        //read the input vector and send it to move selector
        if (inputVector != Vector2.zero)
        {
            if (inputVector.x > 0f)
            {
                MoveSelector(1);
            }
            else if (inputVector.x < 0f)
            {
                MoveSelector(-1);
            }
            else if (inputVector.y > 0f)
            {
                MoveSelector(columnCount);
            }
            else if (inputVector.y < 0f)
            {
                MoveSelector(-columnCount);
            }
        }

    }

    private void MoveSelector(int direction)
    {
        if (moveSelectorCoroutine != null)
        {
            return;
        }

        int newIndex = currentDossierIndex + direction;

        // Handle wrap-around for horizontal movement
        if (direction == 1 && (currentDossierIndex + 1) % columnCount == 0)
        {
            newIndex = currentDossierIndex - (columnCount - 1);
        }
        else if (direction == -1 && currentDossierIndex % columnCount == 0)
        {
            newIndex = currentDossierIndex + (columnCount - 1);
        }
        // Handle wrap-around for vertical movement
        else if (direction == columnCount && newIndex >= dossierDisplays.Count)
        {
            newIndex = currentDossierIndex % columnCount;
        }
        else if (direction == -columnCount && newIndex < 0)
        {
            newIndex = dossierDisplays.Count - (columnCount - (currentDossierIndex % columnCount));
            if (newIndex >= dossierDisplays.Count)
            {
                newIndex -= columnCount;
            }
        }

        // Ensure the new index is within bounds
        if (newIndex >= 0 && newIndex < dossierDisplays.Count)
        {
            currentDossierIndex = newIndex;
            MoveSelectorToCurrentIndex();
            //function to update the display animal sprite and name
            UpdateDisplayAnimal();
        }
    }

    private void UpdateDisplayAnimal()
    {
        if (dossierDisplays.Count == 0)
        {
            return;
        }
        DossierDisplay currentDossierDisplay = dossierDisplays[currentDossierIndex].GetComponent<DossierDisplay>();
        if(currentDossierDisplay.animal.alreadyTamedOnce)
        {
            displayAnimalSprite.color = Color.white;
        }
        else
        {
            displayAnimalSprite.color = Color.black;
        }
        displayAnimalSprite.sprite = currentDossierDisplay.animal.gardenObjectSprite;
        displayAnimalName.text = currentDossierDisplay.animal.GardenObjectName;
        DisplayRequirements(currentDossierDisplay);
    }

    [SerializeField] private RectTransform requirementDisplayParent;
    [SerializeField] private List<DossierRequirementDisplay> activeRequirementDisplays = new List<DossierRequirementDisplay>();
    public void DisplayRequirements(DossierDisplay dossierDisplay)
    {
        List<AnimalChecklist> requirementsList = new List<AnimalChecklist>();
        //Debug.Log($"Count: {animalObject.itemRequirements.Count}");
        switch (displayState)
        {
            case DisplayState.Appear:
                foreach (ItemRequirement_Abs requirement in dossierDisplay.animal.appearRequirements)
                {
                    requirementsList.Add(new AnimalChecklist(requirement));
                }
                break;
            case DisplayState.Tame:
                foreach (ItemRequirement_Abs requirement in dossierDisplay.animal.tameRequirements)
                {
                    requirementsList.Add(new AnimalChecklist(requirement));
                }
                break;
        }

        //get the count of requirements and check to see if you need to spawn more requirement displays
        int requirementCount = requirementsList.Count;
        if (activeRequirementDisplays.Count < requirementCount)
        {
            int difference = requirementCount - activeRequirementDisplays.Count;
            for (int i = 0; i < difference; i++)
            {
                RectTransform newRequirementDisplay = Instantiate(requirementDisplay, requirementDisplayParent);
                activeRequirementDisplays.Add(newRequirementDisplay.GetComponent<DossierRequirementDisplay>());
            }
        }
        //if the count of requirements is less than the active requirement displays then set them in to inactive
        else if (activeRequirementDisplays.Count > requirementCount)
        {
            for (int i = requirementCount; i < activeRequirementDisplays.Count; i++)
            {
                activeRequirementDisplays[i].gameObject.SetActive(false);
            }
        }

        //now send the information to the active requirement displays
        for (int i = 0; i < requirementCount; i++)
        {
            activeRequirementDisplays[i].gameObject.SetActive(true);
            //need to get a boolean for each requirement to see if it is met
            //if it is met then send darker green color and green color
            //if it is not met then send darker red color and red color
            if (requirementsList[i].itemRequirement.requirementDiscovered == false)
            {
                activeRequirementDisplays[i].SetDisplay(requirementsList[i], darkerUnknownColor, unknownColor, displayState);
            }
            else if (requirementsList[i].itemRequirement.GetCount() >= requirementsList[i].requiredQuantity)
            {
                activeRequirementDisplays[i].SetDisplay(requirementsList[i], darkerGreenColor, greenColor, displayState);
            }
            else
            {
                activeRequirementDisplays[i].SetDisplay(requirementsList[i], darkerRedColor, redColor, displayState);
            }
        }
    }

    private Coroutine moveSelectorCoroutine;
    private void MoveSelectorToCurrentIndex()
    {
        if (currentDossierIndex >= 0 && currentDossierIndex < dossierDisplays.Count)
        {
            moveSelectorCoroutine = StartCoroutine(MoveSelectorCoroutine(dossierDisplays[currentDossierIndex]));
        }
    }

    private IEnumerator MoveSelectorCoroutine(RectTransform targetTransform)
    {
        while (Vector3.Distance(selectorTransform.position, targetTransform.position) > 0.01f)
        {
            selectorTransform.position = Vector3.MoveTowards(selectorTransform.position, targetTransform.position, selectorMoveSpeed * Time.deltaTime);
            yield return null;
        }
        selectorTransform.position = targetTransform.position;
        moveSelectorCoroutine = null;
    }

    public void SwitchToAppear()
    {
        displayState = DisplayState.Appear;
        appearButtonImage.color = activeButtonColor;
        tameButtonImage.color = idleButtonColor;
        UpdateDisplayAnimal();
    }

    public void SwitchToTame()
    {
        displayState = DisplayState.Tame;
        appearButtonImage.color = idleButtonColor;
        tameButtonImage.color = activeButtonColor;
        UpdateDisplayAnimal();
    }

}