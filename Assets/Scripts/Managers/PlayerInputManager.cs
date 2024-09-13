using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInputManager : MonoBehaviour
{
    public Input_Controls inputControls;
    public GameObject targetObject;

    void OnEnable()
    {
        if (inputControls == null)
        {
            inputControls = new Input_Controls();
        }

        inputControls.PC_Controls.Enable();
        inputControls.PC_Controls.LeftClick.performed += LeftClick;
        inputControls.PC_Controls.Point.performed += MousePosition;
    }

    void OnDisable()
    {
        inputControls.PC_Controls.Disable();
        inputControls.PC_Controls.LeftClick.performed -= LeftClick;
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left Click");
        //move target object to mouse position without using input controls
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        //targetObject.transform.position = worldPosition;
        //raycast to see if we hit a garden object
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            //if we hit a garden object, call the print item name function
            //hit.collider.gameObject.GetComponent<GardenObject_MonoBehavior>().PrintItemName();
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        Debug.Log("Mouse Position");
        targetObject.transform.position = worldPosition;
    }
}
