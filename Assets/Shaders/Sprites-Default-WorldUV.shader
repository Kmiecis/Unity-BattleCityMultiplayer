Shader "Sprites/Default-WorldUV"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Tiling ("Tiling", Vector) = (0,0,0,0)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            //"DisableBatching"="True" // Required if we rely on unity_ObjectToWorld
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVertWorld
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            
            float4 _Tiling;
            float4 _Offset;

            v2f SpriteVertWorld(appdata_t IN)
            {
                v2f OUT = SpriteVert(IN);
                float4 objWorldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
                OUT.texcoord += _Offset.xy;
                OUT.texcoord += objWorldPos.xy;
                OUT.texcoord += _Tiling.xy * _Time.y;
                return OUT;
            }
        ENDCG
        }
    }
}
