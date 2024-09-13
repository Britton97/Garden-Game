using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : SerializedMonoBehaviour
{
    public TextMeshProUGUI timeText;
    [Range(0, 1)]
    public float dayStartProgress;
    public DataFloat_SO timeScale;
    private float time;
    public bool changeLights;
    public Light2D globalLight;
    public SpriteRenderer test;
    public Material shadowMaterial;
    [HorizontalGroup("Shadow Alpha Range")]
    public float minShadowAlpha;
    [HorizontalGroup("Shadow Alpha Range")]
    public float maxShadowAlpha;
    [SerializeField] Gradient gradient;
    [HorizontalGroup("Light Intensity Range")]
    public float minIntensity;
    [HorizontalGroup("Light Intensity Range")]
    public float maxIntensity;
    [HorizontalGroup("Shadow Intensity Range")]
    public float minShadowIntensity;
    [HorizontalGroup("Shadow Intensity Range")]
    public float maxShadowIntensity;
    //public GameObject lightRotationParent;
    [HorizontalGroup("Light Rotation Range")]
    public float minRotation;
    [HorizontalGroup("Light Rotation Range")]
    public float maxRotation;
    [SerializeField] AnimationCurve lightIntensityCurve;
    public GameAction lightAction;
    public DataFloat_SO dayProgress;
    [Unit(Units.Second)]
    public int checkRequirementsFrequency;
    public GameAction checkRequirementsAction;

    private void Start()
    {
        //shadowMaterial = test.material;
        //shadowMaterial.SetFloat("_shadowBaseAlpha", .69f);
        time = 24 * 60 * 60 * dayStartProgress;
        StartCoroutine(UpdateTime());
        StartCoroutine(TimeCheckRequirements());
    }

    //coroutine that calls once the checkRequirementsFrequency has passed
    public IEnumerator TimeCheckRequirements()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkRequirementsFrequency);
            //Debug.Log("Calling checking requirements");
            checkRequirementsAction.InvokeAction();
        }
    }

    public Transform lightRotationObject; // The object to rotate

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            time += Time.deltaTime * timeScale.data;
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            dayProgress.data = (time % (24 * 60 * 60)) / (24 * 60 * 60);

            // Format time as 12-hour clock with AM/PM
            string timeString = string.Format("{0:D2}:{1:D2} {2}",
                timeSpan.Hours > 12 ? timeSpan.Hours - 12 : (timeSpan.Hours == 0 ? 12 : timeSpan.Hours),
                timeSpan.Minutes,
                timeSpan.Hours >= 12 ? "PM" : "AM");

            timeText.text = timeString;

            // Change the color and intensity of the light
            if (changeLights)
            {
                float timeFraction = (time % (24 * 60 * 60)) / (24 * 60 * 60); // Get the fraction of the day that has passed
                Color color = gradient.Evaluate(timeFraction); // Get the color from the gradient at the current time
                globalLight.color = color; // Set the color of the light

                float intensityFraction = lightIntensityCurve.Evaluate(timeFraction); // Get the intensity fraction from the curve at the current time
                float intensity = minIntensity + (maxIntensity - minIntensity) * intensityFraction; // Calculate the actual intensity
                globalLight.intensity = intensity; // Set the intensity of the light
                globalLight.shadowIntensity = minShadowIntensity + (maxShadowIntensity - minShadowIntensity) * intensityFraction; // Set the shadow intensity of the light

                //lightingSystem._shadowAlpha.value = minShadowAlpha + (maxShadowAlpha - minShadowAlpha) * intensityFraction; // Set the shadow alpha of the light
                //shadowMaterial.SetFloat("_shadowBaseAlpha", minShadowAlpha + (maxShadowAlpha - minShadowAlpha) * intensityFraction); // Set the shadow alpha of the light
                lightAction.InvokeAction(); // Invoke the light action
            }

            // Rotate the light
            //float rotationRange = maxRotation - minRotation;
            //float rotation = minRotation + rotationRange * ((time % (24 * 60 * 60)) / (24 * 60 * 60));
            //lightRotationObject.rotation = Quaternion.Euler(lightRotationObject.rotation.eulerAngles.x, lightRotationObject.rotation.eulerAngles.y, rotation);
            yield return null;
        }
    }
}