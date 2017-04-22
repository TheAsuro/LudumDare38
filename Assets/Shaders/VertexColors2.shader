Shader "Custom/VertexColors2" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MountainBorder ("Mountain Border", Range(0,1)) = 0.68
		_WaterColor ("Water Color", Color) = (1,1,1,1)
		_GroundColor ("Ground Color", Color) = (1,1,1,1)
		_MountainColor ("Mountain Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
            float4 color : COLOR;
		};

		half _Glossiness;
		half _Metallic;
		half _MountainBorder;
		float4 _WaterColor;
		float4 _GroundColor;
		float4 _MountainColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {

            half noiseValue = IN.color.a;

            if (noiseValue <= _MountainBorder)
                o.Albedo = lerp(_WaterColor, _GroundColor, noiseValue);
            else
                o.Albedo = lerp(_GroundColor, _MountainColor, (noiseValue - _MountainBorder) / 0.2);

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
