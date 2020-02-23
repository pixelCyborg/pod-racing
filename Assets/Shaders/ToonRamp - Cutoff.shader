// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'unity_World2Shadow' with 'unity_WorldToShadow'

Shader "Custom/Fragment Cel-Shader Cutoff"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Color("Base Color", Color) = (1,1,1,1)
		_UnlitColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
		_UnlitThreshold("Shadow Range", Range(0,1)) = 0.1
		_HighlightThreshold("Highlight Range", Range(0.8,1)) = 0.98
		_HighlightMultiplier("Highlight Brightness", Range(1, 2)) = 1.5
		_LightingInfluence("Lighting Influence", Range(0,2)) = 1.0
	}

		SubShader
	{
	Tags{"Queue" = "Transparent" "RenderType" = "Transparent" }
	LOD 100
		Pass
		{
		Tags {"LightMode" = "ForwardBase"}
			 ZWrite Off // don't write to depth buffer 
			Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase 

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			float4 _LightColor0;


			float4 _Color;
			float _Cutoff;
			float4 _UnlitColor;
			float _LightingInfluence;
			float _UnlitThreshold;
			float _HighlightThreshold;
			sampler2D _MainTex;
			float _HighlightMultiplier;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 posWorld : TEXCOORD0;
				float3 normalDir : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float4 _ShadowCoord : TEXCOORD3;
			};

			v2f vert(appdata IN)
			{
				v2f OUT;
				OUT.pos = UnityObjectToClipPos(IN.vertex);

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				OUT.uv = IN.uv;
				OUT.posWorld = mul(modelMatrix, IN.vertex);
				OUT.normalDir = normalize(
					mul(float4(IN.normal, 0.0), modelMatrixInverse).xyz
				);
				//OUT._ShadowCoord = mul(unity_WorldToShadow[0], mul(unity_ObjectToWorld, IN.vertex));
				
				return OUT;
			}

			float4 frag(v2f IN) : COLOR
			{
				float3 normalDirection = normalize(IN.normalDir);
				float3 lightDirection;
				float attenuation;
				float3 fragmentColor;


				float newOpacity = tex2D(_MainTex, IN.uv).a; //load cuttext
				if (newOpacity < _Cutoff) {
					newOpacity = 0.0;
				}

				//attenuation = 1.0;
				attenuation = LIGHT_ATTENUATION(IN);

				lightDirection = normalize(_WorldSpaceLightPos0).xyz;
				
				//Shaded Color
				fragmentColor = tex2D(_MainTex, IN.uv) * _UnlitColor.rgb * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);

				float lightAmount = attenuation * max(0.0, dot(normalDirection, lightDirection));
				//Lit Color
				if (lightAmount >= _UnlitThreshold) {
					if (lightAmount >= _HighlightThreshold) {
							fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * _HighlightMultiplier;
					} 
					else {
						fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);
					}
				}

				//Highlight Color
				return float4(fragmentColor, newOpacity);
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}