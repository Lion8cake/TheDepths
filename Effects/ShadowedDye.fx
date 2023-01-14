sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    float4 noise = tex2D(uImage1, noiseCoords);
    color.rgb = noise.b * 2;
    return color * sampleColor;
    //float4 color = tex2D(uImage0, coords);
    //float2 noiseCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    //float4 noise = tex2D(uImage1, noiseCoords + float2(uTime * 0.05, 0));
    //float luminosity = (color.r + color.g + color.b) / 3;
    //color.rgb = luminosity * noise.b * 2;
    //return color * sampleColor;
}

technique Technique1
{
    pass ModdersToolkitShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}