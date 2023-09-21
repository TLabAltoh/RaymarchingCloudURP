using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RaymarchingCloudRenderPass : ScriptableRenderPass
{
    private const string NAME = nameof(RaymarchingCloudRenderPass);

    private int m_raymarchingTempID = 0;

    private RenderTargetIdentifier m_currentTarget = default;

    private Material m_material;

    public RaymarchingCloudRenderPass(Material material, RenderPassEvent passEvent)
    {
        renderPassEvent = passEvent;
        m_material = material;

        m_raymarchingTempID = Shader.PropertyToID("RaymarchingTempID");
    }

    public void SetRenderTarget(RenderTargetIdentifier target)
    {
        m_currentTarget = target;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!m_material) return;

        ref CameraData camData = ref renderingData.cameraData;

        CommandBuffer buf = CommandBufferPool.Get(NAME);

        RenderTextureDescriptor descriptor = camData.cameraTargetDescriptor;

#if true
        buf.GetTemporaryRT(m_raymarchingTempID, descriptor, FilterMode.Bilinear);
        buf.Blit(m_currentTarget, m_raymarchingTempID);
        buf.Blit(m_raymarchingTempID, m_currentTarget, m_material);
        buf.ReleaseTemporaryRT(m_raymarchingTempID);
#else
        buf.Blit(m_currentTarget, m_currentTarget, m_material);
#endif
        context.ExecuteCommandBuffer(buf);
        CommandBufferPool.Release(buf);
    }
}
