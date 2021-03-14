#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
#endif

matrix ViewProjection;

texture2D DiffuseTexture;
SamplerState TextureSampler = sampler_state 
{
	Texture = (DiffuseTexture);
	//magfilter = POINT; minfilter = POINT; mipfilter = POINT; //magfilter = linear;//minfilter = linear;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

// cheating here a little for spritebatch pos3d will actually be the texture color * the Draw(.. color ); 
// to do it right you have to effect.apply after begin to use the vertex shader and pass in a projection matrix.
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
	float4 Pos3d : TEXCOORD1;
};

struct PixelShaderOutput
{
	float4 ColorA : COLOR0;
	float4 ColorB : COLOR1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 pos = mul(input.Position, ViewProjection);
	output.Position = pos;
	output.Pos3d = pos;
	output.Color = input.Color;
	output.TextureCoordinates = input.TextureCoordinates;

	return output;
}

PixelShaderOutput MainPS(VertexShaderOutput input)  // : COLOR
{
	PixelShaderOutput  output; 

	output.ColorA = input.Pos3d;

	float4 texcolor = tex2D(TextureSampler, input.TextureCoordinates);
	output.ColorB = input.Color * texcolor;

	return output;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile 
			VS_SHADERMODEL MainVS();
		PixelShader = compile 
			PS_SHADERMODEL MainPS();
	}
};