Shader "Unlit/earth"
{
	Properties
	{

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
			
			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct VertOut
			{
				float4 color : COLOR;
				float4 vertex : SV_POSITION;
			};
			
			VertOut vert (VertIn v)
			{
				VertOut o;
				o.color = v.color;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (VertOut i) : SV_Target
			{
				return fixed4(i.color);
			}
			ENDCG
		}
	}
}
