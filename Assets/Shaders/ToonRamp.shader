Shader "Custom/Fragment Cel-Shader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Base Color", Color) = (1,1,1,1)
		_UnlitColor("Shadow Color", Color) = (0.5,0.5,0.5,1)
		_UnlitThreshold("Shadow Range", Range(0,1)) = 0.1
		_HalfLitThreshold("Dimming Factor", Range(0,1)) = 0.5
		_HighlightThreshold("Highlight Range", Range(0.8,1)) = 0.98
		_HighlightMultiplier("Highlight Brightness", Range(1, 2)) = 1.5
		_LightingInfluence("Lighting Influence", Range(0,2)) = 1.0
		_PointLightInfluence("Point Light Influence", Range(0, 1)) = 0.5
	}

		SubShader
	{
	Tags{"RenderType" = "Opaque"}
	LOD 100
	//ZWrite Off
	//Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase 

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			float4 _LightColor0;

			float4 _MainTex_ST;
			float4 _Color;
			float4 _UnlitColor;
			float _LightingInfluence;
			float _UnlitThreshold;
			float _HalfLitThreshold;
			float _HighlightThreshold;
			sampler2D _MainTex;
			float _HighlightMultiplier;
			float _PointLightInfluence;

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
				float3 vertexLighting : TEXCOORD4;
			};

			v2f vert(appdata IN)
			{
				v2f OUT;
				
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

				OUT.posWorld = mul(modelMatrix, IN.vertex);
				OUT.normalDir = normalize(
					mul(float4(IN.normal, 0.0), modelMatrixInverse).xyz
				);

				TRANSFER_VERTEX_TO_FRAGMENT(OUT);
				return OUT;
			}

			float4 frag(v2f IN) : COLOR
			{
				float3 normalDirection = normalize(IN.normalDir);
				float3 lightDirection;
				float attenuation;
				float3 fragmentColor;
				
				//Shaded Color
				fragmentColor = tex2D(_MainTex, IN.uv) * _UnlitColor.rgb * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);

				if (0.0 == _WorldSpaceLightPos0.w) // directional light?
				{
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
					else if (lightAmount >= _UnlitThreshold * 0.5 * _HalfLitThreshold) {
						fragmentColor = tex2D(_MainTex, IN.uv) * (_UnlitColor.rgb * 1.5) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);
					}
				}
				else {
					attenuation = LIGHT_ATTENUATION(IN);
					lightDirection = normalize(_WorldSpaceLightPos0.xyz - IN.posWorld.xyz).xyz;
					float lightAmount = attenuation * max(0.0, dot(normalDirection, lightDirection));

					//Shaded Color
					fragmentColor = 0;

					//Lit Color
					if (lightAmount >= _UnlitThreshold) {
						if (lightAmount >= _HighlightThreshold) {
							fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * _HighlightMultiplier;
						}
						else {
							fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);
						}
					}
					else if (lightAmount >= _UnlitThreshold * 0.5 * _HalfLitThreshold) {
						fragmentColor = tex2D(_MainTex, IN.uv) * (_UnlitColor.rgb * 1.5) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);
					}
				}

				//Highlight Color
				return float4(fragmentColor, 1.0);
			}
			ENDCG
		}
		
		//FORWARD ADD PASS
		Pass
		{
			Tags {"LightMode" = "ForwardAdd"}
			Blend One One // Additive

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			float4 _LightColor0;


			float4 _Color;
			float4 _UnlitColor;
			float _LightingInfluence;
			float _UnlitThreshold;
			float _HalfLitThreshold;
			float _HighlightThreshold;
			sampler2D _MainTex;
			float _HighlightMultiplier;
			float _PointLightInfluence;

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
				float3 vertexLighting : TEXCOORD4;
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

				TRANSFER_VERTEX_TO_FRAGMENT(OUT);
				return OUT;
			}

			float4 frag(v2f IN) : COLOR
			{
				float3 normalDirection = normalize(IN.normalDir);
				float3 lightDirection;
				float attenuation;
				float3 fragmentColor;

				//Shaded Color
				fragmentColor = tex2D(_MainTex, IN.uv) * _UnlitColor.rgb * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);

				if (0.0 == _WorldSpaceLightPos0.w) // directional light?
				{
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
					else if (lightAmount >= _UnlitThreshold * 0.5 * _HalfLitThreshold) {
						fragmentColor = tex2D(_MainTex, IN.uv) * (_UnlitColor.rgb * 1.5) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence);
					}
				}
				else {
					attenuation = LIGHT_ATTENUATION(IN);
					lightDirection = normalize(_WorldSpaceLightPos0.xyz - IN.posWorld.xyz).xyz;
					float lightAmount = attenuation * max(0, dot(normalDirection, lightDirection));

					//Shaded Color
					fragmentColor = 0;

					//Lit Color
					if (lightAmount >= _UnlitThreshold) {
						if (lightAmount >= _HighlightThreshold) {
							fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * _HighlightMultiplier * lightAmount;
						}
						else {
							fragmentColor = tex2D(_MainTex, IN.uv) * _Color.rgb * (_LightColor0.rgb * _LightingInfluence * lightAmount);
						}
					}
				}

				//Highlight Color
				return float4(fragmentColor * _PointLightInfluence, 1.0);
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}