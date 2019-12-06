Shader "Custom/Tri-Planar Local"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Scale("Scale", Range(0.1, 10)) = 2
		_Gradient("_Gradient", Range(0.01, 10.0)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"IgnoreProjector" = "False"
			"RenderType" = "Opaque"
		}

		Cull Back
		ZWrite On

		CGPROGRAM

		#pragma surface surf Lambert
		#pragma exclude_renderers flash

		sampler2D _Texture;
		float _Scale;
		float _Gradient;

		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			float3 projNormal = normalize(pow(abs(IN.worldNormal), _Gradient)) + float3(0.0001, 0.0001, 0.0001);

			// Turn World position into local position, and get plane coordinates
			float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
			float2 x_coords = frac(localPos.zy * _Scale);
			float2 y_coords = frac(localPos.zx * _Scale);
			float2 z_coords = frac(localPos.xy * _Scale);

			// Calculate multipliers for the three planes
			float xy = lerp(projNormal.x, projNormal.y, projNormal.x / (projNormal.x + projNormal.y));
			float yz = lerp(projNormal.y, projNormal.z, projNormal.y / (projNormal.y + projNormal.z));
			float zx = lerp(projNormal.z, projNormal.x, projNormal.z / (projNormal.z + projNormal.x));

			float xxyz = lerp(xy, zx, projNormal.y / (projNormal.y + projNormal.z));
			float xyyz = lerp(yz, xy, projNormal.z / (projNormal.z + projNormal.x));
			float xyzz = lerp(zx, yz, projNormal.x / (projNormal.x + projNormal.y));
			float triple = (xxyz + xyyz + xyzz) / 16.4;

			float x = projNormal.x - (xy + zx) / 3.35 + triple;
			float y = projNormal.y - (xy + yz) / 3.35 + triple;
			float z = projNormal.z - (yz + zx) / 3.35 + triple;

			// Get Albedo Texture
			float3 x_col = tex2D(_Texture, x_coords);
			float3 y_col = tex2D(_Texture, y_coords);
			float3 z_col = tex2D(_Texture, z_coords);

			o.Albedo = x_col * x + y_col * y + z_col * z;
		}

		ENDCG
	}

	Fallback "Diffuse"
}