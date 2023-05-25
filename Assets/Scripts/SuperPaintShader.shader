Shader "Custom/SuperPaintShader"
{
    Properties
    {
        _MainTex ("不用贴东西", 2D) = "white" {}
		_BGTex("刷子图案", 2D) = "white" {}
		_BrushTex("刷子形状", 2D) = "white" {}
		_LimitTex("有效区域（白色为有效）", 2D) = "white" {}
		_Color("暂时没用",Color) = (1,1,1,1)
		_UV("自动调整的UV",Vector) = (0,0,0,0)
		_Size("刷子大小 越大越小",Range(1,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"= "Fade" }
        LOD 100
		ZTest Always Cull Off ZWrite Off Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _BrushTex;
			sampler2D _BGTex;
			sampler2D _LimitTex;
            float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _UV;
			float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float size = _Size;
				float2 uv = i.uv + (0.5f / size);
				uv = uv - _UV.xy;
				uv *= size;
                fixed4 col = tex2D(_BrushTex, uv);


				fixed4 colt = tex2D(_BGTex, i.uv);
				fixed4 coll = tex2D(_LimitTex, i.uv);

				colt.a = step(0.5f, colt.a);
				colt.a *= step(0.5f, col.a)*10000;
				colt.a = step(0.5f, colt.a);

				colt.a *= coll.r;

                return colt;
            }
            ENDCG
        }
    }
}
