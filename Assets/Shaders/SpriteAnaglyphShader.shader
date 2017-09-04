// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/AnaglyphShader" {
	Properties {
		_MainTex ("Depth Map", 2D) = "white" {}
		_LuminancePattern("Encoding Texture", 2D) = "white" {}
		_DepthFactor("Depth Factor", Float) = 0.01
		_WheelAngle("Wheel Angle", Float) = 0
		_WheelScale("Wheel Scale", Float) = 0.5
		_WheelSize("WheelSize", Float) = 1.0  //Size = Zoom
		_WheelShear("Wheel Shear", Float) = 1.0
		_WheelXPos("Wheel XPos", Float) = 0
		_WheelYPos("Wheel YPos", Float) = 0
		//_WheelColor1("WheelColor1",Color) = (1, 0.18359375, 0, 1)//(0.6484375, 0.18359375, 0, 1)
		//_WheelColor2("WheelColor2", Color) = (0.32421875, 0.5703125, 0, 1) //(0.32421875, 0.2109375, 0, 1)
		_MaskAngle("Mask Angle", Float) = 0
		//_MaskScalePos("Mas Scale Pos", Float) = 0;
		_MaskScale("Mask Scale", Float) = 1.0
		_MaskShear("Mask Shear", Float) = 1.0
		_MaskSize("Mask Size", Float) = 1.0
		_TextureScale("Texture Scale", Float) = 2.0
		_MaskXPos("Mask XPos", Float) = 0
		_MaskYPos("Mask YPos", Float) = 0
		_FixPtRadius("Fix Point Radius", Float) = 0.03
		//_MaskColor("Mask Color", Color) = (1, 1, 1, 1)
		_MarkThreshold("Mark Threshold", Float) = 1.1 //range 0 to 1.0, use value above 1 to have no markers
		_MarkInnerEnd("Mark inner End", Float) = 0.01
		_MarkOuterEnd("Mark outer End", Float) = 0.8
		_MarkIsCross("Cross Markers", Float) = 0 //9: use color map maixma 1: use crossshaped markers
		}
	SubShader {
	 	LOD 200
		Tags { 
        	"Queue" = "Transparent"
        	"IgnoreProjector" = "True"
 			"RenderType"="Transparent"
		 }
		Pass {	
        	
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			//ColorMask RGB
			//Blend SrcColor One
			Blend Off
        
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
        	#include "UnityCG.cginc"
	
			sampler2D _MainTex;
			sampler2D _LuminancePattern;
			float _DepthFactor;
			float _WheelAngle;
			float _WheelScale;
			float _WheelSize;
			float _WheelShear;
			float _WheelXPos;
			float _WheelYPos;
			//half4 _WheelColor1;
			//half4 _WheelColor2;
			float _MaskAngle;
			//float _MaskScalePos;
			float _MaskScale;
			float _MaskShear;
			float _MaskSize;
			float _MaskXPos;
			float _MaskYPos;
			float _TextureScale;
			float _FixPtRadius;
			//half4 _MaskColor;

			float _MarkThreshold; //threshold fo depth to be collored (range 0 to 1.0, put above 1 to have no effect)
			float _MarkInnerEnd;
			float _MarkOuterEnd;
			float _MarkIsCross;
	
	        struct appdata_t
	        {
	            float4 vertex : POSITION;
	            float2 texcoord : TEXCOORD0;
	        };
	        struct v2f
	        {
	            float4 vertex : POSITION;
	            float2 texcoord : TEXCOORD0;
	        };
	
	        v2f vert (appdata_t v)
	        {
	            v2f o;
	            o.vertex = UnityObjectToClipPos(v.vertex);
	            o.texcoord = v.texcoord;
	            return o;
	        }
	
			half4 frag(v2f IN):COLOR {
				float2 uvd = IN.texcoord - 0.5; 
				float a = atan2(uvd.x, uvd.y);
				float r = length(uvd);
				float aw = a + radians(_WheelAngle);
				half4 depthColor = half4(0.5,0.5, 0.5,1);
				if ( r  <  0.5 * _WheelSize)
				{
					float2 uvw = _WheelScale / _WheelSize * r * float2(sin(aw) * _WheelShear ,cos(aw) * 1 / _WheelShear) + 0.5 + float2(_WheelXPos, _WheelYPos);
					depthColor = tex2D (_MainTex, uvw); //removed -0.5)
				}
				//float d = _DepthFactor * _MaskScale * (depthColor.r - 0.5);
				float d = _DepthFactor * (depthColor.r - 0.5);

				float am = 0;
				float2 uvm = uvd + float2(_MaskXPos, _MaskYPos);
				float ma = atan2(uvm.x, uvm.y);
				float mr = length(uvm);
				float amw = ma;
				float scale = 2 / _TextureScale;
				float shear = 1;
				if (mr  <  0.5 * _MaskSize)
				{
					am = radians(_MaskAngle);
					amw = ma + am;
					scale = _MaskScale / _TextureScale;
					shear = _MaskShear;					
				}
				float2 dm = scale * d * float2(cos(-am), sin(-am));
				float2 uvmw = scale * (mr * float2(sin(amw) * shear, cos(amw) * 1 / shear)) + 0.5;
			
				half4 lcol = tex2D (_LuminancePattern, uvmw - dm);
				half4 rcol = tex2D (_LuminancePattern, uvmw + dm);
				
				half4 destColor = half4(1, 1, 0, 1);
				destColor.r = lcol.r;
				destColor.gb = rcol.gb;
				//Markercode
				float cmr = depthColor.r;
				if (r  < 0.5 * _WheelSize)
				{
					if (cmr > _MarkThreshold) //return marker pixel
					{
						if (_MarkIsCross > 0.1)
						{
							float mc = fmod(abs(a), 0.78539816339744830961566084581988); //Pi / 4
							if ((mc < 0.1 || mc > 0.68539816339744830961566084581988) && r < _MarkOuterEnd)  //0.8
							{
								destColor.r = 0.5 * lcol.r + 0.5;
								destColor.g = 0.5 * rcol.r + 0.5;
								destColor.b = 0.1;
							}
						}
						else
						{
							if (r > _MarkInnerEnd &&  r < _MarkOuterEnd)  //0.8
							{
								destColor.r = 0.5 * lcol.r + 0.5;
								destColor.g = 0.5 * rcol.r + 0.5;
								destColor.b = 0.1;
							}
						}
					}
				}
				if (r < _FixPtRadius * _TextureScale)
					destColor = half4(0, 0, 1, 1);

				return destColor;
			}
			ENDCG
		} 
	}
	FallBack "Sprite/Diffuse"
}
