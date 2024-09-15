using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class Animal_MonoBehavior : GardenObject_MonoBehavior, iRequirements, iDirectable, iEmoji, iWaterable, iMatable
{
    #region Variables
    public GameObject interest;
    public bool isTamed = false;
    public NavMeshAgent navMeshAgent;
    public Animal_SO animalObject;
    [BoxGroup("Hunger")]
    public float maxHunger = 100;
    [BoxGroup("Hunger")]
    public float hunger = 100;
    [BoxGroup("Hunger")]
    public float hungerRate = 1;
    [ReadOnly] public float idleTime = 0;
    public SpriteRenderer _spriteRenderer;

    //public List<BaseState_Animal> states = new List<BaseState_Animal>();
    public BaseState_Animal currentState;
    public List<AnimalChecklist> localGardenItemRequirements = new List<AnimalChecklist>();

    [SerializeField] private GifPlayer gifPlayer;
    public GifPlayer _GifPlayer => gifPlayer;
    #endregion
    #region Start and Update
    public override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        hunger = maxHunger;
        //currentState.EnterState(this);
        if (isTamed)
        {
            _spriteRenderer.color = animalObject.tamedColor;
        }
        else
        {
            _spriteRenderer.color = animalObject.untamedColor;
        }
        MakeLocalCopyOfRequirements();
        StartCoroutine(WaitForOneFrame());
        checkForObjectRequirementsCoroutine = StartCoroutine(CheckRequirementsPeriodically());

        //check the animal_so to see if the animal has been seen before
        //if it has not been seen before then add experience to the unlockable tree and set the first time seen to true
        if (!animalObject.FirstTimeSeen)
        {
            animalObject.alreadySeenOnce = true;
            animalObject.unlockableTree.AddExperience((int)animalObject.FirstTimeSeenExperience);
            //add this animal to the seen animal list
            animalObject.AddToSeenAnimalList(animalObject);
        }
    }

    void Update()
    {
        currentState.UpdateState(this);
        DrainHunger();
        FlipSpriteRenderer();
    }
    #endregion
    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.StateOnTriggerEnter2D(this, other);
    }
    #endregion
    #region Requirements
    public void MakeLocalCopyOfRequirements()
    {
        localGardenItemRequirements.Clear();
        //Debug.Log($"Count: {animalObject.itemRequirements.Count}");
        foreach (ItemRequirement_Abs requirement in animalObject.tameRequirements)
        {
            localGardenItemRequirements.Add(new AnimalChecklist(requirement));
        }
    }

    private Coroutine checkForObjectRequirementsCoroutine;
    private IEnumerator CheckRequirementsPeriodically()
    {
        while (true)
        {
            CheckForObjectRequirements();
            yield return new WaitForSeconds(10f);
        }
    }

    public void CheckForObjectRequirements()
    {
        //check to local copy of requirements and see if any of those requirements is a Object_SO
        //if it is then check if the object is in the scene and the quantity is met
        //if it is then set the requirement to true
        foreach (AnimalChecklist item in localGardenItemRequirements)
        {
            //Debug.Log($"Checking for {item.itemRequirement.GetName()} ({item.itemRequirement.GetCount()}) on {animalObject.GardenObjectName}");
            if (item.isMet) { continue; }
            if (item.itemRequirement is GardenItemRequirement)
            {
                //cast the item requirement to a GardenItemRequirement
                GardenItemRequirement gardenItemRequirement = (GardenItemRequirement)item.itemRequirement;
                if (gardenItemRequirement.requirementType == RequirementType.ItemInGarden)
                {
                    item.currentQuantity = GardenManager.Instance.GetQuantityOfItem(gardenItemRequirement.ItemName);
                }
                if (item.currentQuantity >= item.itemRequirement.requiredQuantity)
                {
                    item.isMet = true;
                    animalObject.CheckForNextTamingUnlock(this);
                }
            }
            else if (item.itemRequirement is TileRequirement)
            {
                //Debug.Log($"Checking for {item.itemRequirement.GetName()} ({item.itemRequirement.GetCount()}) on {animalObject.GardenObjectName}");
                int tileCount = item.itemRequirement.GetCount();
                item.currentQuantity = tileCount;
                if (tileCount >= item.itemRequirement.requiredQuantity)
                {
                    item.isMet = true;
                    animalObject.CheckForNextTamingUnlock(this);
                }
            }
        }

        //check if all requirements are met
        bool allRequirementsMet = true;
        foreach (AnimalChecklist requirement in localGardenItemRequirements)
        {
            if (!requirement.isMet)
            {
                allRequirementsMet = false;
                break;
            }
        }

        if (allRequirementsMet)
        {
            isTamed = true;
            //discover all off the appear requirements and tame requirements
            animalObject.UnlockAllTameAndAppearRequirements();
            if (!animalObject.FirstTimeTame)
            {
                animalObject.alreadyTamedOnce = true;
                animalObject.unlockableTree.AddExperience((int)animalObject.FirstTimeTameExperience);
            }
            _spriteRenderer.color = animalObject.tamedColor;
            if (checkForObjectRequirementsCoroutine != null)
            {
                StopCoroutine(checkForObjectRequirementsCoroutine);
                checkForObjectRequirementsCoroutine = null;
            }
        }
    }

    public void CheckTamingRequirements(string foodName) //actually more like check requirements
    {
        foreach (AnimalChecklist requirement in localGardenItemRequirements)
        {
            requirement.CheckRequirement(foodName, this);
        }

        bool allRequirementsMet = true;
        foreach (AnimalChecklist requirement in localGardenItemRequirements)
        {
            if (!requirement.isMet)
            {
                allRequirementsMet = false;
                break;
            }
        }
        Debug.Log($"All Requirements Met: {allRequirementsMet} for {animalObject.GardenObjectName}");
        if (allRequirementsMet)
        {
            isTamed = true;
            animalObject.UnlockAllTameAndAppearRequirements();
            if (!animalObject.FirstTimeTame)
            {
                animalObject.alreadyTamedOnce = true;
                animalObject.unlockableTree.AddExperience((int)animalObject.FirstTimeTameExperience);
            }
            _spriteRenderer.color = animalObject.tamedColor;
            if (checkForObjectRequirementsCoroutine != null)
            {
                StopCoroutine(checkForObjectRequirementsCoroutine);
                checkForObjectRequirementsCoroutine = null;
            }
        }
    }

    public List<AnimalChecklist> GetRequirements()
    {
        return localGardenItemRequirements;
    }

    public bool ReportRequirements()
    {
        if (isTamed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion
    #region State Functions
    public IEnumerator WaitForOneFrame()
    {
        yield return null;
        currentState.EnterState(this);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetState(BaseState_Animal newState)
    {
        if (idleTimerCoroutine != null)
        {
            StopCoroutine(idleTimerCoroutine);
            idleTimerCoroutine = null;
            idleTime = 0;
        }
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState(this);
    }
    #endregion
    #region Interface Implementations
    public override string GetName()
    {
        return animalObject.GardenObjectName;
    }

    public override Sprite GetSprite()
    {
        return animalObject.gardenObjectSprite;
    }

    public override int GetSellPrice()
    {
        return (int)animalObject.sellPrice;
    }

    public override bool IsSellable()
    {
        if (isTamed)
        {
            return true;
        }
        return false;
    }

    public bool IsDirectable()
    {
        return isTamed;
    }

    public override bool IsEdible()
    {
        return isTamed;
    }

    public bool IsWaterable()
    {
        return isTamed;
    }

    public void Water(float waterAmount)
    {
        //play frustrated gif
        if (_GifPlayer.GetGifName() != "Frustrated")
        {
            _GifPlayer.PlayGif("Frustrated", 3f);
        }
    }

    private int lastMateTime = -999;
    public bool IsMatable()
    {
        //if the last mate time is greater than animalso.mateCooldown then return true
        if (Time.time - lastMateTime > animalObject.mateCooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Mate(Animal_MonoBehavior animal1, Animal_MonoBehavior animal2)
    {
        Debug.Log($"{animal1.animalObject.GardenObjectName} and {animal2.animalObject.GardenObjectName} are mating");
        lastMateTime = (int)Time.time;
        animal1._GifPlayer.PlayGif("Love", 3f);
        animal2._GifPlayer.PlayGif("Love", 3f);
        animal1.interest = null;
        animal2.interest = null;
        //create a new animal
        GameObject newAnimal = Instantiate(animal1.animalObject.objectPrefab, animal1.transform.position, Quaternion.identity);
        //tame the new animal
        Animal_MonoBehavior newAnimalMonoBehavior = newAnimal.GetComponent<Animal_MonoBehavior>();
        newAnimalMonoBehavior.isTamed = true;
        newAnimalMonoBehavior._GifPlayer.PlayGif("Happy", 3f);
    }

    #endregion
    #region Idle Timer / Functions
    [HideInInspector] public Coroutine idleTimerCoroutine;
    public void IdleOnTimer(float timeInIdle)
    {
        idleTime = timeInIdle;
        //start a coroutine that will wait for the time to pass then change
        if (idleTimerCoroutine != null)
        {
            StopCoroutine(idleTimerCoroutine);
            idleTimerCoroutine = null;
        }
        idleTimerCoroutine = StartCoroutine(IdleTimer());
    }

    public IEnumerator IdleTimer()
    {
        //while timer is greater than 0 decrement it
        while (idleTime > 0)
        {
            idleTime -= Time.deltaTime;
            yield return null;
        }
        idleTime = 0;
    }
    #endregion
    public void DrainHunger()
    {
        hunger = hunger > 0 ? hunger - hungerRate * Time.deltaTime : 0;
    }
    //function that will flip the sprite renderer depending on the direction of the animal only flip on the x axis
    public void FlipSpriteRenderer()
    {
        if (navMeshAgent.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (navMeshAgent.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    //coroutine that will make a timer to destroy the animal after a certain amount of time
    private Coroutine destroyAfterTimeCoroutine;
    public void DestroyAfterTime(float time)
    {
        if (destroyAfterTimeCoroutine == null)
        {
            destroyAfterTimeCoroutine = StartCoroutine(DestroyAfterTimeCoroutine(time));
        }
    }
    private IEnumerator DestroyAfterTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
