using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AdjustLights : MonoBehaviour
{
    [SerializeField] Light2D selfLight;

    public DataFloat_SO dayProgress;
    public AnimationCurve lightIntensityCurve;
    public float minIntensity = 0.1f;
    public float maxIntensity = 1f;

    public void UpdateLight()
    {
        selfLight.intensity = lightIntensityCurve.Evaluate(dayProgress.data) * (maxIntensity - minIntensity) + minIntensity;
    }
}
