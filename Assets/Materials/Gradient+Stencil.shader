Shader "Unlit/Gradient+Stencil"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 0.7, 1)
		_Scale("Scale", float) = 5
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

		Pass
		{
			Stencil
			{
				Comp Always
				Ref 0
				Pass IncrSat
			}

			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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

			float4 _Color;
			float _Scale;
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color * (1 - length(i.uv) / _Scale);
			}
			ENDCG
		}
	}
}
