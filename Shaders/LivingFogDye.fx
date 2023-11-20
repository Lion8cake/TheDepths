sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float2 uTargetPosition;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 BaseColor = tex2D(uImage0, coords);
    float2 FogCoords = (coords * uImageSize0 - uSourceRect.xy) / uImageSize1;
    float4 Fog = tex2D(uImage1, FogCoords + float2(uTime * 0.05, uTime * 0.05));
    float PixelAlpha = (BaseColor.r + BaseColor.g + BaseColor.b) / 3;
    BaseColor.rgb = PixelAlpha * Fog.rgb * 2;
    return BaseColor * sampleColor * BaseColor.a;
}

technique Technique1
{
    pass LivingFogDyeShaderPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}