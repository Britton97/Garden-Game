using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float lerpTime = 1f; // Time to reach max speed
    [SerializeField] private Vector2 xMinMax; // Min and Max x position
    [SerializeField] private Vector2 yMinMax; // Min and Max y position
    private Vector2 inputVector;
    private float inputTime;

    void Update()
    {
        inputVector = playerInput.actions["Movement"].ReadValue<Vector2>();

        if (inputVector != Vector2.zero)
        { //if there is input
            inputTime += Time.deltaTime;
        }
        else
        { //if there is no input
            inputTime = Mathf.Max(inputTime - Time.deltaTime, 0);
        }

        // Normalize inputTime based on lerpTime
        float normalizedTime = Mathf.Clamp01(inputTime / lerpTime);
        float curveValue = movementCurve.Evaluate(normalizedTime);

        Vector3 newPos = new Vector3(
            transform.position.x + (inputVector.x * movementSpeed * curveValue * Time.deltaTime),
            transform.position.y + (inputVector.y * movementSpeed * curveValue * Time.deltaTime),
            transform.position.z
        );

        // Clamp the new position within the specified bounds
        newPos.x = Mathf.Clamp(newPos.x, xMinMax.x, xMinMax.y);
        newPos.y = Mathf.Clamp(newPos.y, yMinMax.x, yMinMax.y);

        transform.position = newPos;
    }
}