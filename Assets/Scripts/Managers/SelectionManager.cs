using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    public Tween_Vector3 tweenVector3;
    public GameObject currentSelection;
    [ReadOnly]
    public GameObject hoveringOver;
    [ReadOnly] public GameObject secondaryHoveringOver;
    [SerializeField] private DataGameObject_SO hoveringOverSO;
    [SerializeField]
    [ReadOnly]
    public InteractionManager currentInteractable;
    public LayerMask raycastLayer, groundLayer;
    public GameAction_GameState gameState;
    [FoldoutGroup("Money")]
    public DataInt_SO moneySO;
    [FoldoutGroup("Money")]
    public GameAction moneySpentAction;
    public SelectionMenu_UI selectionMenuUI;
    [SerializeField] private SpriteRenderer selfSprite;
    private Material spriteMaterial;
    [ColorUsage(true, true)] public Color defaultColor, highlightColor, selectionColor;

    [FoldoutGroup("Selection Events")]
    public UnityEvent onSelectionEvent;
    [FoldoutGroup("Selection Events")]
    public UnityEvent onDeselectionEvent;
    public BaseState_Animal walkToPointState;
    //public TileMap_Manager tileMap;

    private void Awake()
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
        tweenVector3.SetInitialValues(gameObject);
        //get the material on the object sprite
        spriteMaterial = selfSprite.material;
        currentInteractable = new InteractionManager();
        //subscribe to the left click event
    }

    public void RecieveTweenValue(Vector3 value)
    {
        transform.position = value;
    }

    void FixedUpdate()
    {
        if (transform.parent == null)
        {
            FollowMouse();
        }
        if (gameState._gameState == GameState.Play)
        {
            if (currentSelection == null)
            {
                RaycastForInteractables();
            }
            else if (currentSelection != null)
            {
                RaycastForInteractablesWithCurrentSelection();
            }
        }
    }

    void LateUpdate()
    {
        if (transform.parent == null)
        {
            transform.position = lastMousePos;
        }
    }

    #region Mouse Functions
    Vector3 lastMousePos;
    public void FollowMouse()
    {
        //move the transform to the mouse position
        //if raycast at pos is over a garden object then change the color of the selfimage to red if not then change it to white
        Vector3 mousePos = ReturnMousePosition();
        RaycastHit2D hit = RaycastForCollider(mousePos, raycastLayer);
        //Debug.Log($"I hit {hit.collider}");
        if (hit.collider != null && hit.collider.gameObject.GetComponent<GardenObject_MonoBehavior>() != null)
        {
            spriteMaterial.SetColor("_Color", highlightColor);
        }
        else
        {
            spriteMaterial.SetColor("_Color", defaultColor);
        }
        lastMousePos = mousePos;
    }
    public Vector3 ReturnMousePosition()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;
        return worldPosition;
    }
    public RaycastHit2D RaycastForCollider(Vector3 _position, LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(_position, Vector2.zero, Mathf.Infinity, layer);
        if (hit.collider != null)
        {
            return hit;
        }
        return hit;
    }
    #endregion
    public void LeftClick()
    {
        switch (gameState._gameState)
        {
            case GameState.Play:
                Perform_LeftClick();
                break;
            case GameState.Pause:
                LeftClick_PlantObject();
                break;
            case GameState.Build:
                //cast last mouse position to a vector3int
                break;
            case GameState.End:
                break;
            case GameState.Start:
                break;
        }
    }

    #region Button Functions
    public void Perform_LeftClick()
    {
        if (currentInteractable == null) return; //if current interactable is null then return
        if (currentSelection == null) //if current selection is null
        {
            //currentSelection = hoveringOver;
            currentInteractable.AButtonSelect(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
        }
        else
        {
            currentInteractable.AButtonPerformAction(this, currentSelection.GetComponent<GardenObject_MonoBehavior>());
            //currentSelection = null;
        }
    }

    public void Perform_NextButton()
    {
        if (currentInteractable == null) return;
        if (currentSelection == null && hoveringOver != null)
        {
            currentInteractable.BButtonSelect(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
        }
        else if (currentSelection != null)
        {
            currentInteractable.BButtonPerformAction(this, currentSelection.GetComponent<GardenObject_MonoBehavior>());
        }
    }

    public void Perform_PreviousButton()
    {
        if (currentInteractable == null) return;
        if (currentSelection == null && hoveringOver != null)
        {
            currentInteractable.YButtonSelect(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
        }
        else if (currentSelection != null)
        {
            currentInteractable.YButtonPerformAction(this, currentSelection.GetComponent<GardenObject_MonoBehavior>());
        }
    }

    #endregion
    #region Raycast Functions
    private iInteractable secondaryInteractable;
    public void RaycastForInteractablesWithCurrentSelection()
    {
        //this is different than the normal raycast for interactables because it will only raycast for interactables if there is a current selection
        //if there the other interactable is not null then call HoverWhileSelected on the current interactable
        Vector3 mousePos = ReturnMousePosition();
        RaycastHit2D hit = RaycastForCollider(mousePos, raycastLayer);

        if(hit.collider != null && secondaryInteractable == null)
        {
            if(hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable possibleInteractable))
            { //if hit is an interactable object
                secondaryInteractable = possibleInteractable;
            }
            else
            { //else null out secondaryInteractable
                secondaryInteractable = null;
            }
        }
        else if (hit.collider == null)
        { //if hit is nothing null out secondaryInteractable and secondaryHoveringOver
          //then call HoverExitWhileSelected on the current interactable
            secondaryHoveringOver = null;
            secondaryInteractable = null;
            currentInteractable.HoverExitWhileSelected(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
        }
        else if (secondaryInteractable != null && hoveringOver != hit.collider.gameObject && secondaryHoveringOver == null)
        { //if secondaryInteractable is not null and the hit object is not the current hovering object and secondaryHoveringOver is null
          //then call HoverEnterWhileSelected on the current interactable
            secondaryHoveringOver = hit.collider.gameObject;
            //Debug.Log("Possible Interactable");
            currentInteractable.HoverEnterWhileSelected(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
        }
    }

    public void RaycastForInteractables()
    {
        /*
        raycast for interactables
        if raycast hits nothing then call hover exit if hovering over is not null. Also null out the current interactable, hovering over and hovering over SO. Then Return
        if raycast hits an interactable object and it not the current hovering object then call the OnHoverExit function on the current interactable if it is not null
        if the possible interactable's OnHoverEnter function returns true then set the current interactable to the possible interactable and set the hovering over to the hit object
        */
        Vector3 mousePos = ReturnMousePosition();
        RaycastHit2D hit = RaycastForCollider(mousePos, raycastLayer);

        if (hit.collider == null)
        { //if hit is nothing
            if (hoveringOver != null)
            { //if hovering over is not null
                currentInteractable.OnHoverExit(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
            }
            currentInteractable = null;
            hoveringOver = null;
            hoveringOverSO.data = null;
            return;
        }

        if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable possibleInteractable) && hoveringOver != hit.collider.gameObject)
        { // if hit is an interactable object and the object is not the current hovering object
            if (hoveringOver != null)
            { //if hovering over is not null then call the OnHoverExit function on the current interactable
                currentInteractable.OnHoverExit(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
            }
            if (possibleInteractable.interactionManager.OnHoverEnter(this, hit.collider.gameObject.GetComponent<GardenObject_MonoBehavior>()))
            { //if the OnHoverEnter function returns true then set the current interactable to the possible interactable and set the hovering over to the hit object
                hoveringOver = hit.collider.gameObject;
                hoveringOverSO.data = hoveringOver;
                currentInteractable = possibleInteractable.interactionManager;
            }
        }
    }
    #endregion

    public bool MoveAnimalToPoint(Vector3 pos, RaycastHit2D gardenHit)
    {
        if (gardenHit.collider != null && currentSelection == gardenHit.collider.gameObject) { return false; } //if the hit object is the current selection then return false

        if (gardenHit.collider != null && gardenHit.collider.gameObject.TryGetComponent<Plant_MonoBehavior>(out Plant_MonoBehavior plant)
        && currentSelection.TryGetComponent<Animal_MonoBehavior>(out Animal_MonoBehavior animal) && plant.plantState == PlantState.Mature)
        { //if the hit object is a plant and the current selection is an animal
            if (animal.isTamed == false) { return false; }

            Debug.Log("Animal Eating Plant");
            animal.interest = gardenHit.collider.gameObject;
            animal.navMeshAgent.SetDestination(gardenHit.point);
            animal.SetState(walkToPointState);
            Deselect();
            return true;
        }
        else
        {
            RaycastHit2D groundHit = RaycastForCollider(pos, groundLayer);
            if (currentSelection.TryGetComponent<Animal_MonoBehavior>(out Animal_MonoBehavior _animal))
            {
                if (_animal.isTamed == false) { return false; }
                Debug.Log("Moving Animal to Point");
                //if hit collider hit something then set the animals destination to the hit point
                if (groundHit.collider != null)
                {
                    Debug.Log("Got here");
                    _animal.navMeshAgent.SetDestination(groundHit.point);
                    //set the animal's state to walk to point
                    _animal.SetState(walkToPointState);
                    Deselect();
                    return true;
                }
                return false;
            }
        }

        return false;
    }

    private void LeftClick_PlantObject()
    {
        int price = SeedSelectionMenu.Instance.GetCurrentPlantCost();
        //Debug.Log($"Planting {SeedSelectionMenu.Instance.GetCurrentPlant().name} for {price}");
        if (moneySO.data < price) { return; }

        Vector3 mousePos = ReturnMousePosition();
        RaycastHit2D hit = RaycastForCollider(mousePos, groundLayer);
        RaycastHit2D gardenHit = RaycastForCollider(mousePos, raycastLayer);

        if (gardenHit.collider == null && hit.collider != null && !hit.collider.gameObject.GetComponent<GardenObject_MonoBehavior>())
        {
            //Debug.Log($"Trying to plant hit {hit.collider.gameObject.name}");
            SeedSelectionMenu.Instance.GetCurrentPlant();
            Instantiate(SeedSelectionMenu.Instance.GetCurrentPlant(), hit.point, Quaternion.identity);
            moneySO.data -= price;
            moneySpentAction.InvokeAction();
        }
    }

    public void Deselect()
    {
        if (currentInteractable != null)
        {
            spriteMaterial.SetColor("_Color", defaultColor);
            currentInteractable.Deselect(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
            //turn on any collider on the current selection
            if (currentSelection != null)
            {
                currentSelection.GetComponent<Collider2D>().enabled = true;
            }
            currentSelection = null;
            currentInteractable = null;
            hoveringOver = null;
            selectionMenuUI.Deselect();
            onDeselectionEvent.Invoke();
            //transform.parent = null;
        }
    }

    public void SoftDeSelect()
    {
        if (currentInteractable == null) return;
        currentSelection = null;
        currentInteractable.SoftDeSelect(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
    }

    public void UpdateManagerSelectionDetails()
    {
        if (currentInteractable == null) return;
        currentInteractable.UpdateManagerSelectionsDetails(this, hoveringOver.GetComponent<GardenObject_MonoBehavior>());
    }

    public bool IsSelectionManagerUsingObject(GameObject obj)
    {
        //Debug.Log($"Checking if currentSelection({currentSelection}) is passedObj({obj})");
        if (currentSelection == obj)
        {
            Deselect();
            return true;
        }
        return false;
    }
}