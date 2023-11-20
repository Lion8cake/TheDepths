sampler uImage0 : register(s0);
sampler uImage1 : register(s1); // Automatically Images/Misc/Perlin via Force Shader testing option
sampler uImage2 : register(s2); // Automatically Images/Misc/noise via Force Shader testing option
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 BaseImage = tex2D(uImage0, coords);
    float2 FogCoords = (coords * uImageSize1 - uSourceRect.xy) / uImageSize2;
    float FogAsset = tex2D(uImage1, FogCoords + float2(uTime * 0.05, uTime * 0.05));
    float3 FogColor = uColor * FogAsset;
    return ((BaseImage) + ((float4(FogColor, 1) * (1 - uIntensity)) * uOpacity));
}

technique Technique1
{
    pass DepthsFogShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}