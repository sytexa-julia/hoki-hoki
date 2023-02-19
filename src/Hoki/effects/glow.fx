////////////// CONSTANTS //////////////
Texture		Tex;					//Texture currently being used
sampler		TexSampler=				//Sampler for textures
			sampler_state{
				texture = <Tex>;	//Texture register to use
				mipfilter=NONE;		//Filter for mipmaps (NONE=nearest neighbor)
			};
float4x4	World	: WORLD;		//World transformation matrix
float4x4	View	: VIEW;			//View transformation matrix
float4x4	Proj	: PROJECTION;	//Projection transformation matrix

float Blurriness=0.0f;
float TexWidth=0.0f;
float TexHeight=0.0f;

////////////// VERTEX SHADERS //////////////

//Transforms position, passes on texture coordinates and vertex color
void VSColTex(
	in	float4 inPos	: POSITION,
	in	float4 inCoord	: TEXCOORD0,
	in	float4 inDiff	: COLOR0,
	out	float4 outPos	: POSITION,
	out float4 outCoord	: TEXCOORD0,
	out float4 outDiff	: COLOR0
) {
	//Multiply InPos by World, View, Projection
	outPos=mul(inPos,mul(mul(World,View),Proj));
	outCoord=inCoord;
	outDiff=inDiff;
}

////////////// PIXEL SHADERS //////////////

//Returns texture sample color modulated with vertex color
float4 PSColTex(
	float2 TexCoord	: TEXCOORD0,
	float4 Diffuse	: COLOR0
) : COLOR {
	return tex2D(TexSampler,TexCoord)*Diffuse;
}

//List of adjacent tex coords to use when blurring
const float2 Samples[8] = {
   -1, -1,
    0, -1,
    1, -1,
   -1,  0,
    1,  0,
   -1,  1,
    0,  1,
    1,  1,
};

//Blurs the texture
float4 PSBlur(
	float2 TexCoord	: TEXCOORD0,
	float4 Diffuse	: COLOR0
) : COLOR {
	float4 avg=tex2D(TexSampler,TexCoord);
	float WidthRatio=Blurriness/TexWidth;
	float HeightRatio=Blurriness/TexHeight;
	for (int i=0;i<8;i++) avg+=tex2D(TexSampler,TexCoord+float2(WidthRatio,HeightRatio)/3.0*Samples[i]);
	for (int i=0;i<8;i++) avg+=tex2D(TexSampler,TexCoord+float2(WidthRatio,HeightRatio)/2.0*Samples[i]);
	for (int i=0;i<8;i++) avg+=tex2D(TexSampler,TexCoord+float2(WidthRatio,HeightRatio)*Samples[i]);
	return avg/25*Diffuse;
}

//Shifts x coord by sin(y)
float4 PSBend(
	float2 TexCoord	: TEXCOORD0,
	float4 Diffuse	: COLOR0
) : COLOR {
	float2 ShiftedCoord={0.0f,TexCoord.y};
	ShiftedCoord.x=saturate(TexCoord.x+sin(TexCoord.y*128.0)/128.0);
	return tex2D(TexSampler,ShiftedCoord);
}

technique Textured {
	pass P0 {
		//Blending states
		ColorOp[0]=MODULATE;
		ColorArg1[0]=TEXTURE;
		ColorArg2[0]=DIFFUSE;
		
		AlphaOp[0]=MODULATE;
		AlphaArg1[0]=TEXTURE;
		AlphaArg2[0]=DIFFUSE;
		
		SrcBlend=SRCALPHA;
		DestBlend=INVSRCALPHA;
		AlphaBlendEnable=true;
	
		VertexShader=compile vs_1_1 VSColTex();
		PixelShader=compile ps_1_1 PSColTex();
	}
}

technique Blurry {
	pass P0 {
		//Blending states
		ColorOp[0]=MODULATE;
		ColorArg1[0]=TEXTURE;
		ColorArg2[0]=DIFFUSE;
		
		AlphaOp[0]=MODULATE;
		AlphaArg1[0]=TEXTURE;
		AlphaArg2[0]=DIFFUSE;
		
		SrcBlend=SRCALPHA;
		DestBlend=INVSRCALPHA;
		AlphaBlendEnable=true;
		AlphaTestEnable=false;
		
		MinFilter[0]=LINEAR;
		MagFilter[0]=LINEAR;
	
		VertexShader=compile vs_1_1 VSColTex();
		PixelShader=compile ps_2_0 PSBlur();
	}
	
}

Technique Wavy {
	Pass P0 {
		//Blending states
		ColorOp[0]=MODULATE;
		ColorArg1[0]=TEXTURE;
		ColorArg2[0]=DIFFUSE;
		
		AlphaOp[0]=MODULATE;
		AlphaArg1[0]=TEXTURE;
		AlphaArg2[0]=DIFFUSE;
		
		SrcBlend=SRCALPHA;
		DestBlend=INVSRCALPHA;
		AlphaBlendEnable=true;
		
		VertexShader=compile vs_1_1 VSColTex();
		PixelShader=compile ps_2_0 PSBend();
	}
}