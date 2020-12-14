﻿Shader "Unlit/451Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 vertexWC : TEXCOORD3;
				float3 normal :NORMAL;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float4x4 MyXformMat;
			fixed4 MyColor;

			float4 LightPosition;
			fixed4 LightColor;
			float  LightNear;
			float  LightFar;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(MyXformMat, v.vertex);
				o.vertexWC = o.vertex;
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex);

				float3 p = v.vertex + 10 * v.normal;
				p = mul(MyXformMat, p);
				o.normal = normalize(p - o.vertexWC);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float ComputeDiffuse(v2f i) {
				float3 l = LightPosition - i.vertexWC;
				float d = length(l);
				l = l / d;
				float strength = 1;

				float ndotl = clamp(dot(i.normal, l), 0, 1);
				if (d > LightNear) {
					if (d < LightFar) {
						float range = LightFar - LightNear;
						float n = d - LightNear;
						strength = smoothstep(0, 1, 1.0 - (n * n) / (range * range));
					}
					else {
						strength = 0;
					}
				}
				return ndotl * strength;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * MyColor;

				float diff = ComputeDiffuse(i);
				return col * diff * LightColor;
			}
			ENDCG
		}
	}
}