#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

smapler s0 : register(s0);
float4 OverlayColor : register(c0);

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	return color * OverlayColor;
}

technique SpriteDrawing
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};