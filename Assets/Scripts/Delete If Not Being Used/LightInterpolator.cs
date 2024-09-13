using UnityEngine;
//using UnityEngine.Rendering.Universal;
//using System.Collections;
//using System.Collections.Generic;

public class LightInterpolator : MonoBehaviour
{
    /*
    public Light2D freeformLight; // The freeform light to interpolate
    public List<Light2D> freeformLights; // The freeform lights to interpolate
    public float duration; // The duration of the interpolation
    public float refreshRate = 0.1f; // The refresh rate in seconds

    private IEnumerator currentCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (freeformLights.Count > 0)
        {
            currentCoroutine = InterpolatePositions();
            StartCoroutine(currentCoroutine);
        }
    }

    private IEnumerator InterpolatePositions()
    {
        int currentLightIndex = 0;

        while (true)
        {
            Vector3[] startPos = freeformLights[currentLightIndex].shapePath;
            Vector3[] endPos = freeformLights[(currentLightIndex + 1) % freeformLights.Count].shapePath;

            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                float t = (Time.time - startTime) / duration;
                Vector3[] newPath = new Vector3[freeformLight.shapePath.Length];
                for (int i = 0; i < newPath.Length; i++)
                {
                    newPath[i] = Vector3.Lerp(startPos[i], endPos[i], t);
                }
                freeformLight.SetShapePath(newPath);
                yield return new WaitForSeconds(refreshRate);
            }

            currentLightIndex = (currentLightIndex + 1) % freeformLights.Count;
        }
    }
    */

    void Start()
    {
        Debug.LogWarning("Testing to see if this script is being used");
    }
}