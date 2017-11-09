Shader "Custom/ShowAlways" {
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{ "Queue" = "Geometry+1" }

		Pass {
		Cull Off
		ZTest Always
		ZWrite Off
		Color[_Color]
		}
	}
	
}
