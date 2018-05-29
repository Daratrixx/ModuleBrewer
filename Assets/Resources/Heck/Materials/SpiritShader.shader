
Shader "Custom/Spirit Shader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_ShadowColor("Shadow color", Color) = (0,0,0,0)
		_ShadowStrength("Shadow strength", range(1,5)) = 1
	}
		SubShader{
			Tags{
				"Queue" = "Geometry"
				"RenderType" = "Opaque"
			}
			PASS {
				CGPROGRAM

				#pragma vertex vertexShader
				#pragma fragment fragmentShader
				#include "UnityCG.cginc"

				struct VertexOutput {
					float4 pos : POSITION;
					float3 norm : NORMAL;
					float3 dir : TEXCOORD2;
				};

				VertexOutput vertexShader(appdata_base v) {
					VertexOutput o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.dir = WorldSpaceViewDir(v.vertex);
					o.norm = UnityObjectToWorldNormal(v.normal);
					return o;
				}

				struct FragmentOutput {
					float4 col : COLOR;
				};

				uniform float4 _Color;
				uniform float4 _ShadowColor;
				uniform int _ShadowStrength;

				FragmentOutput fragmentShader(VertexOutput i) {
					FragmentOutput o;
					float light = dot(normalize(i.norm), float4(normalize(i.dir), 1));
					light = pow(light, _ShadowStrength);
					o.col = _Color * light + _ShadowColor * (1 - light);
					o.col.w = 0;
					return o;
				}

				ENDCG
			}
	}
		//FallBack "Diffuse"
}
