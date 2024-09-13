using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class WateringManager : MonoBehaviour
{
    public SpriteRenderer wateringcanSprite;
    public float waterAmount = 10;
    public LayerMask waterableLayer;
    [SerializeField]
    [ReadOnly]
    private bool isWatering = false;
    public UnityEvent startWateringEvent;
    public UnityEvent stopWateringEvent;

    void Update()
    {
        // Move the sprite renderer to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z position is 0 for 2D
        transform.position = mousePosition;
        if (isWatering)
        {
            CheckForWaterable();
        }
    }

    public void CheckForWaterable()
    {
        // Perform a 2D raycast from the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, waterableLayer);

        // Check if the object hit implements IWaterable
        if (hit.collider != null)
        {
            iWaterable waterable = hit.collider.GetComponent<iWaterable>();
            if (waterable != null)
            {
                // Perform actions on the waterable object
                //Debug.Log("Watering " + hit.collider.name);
                waterable.Water(waterAmount * Time.deltaTime);
            }
            //if hit is plant mono behavior
            if (hit.collider.TryGetComponent(out Plant_MonoBehavior plant))
            {
                if (plant.waterProgress >= .95f && plant._GifPlayer.IsGifAlreadyPlayingOrQueued("Happy"))
                {
                    plant._GifPlayer.PlayGif("Happy", 3f);
                }
            }
        }
    }

    public void ChangeWateringState()
    {
        if (isWatering)
        {
            StopWatering();
        }
        else
        {
            StartWatering();
        }
    }

    public void StartWatering()
    {
        isWatering = true;
        startWateringEvent.Invoke();
    }

    public void StopWatering()
    {
        isWatering = false;
        stopWateringEvent.Invoke();
    }
}