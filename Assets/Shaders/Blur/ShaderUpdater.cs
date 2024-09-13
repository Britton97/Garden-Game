using UnityEngine;

public class ShaderUpdater : MonoBehaviour
{
    public Material material; // Reference to the material using your shader
    public Camera renderCamera; // Reference to the camera that renders to the texture
    public RenderTexture renderTexture; // Reference to the RenderTexture
    public float updateInterval = 0.5f; // Interval in seconds between updates

    // Properties for dynamic updates
    public Color layerColor = Color.white; // Color of the layers
    public int layerCount = 10; // Number of layers
    public float pixelsPerUnit = 32.0f; // Pixels per unit
    public int maxSearchRange = 5; // Maximum search range for the shader

    private void Start()
    {

        if (renderCamera != null && renderTexture != null)
        {
            StartCoroutine(UpdateRenderTexturePeriodically());
        }
        else
        {
            Debug.LogWarning("Render Camera or Render Texture not assigned.");
        }

        /*
        if (material != null)
        {
            StartCoroutine(UpdateShaderPeriodically());
        }
        else
        {
            Debug.LogWarning("Material not assigned.");
        }
        */
    }

    private System.Collections.IEnumerator UpdateShaderPeriodically()
    {
        while (true)
        {
            Debug.Log("Updating shader properties...");
            // Update shader properties dynamically
            material.SetColor("_LayerColor", layerColor);
            material.SetInt("_LayerCount", layerCount);
            material.SetFloat("_PixelsPerUnit", pixelsPerUnit);
            material.SetInt("_MaxSearchRange", maxSearchRange);

            // Wait for the next update
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private System.Collections.IEnumerator UpdateRenderTexturePeriodically()
    {
        while (true)
        {
            Debug.Log("Updating Render Texture properties...");

            // Enable the camera to render to the texture
            renderCamera.enabled = true;
            yield return null; // Wait for one frame to ensure rendering is complete
            renderCamera.enabled = false;

            // Wait for the next update
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
