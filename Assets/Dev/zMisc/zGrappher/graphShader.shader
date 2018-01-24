Shader "Z/graphShader"
{
	Properties
	{
		_MainTex ("A", 2D) = "white" {}
		_MainTex2 ("B", 2D) = "white" {}
		_Amp ("_Amp", Range(0,3)) = 0.0	
		_Fade ("_Fade	", Range(0,3)) = 0.0
		_Avg ("_Avg	", Range(0,.1)) = 0.0
			_MainCol1("Color", Color) = (1,1,1,1)
		
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _MainTex2;
			float4 _MainTex_ST;
			float4 _MainCol1;
			float _Amp;
			float _Avg;
			float _Fade;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float y=(i.uv.y-0.5)*2;
				if (y<0) y*=-1;
		
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col2 = tex2D(_MainTex2, i.uv);
			//	float2 uvoffs=float2(_Avg,0);

	/*fixed4 col2 = tex2D(_MainTex, i.uv+uvoffs);
	fixed4 col3 = tex2D(_MainTex, i.uv-uvoffs);
	col=(col+col2+col3)/3;
				if (col.r*_Amp>y)
				 col=_MainCol1; 
				 else 
				 
				 
				 col=_MainCol1*(col.r*_Amp);
				//if (y<0) col=float4(col.r,0,0,1);*/
			  return col*_Fade+col2*(1-_Fade);
			}
			ENDCG
		}
	}
}
