using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DitherRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class DitherSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material ditherMaterial = null;
    }

    public DitherSettings settings = new DitherSettings();

    class DitherRenderPass : ScriptableRenderPass
    {
        private Material ditherMaterial;
        private RTHandle tempTexture;

        public DitherRenderPass(Material material)
        {
            this.ditherMaterial = material;
            tempTexture = RTHandles.Alloc("_TempTexture", name: "_TempTexture");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cameraTextureDescriptor.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref tempTexture, cameraTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp);
            ConfigureTarget(tempTexture);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (ditherMaterial == null)
            {
                Debug.LogError("Dither Material is not set.");
                return;
            }

            var cmd = CommandBufferPool.Get("Dither Pass");

            var cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Blit using RTHandles
            Blitter.BlitCameraTexture(cmd, cameraColorTargetHandle, tempTexture, ditherMaterial, 0);
            Blitter.BlitCameraTexture(cmd, tempTexture, cameraColorTargetHandle);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            tempTexture.Release();
        }
    }

    DitherRenderPass ditherPass;

    public override void Create()
    {
        if (settings.ditherMaterial == null)
        {
            Debug.LogError("Dither Material not set in DitherRenderFeature.");
            return;
        }

        Debug.Log($"Assigned Dither Material: {settings.ditherMaterial.name}");

        ditherPass = new DitherRenderPass(settings.ditherMaterial)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }


    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.ditherMaterial == null)
        {
            Debug.LogWarning("Missing Dither Material");
            return;
        }

        renderer.EnqueuePass(ditherPass);
    }
}
