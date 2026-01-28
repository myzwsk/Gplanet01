Shader "Custom/WaterClip"
{
    Properties
    {
        _Color ("Color", Color) = (0.6,0.8,1,0.6)
        _MinY ("Min Y", Float) = 0
        _MaxY ("Max Y", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _MinY;
            float _MaxY;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.worldPos.y < _MinY || i.worldPos.y > _MaxY)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
