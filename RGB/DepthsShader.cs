using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;
using Terraria.GameContent.RGB;

namespace TheDepths.RGB
{
	public class DepthsShader : ChromaShader
	{
		private readonly Vector4 _backgroundColor;

		private readonly Vector4 _fogColor;

		private readonly Vector4 _waterColor;

		private readonly Vector4 _quicksilverColor; //unused, was going to replace half of all water ripples

		[RgbProcessor(EffectDetailLevel.Low)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float amount = (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
				Vector4 color = Vector4.Lerp(_backgroundColor, _waterColor, amount);
				fragment.SetColor(i, color);
			}
		}

		[RgbProcessor(EffectDetailLevel.High)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//Iterate over all the pixels provided on the keyboard
			for (int j = 0; j < fragment.Count; j++)
			{
				Vector4 color = _backgroundColor; //Background Color
				Vector2 pixelPos = fragment.GetCanvasPositionOfIndex(j); //position of the LED on the keyboard

				//Get the static noise of the keyboard moving in the bottom-right direction at a slow speed
				float boarderNoise = NoiseHelper.GetStaticNoise(pixelPos * new Vector2(0.2f, 0.4f) + new Vector2(time * -0.05f, time * -0.05f));
				color = Vector4.Lerp(color, _fogColor, boarderNoise * boarderNoise); //change the color to be a slightly lighter based on how strong the noise is at the position
				//This is used to add a fog effect to the back of the keyboard

				int rippleCount = 2; //ripple count
				for (int i = 0; i < rippleCount; i++)
				{
					//time in float, but for each ripple, this is offset so no 2 ripples fade and splash at the same time
					float timeOff = time + ((1f / rippleCount) * i);

					//Get the time between 2 seconds, by getting the remainder when dividing the offset by 1
					float timeBetweenSeconds = timeOff % 1f;

					//Get a random position on the keyboard to draw the ripple at
					Vector2 randomPos = Random(timeOff, i, fragment.CanvasBottomRight);
					//Get strength of the ripple's color based on position away from the center of the ripple
					float ripStrength = (pixelPos - randomPos).Length() - timeBetweenSeconds;

					float alphaStrength = 
						(float)Math.Min(MathHelper.SmoothStep(0, 3, ripStrength), (1.0f - MathHelper.SmoothStep(0, 3, ripStrength - 0.1f))) //get the definitive position of the ripple and whether the pixel is in range
						* ((1f - timeOff % 1f)); //the fade out so the ripple doesn't appear to just vanish

					//check if the pixel as more than just 0 alpha
					if (alphaStrength > 0)
					{
						//modify the color usuing the ripple color and it's strength
						color = Vector4.Lerp(color, _waterColor * 1.5f, alphaStrength);
					}
				}
				//Set the pixel's color
				fragment.SetColor(j, color);
			}
		}

		//Honestly kinda wish there was a better solution
		//(...and honestly, I hope there is a better solution out there)
		//Here we get a "random" position for the ripples
		//Since true randomness is inachieveable with chroma shaders, we have to create a fake randomness using multiplication
		//Every second, the value changes, and has a chance to put a ripple outside to give even more variance to the ripples.
		private Vector2 Random(float p, int i, Vector2 posLimit)
		{
			int time = (int)((p * 100f) / 100f);
			float index = i + 1f;
			Vector2 newPos = new Vector2((time * index) * 12.9898f, (time * index) * 78.233f);
			if ((int)(newPos.X * 3.5f) % 2 == 0) //Occassionally put one out of bounds for more randomness
			{
				return new Vector2(-255f, -255f);
			}
			newPos.X %= posLimit.X;
			newPos.Y %= posLimit.Y;
			return newPos;
		}

		public DepthsShader()
		{
			_backgroundColor = Color.Black.ToVector4();
			_fogColor = new Color(25, 25, 25).ToVector4();
			_waterColor = new Color(24, 59, 152).ToVector4();
			_quicksilverColor = new Color(123, 142, 148).ToVector4();
		}
	}
}
