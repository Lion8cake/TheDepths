using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria;

namespace TheDepths.RGB
{
	public class QuicksilverIndicatiorShader : ChromaShader
	{
		private readonly Vector4 _quicksilverColor;

		private readonly Vector4 _quicksilverShineColor;

		private readonly Vector4 _backgroundColor;

		public QuicksilverIndicatiorShader()
		{
			_backgroundColor = Color.Black.ToVector4();
			_quicksilverColor = new Color(122, 140, 146).ToVector4();
			_quicksilverShineColor = new Color(255, 253, 246).ToVector4();
		}

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				int AmuletsActive = Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().GetActiveAmulets();
				float quicksilverPercent = 1f - (float)(Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().QuicksilverTimer + ((60 * 4 * AmuletsActive) - Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().AmuletTimer)) / ((Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().stoneRose ? 60 * 4 : 60 * 2) + (60 * 4 * AmuletsActive));
				Vector4 color = Vector4.Lerp(_backgroundColor, _quicksilverColor, quicksilverPercent);
				fragment.SetColor(i, color);
			}
		}

		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int j = 0; j < fragment.Count; j++)
			{
				Vector2 canvasPos = fragment.GetCanvasPositionOfIndex(j);

				Vector4 pixelColor = _backgroundColor;
				int AmuletsActive = Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().GetActiveAmulets();
				float quicksilverPercent = 1f - (float)(Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().QuicksilverTimer + ((60 * 4 * AmuletsActive) - Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().AmuletTimer)) / ((Main.LocalPlayer.GetModPlayer<TheDepthsPlayer>().stoneRose ? 60 * 4 : 60 * 2) + (60 * 4 * AmuletsActive));

				float waveHeight = QuicksilverWave(fragment.CanvasBottomRight.Y, quicksilverPercent, canvasPos.X, time, 0);
				
				float quicksilverAmount = (canvasPos.Y - waveHeight) * 2;

				if (canvasPos.Y > waveHeight && quicksilverAmount > 0)
				{
					pixelColor = Vector4.Lerp(pixelColor, _quicksilverColor * 0.8f, quicksilverAmount);
				}

				float shineWaveTop = QuicksilverWave(fragment.CanvasBottomRight.Y, quicksilverPercent, canvasPos.X, time, 1);
				float shineWaveBottom = QuicksilverWave(fragment.CanvasBottomRight.Y, quicksilverPercent, canvasPos.X, time, 2);
				float shineAmount = (canvasPos.Y - shineWaveTop) * 2f;
				if (canvasPos.Y > shineWaveTop && canvasPos.Y < shineWaveBottom && shineAmount > 0)
				{
					pixelColor = Vector4.Lerp(pixelColor, _quicksilverShineColor * 1.5f, shineAmount);
				}

				fragment.SetColor(j, pixelColor);
			}
		}

		private static float QuicksilverWave(float maxY, float liquidPerc, float x, float time, float yOff)
		{
			float waveHeight = MathHelper.Lerp(0, maxY, liquidPerc) + ((maxY * 0.25f) * (yOff - 1));
			waveHeight -= (float)Math.Sin(((x / 2) + time) * Math.PI) * 0.1f;
			waveHeight += (float)Math.Sin((x / 2) - time) * 0.1f;
			return waveHeight;
		}
	}
}
