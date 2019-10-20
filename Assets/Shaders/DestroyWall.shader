//@febucci, https://www.febucci.com/tutorials/
//Support my work here: https://www.patreon.com/febucci

Shader "Custom/DestroyWall" {
	Properties{
	_HighlightColor("Highlight Color", Color) = (1,1,1,1)
	}

		SubShader{
		Tags { "RenderType" = "Fade" }

		CGPROGRAM
		#pragma surface surf Standard vertex:vert

		#include "UnityShaderVariables.cginc" //to use _Time

		float4 _HighlightColor;

		struct Input {
		float vertexPos;
		};

		void vert(inout appdata_full v, out Input o) {

		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.vertexPos = v.vertex.y; // passes the vertex data to pixel shader, you can change it to a different axis or scale it (divide or multiply the value)
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {

		float highlight_value = clamp( //clamps the value between 0 and 1 (since "cos" can go from -1 to 1)
		cos(
		_Time.y * 4 - //4 is the effect speed
		IN.vertexPos),
		0,
		1);

		o.Emission = lerp((0,0,0), _HighlightColor.xyz, highlight_value);
		}
		ENDCG
	}

		FallBack "Diffuse"
}
