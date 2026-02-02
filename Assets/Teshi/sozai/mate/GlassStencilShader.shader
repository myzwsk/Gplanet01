Shader "Custom/GlassStencil"
{
    Properties
    {
        _Color ("Color", Color) = (0.5,0.8,1,0.4)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Stencil
        {
            Ref 1
            Comp Equal
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Color [_Color]
        }
    }
}
