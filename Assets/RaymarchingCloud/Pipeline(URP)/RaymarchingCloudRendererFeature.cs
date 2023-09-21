using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RaymarchingCloudRendererFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Material m_material;

    [SerializeField]
    private RenderPassEvent m_renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

    private RaymarchingCloudRenderPass m_raymarchingCloudTexturePass = null;

    public override void Create()
    {
        if (m_raymarchingCloudTexturePass == null)
            m_raymarchingCloudTexturePass = new RaymarchingCloudRenderPass(m_material, m_renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_raymarchingCloudTexturePass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_raymarchingCloudTexturePass.SetRenderTarget(renderer.cameraColorTargetHandle);
    }
}
