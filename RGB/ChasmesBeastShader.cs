using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria.GameContent.RGB;

namespace TheDepths.RGB
{
	public class ChasmesBeastShader : ChromaShader
	{
		private readonly Vector4 _shaleColor;

		private readonly Vector4 _boarderColor;

		private readonly Vector4 _altBoarderColor;

		private readonly Vector4[] _gemColors;

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float amount = (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
				Vector4 color = Vector4.Lerp(_shaleColor, _altBoarderColor, amount);
				fragment.SetColor(i, color);
			}
		}

		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//used to get the maximum and minimum positions of the keys, this is used to draw a boarder around the keyboard
			float minX = 255f;
			float minY = 255f;
			float maxX = 0f;
			float maxY = 0f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				if (canvasPositionOfIndex.X < minX)
					minX = canvasPositionOfIndex.X;
				if (canvasPositionOfIndex.X > maxX)
					maxX = canvasPositionOfIndex.X;
				if (canvasPositionOfIndex.Y < minY)
					minY = canvasPositionOfIndex.Y;
				if (canvasPositionOfIndex.Y > maxY)
					maxY = canvasPositionOfIndex.Y;
			}
			//Adjust the boarder to make it feel more like a boarder
			minX += 0.1f;
			minY += 0.1f;
			maxX -= 0.1f;
			maxY -= 0.1f;
			//NOW we adjust the colors lol
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 pixelPosition = fragment.GetCanvasPositionOfIndex(i);
				Point gridPosition = fragment.GetGridPositionOfIndex(i);
				float gemNoise = NoiseHelper.GetDynamicNoise(gridPosition.X, gridPosition.Y, time / 30f);
				gemNoise = Math.Max(0f, 1f - gemNoise * 10f);
				float boarderNoise = NoiseHelper.GetStaticNoise(pixelPosition * new Vector2(0.2f, 0.4f) + new Vector2(time * -0.2f, time * -0.2f));
				Vector4 pixelColor = _shaleColor;
				pixelColor = Vector4.Lerp(pixelColor, _gemColors[((gridPosition.X * 100) + (gridPosition.Y)) % 8] / 1.5f, gemNoise);
				if (pixelPosition.X >= maxX || pixelPosition.Y >= maxY || pixelPosition.Y <= minY || pixelPosition.X <= minX)
				{
					pixelColor = Vector4.Lerp(_boarderColor, _altBoarderColor, boarderNoise * boarderNoise);
				}
				fragment.SetColor(i, pixelColor);
			}
		}

		public ChasmesBeastShader()
		{
			_shaleColor = new Color(13, 12, 20).ToVector4();
			_boarderColor = new Color(55, 49, 77).ToVector4();
			_altBoarderColor = new Color(146, 98, 185).ToVector4();
			_gemColors = [ 
				new Color(255, 0, 255).ToVector4(),
				new Color(255, 255, 0).ToVector4(),
				new Color(0, 0, 255).ToVector4(),
				new Color(0, 255, 0).ToVector4(),
				new Color(255, 0, 0).ToVector4(),
				new Color(255, 255, 255).ToVector4(),
				new Color(255, 128, 0).ToVector4(),
				new Color(255, 255, 255).ToVector4()
			];
		}
	}
}
