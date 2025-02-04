﻿using System;
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RipplesRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Ripples", true)]
public sealed class Ripples : PostProcessEffectSettings
{
    public enum RipplesMode
    {
        Radial = 0,
        OmniDirectional = 1,
    }

    [Serializable]
    public sealed class RipplesModeParam : ParameterOverride<RipplesMode> { }

    [DisplayName("Method"), Tooltip("")]
    public RipplesModeParam mode = new RipplesModeParam { value = RipplesMode.Radial };

    [Range(0f, 10), Tooltip("Strength")]
    public FloatParameter strength = new FloatParameter { value = 2f };

    [Range(1f, 10), Tooltip("Distance")]
    public FloatParameter distance = new FloatParameter { value = 5f };

    [Range(0f, 10), Tooltip("Speed")]
    public FloatParameter speed = new FloatParameter { value = 3f };

        [Range(0f, 5), Tooltip("Width")]
    public FloatParameter width = new FloatParameter { value = 1.5f };

        [Range(0f, 5), Tooltip("Height")]
    public FloatParameter height = new FloatParameter { value = 1f };

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        if (enabled.value)
        {
            if (strength == 0) { return false; }
            return true;
        }

        return false;
    }
}

public sealed class RipplesRenderer : PostProcessEffectRenderer<Ripples>
{
    Shader ripplesShader;

    public override void Init()
    {
        ripplesShader = Shader.Find("Hidden/SC Post Effects/Ripples");
    }

    public override void Release()
    {
        base.Release();
    }

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(ripplesShader);

        sheet.properties.SetFloat("_Strength", (settings.strength * 0.01f));
        sheet.properties.SetFloat("_Distance", (settings.distance * 0.01f));
        sheet.properties.SetFloat("_Speed", settings.speed);
        sheet.properties.SetVector("_Size", new Vector2(settings.width, settings.height));

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
    }

}
#endif