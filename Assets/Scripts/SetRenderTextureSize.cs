using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderTextureSize : MonoBehaviour
{
    public Camera renderCamera;

    // Start is called before the first frame update
    void Start()
    {
        AdjustScaleToFitCamera();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustScaleToFitCamera();
    }

    void AdjustScaleToFitCamera()
    {
        if (renderCamera == null)
        {
            Debug.LogError("Render Camera is not assigned.");
            return;
        }

        // Assuming the camera is orthographic
        if (!renderCamera.orthographic)
        {
            Debug.LogError("Render Camera is not orthographic.");
            return;
        }

        float cameraHeight = 2f * renderCamera.orthographicSize;
        float cameraWidth = cameraHeight * renderCamera.aspect;

        transform.localScale = new Vector3(cameraWidth, cameraHeight, 1f);
    }
}