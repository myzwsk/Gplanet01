Shader "Custom/BathtubMask"
{
    SubShader
    {
        Tags { "Queue"="Geometry-1" }

        Pass
        {
            ColorMask 0   
            ZWrite Off

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
        }
    }
}
