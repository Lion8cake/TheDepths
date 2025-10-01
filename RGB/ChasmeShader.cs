using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria.GameContent.RGB;

namespace TheDepths.RGB
{
	public class ChasmeShader : ChromaShader
	{
		private readonly Vector4 _frontBGColor;

		private readonly Vector4 _backBGColor;

		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public ChasmeShader()
		{
			_frontBGColor = new Color(182, 27, 248).ToVector4();
			_backBGColor = new Color(139, 6, 196).ToVector4();
			_primaryColor = new Color(255, 128, 255).ToVector4();
			_secondaryColor = new Color(158, 95, 245).ToVector4();
		}

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float flameNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.3f + new Vector2(12.5f, time * 0.2f));
				flameNoise = Math.Max(0f, 1f - flameNoise * flameNoise * 4f * flameNoise);
				flameNoise = MathHelper.Clamp(flameNoise, 0f, 1f);
				Vector4 color = Vector4.Lerp(_primaryColor, _secondaryColor, flameNoise);
				fragment.SetColor(i, color);
			}
		}

		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPos = fragment.GetCanvasPositionOfIndex(i);
				float backgroundNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f, time * 1f / 3f);
				Vector4 pixelColor = Vector4.Lerp(_backBGColor, _frontBGColor, backgroundNoise);
				float flameHeightNoise = NoiseHelper.GetDynamicNoise(canvasPos * 0.2f, time * 0.5f);
				float maxHeight = 0.4f;
				maxHeight += flameHeightNoise * 0.4f;
				float yPos = 1.1f - canvasPos.Y;
				if (yPos < maxHeight)
				{
					float flameNoise = NoiseHelper.GetStaticNoise(canvasPos * 0.3f + new Vector2(12.5f, time * 0.2f));
					flameNoise = Math.Max(0f, 1f - flameNoise * flameNoise * 4f * flameNoise);
					flameNoise = MathHelper.Clamp(flameNoise, 0f, 1f);
					Vector4 flameColor = Vector4.Lerp(_primaryColor, _secondaryColor, flameNoise);
					float amount = 1f - MathHelper.Clamp((yPos - maxHeight + 0.2f) / 0.2f, 0f, 1f);
					pixelColor = Vector4.Lerp(pixelColor, flameColor, amount);
				}
				fragment.SetColor(i, pixelColor);
			}
		}
	}
}
