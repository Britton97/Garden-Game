using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustColorIntensity : MonoBehaviour
{
    public DataFloat_SO dayProgress;
    public AnimationCurve lightIntensityCurve;
    public float minIntensity = 0.1f;
    public float maxIntensity = 1f;
    public Material material;

    public void UpdateColor()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, lightIntensityCurve.Evaluate(dayProgress.data));
        material.SetColor("_Color", Color.white * Mathf.Pow(2, intensity));
    }
}
