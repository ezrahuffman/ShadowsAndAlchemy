using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(PostProcessOutlineComposite), PostProcessEvent.AfterStack, "Composite")]
public class PostProcessComposite : PostProcessEffectSettings
{
    public ColorParameter color = new ColorParameter() { value = Color.white };
}

public class PostProcessOutlineComposite : PostProcessEffectRenderer<PostProcessComposite>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Unlit/OutlineCompositeShader"));
        sheet.properties.SetColor("_Color", settings.color);

        if (PostProcessOutlineRenderer.outlineRendererTexture != null)
        {
            sheet.properties.SetTexture("_OutlineTex", PostProcessOutlineRenderer.outlineRendererTexture);
        }

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

    }
}
