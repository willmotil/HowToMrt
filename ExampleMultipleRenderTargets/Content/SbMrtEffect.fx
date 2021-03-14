#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

//matrix WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

// cheating here a little for spritebatch pos3d will actually be the texture color * the Draw(.. color ); 
// to do it right you have to effect.apply after begin to use the vertex shader and pass in a projection matrix.
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float4 Pos3d : TEXCOORD0;
};

struct PixelShaderOutput
{
	float4 ColorA : COLOR0;
	float4 ColorB : COLOR1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 pos = input.Position;  // mul(input.Position, WorldViewProjection);
	output.Position = pos;
	output.Pos3d = pos;
	output.Color = input.Color;

	return output;
}

PixelShaderOutput MainPS(VertexShaderOutput input)  // : COLOR
{
	PixelShaderOutput  output; // = (PixelShaderOutput)0;

	output.ColorA = input.Pos3d; //float4(1.0f, 1.0f, 1.0f, 1.0f);//input.Pos3d;
	output.ColorB = input.Color; // float4(1.0f, 1.0f, 1.0f, 1.0f); // input.Color;

	return output;
}

technique BasicColorDrawing
{
	pass P0
	{
		//VertexShader = compile 
		//	VS_SHADERMODEL MainVS();
		PixelShader = compile
			PS_SHADERMODEL MainPS();
	}
};