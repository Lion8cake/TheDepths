using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Dusts;
using TheDepths.Gores;
using TheDepths.Worldgen;

namespace TheDepths.ModSupport.BiomeLava
{
	public class QuicksilverStyles : ModSystem
	{
		public override void Load()
		{
			if (DepthsModCalling.BiomeLavaMod != null)
			{
				QuicksilverStyle();
				CorruptQuicksilverStyle();
				CrimsonQuicksilverStyle();
				HallowQuicksilverStyle();
				JungleQuicksilverStyle();
				IceQuicksilverStyle();
				DesertQuicksilverStyle();
			}
		}

		// The base quicksilver style
		public static void QuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle0";

			string Texture = mod.Name + "/Assets/Lava/Quicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;
			
			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<QuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<QuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer);
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void CorruptQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle1";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Corruption/CorruptionQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<CorruptQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<CorruptQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneCorrupt;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void CrimsonQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle2";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Crimson/CrimsonQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<CrimsonQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<CrimsonQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneCrimson;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void HallowQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle3";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Hallow/HallowQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<HallowQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<HallowQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneHallow;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void JungleQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle4";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Jungle/JungleQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<JungleQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<JungleQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneJungle;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void IceQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle5";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Ice/IceQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<IceQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<IceQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneSnow;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}

		public static void DesertQuicksilverStyle()
		{
			Mod mod = ModContent.GetInstance<TheDepths>();

			string Name = "QuicksilverStyle6";

			string Texture = mod.Name + "/ModSupport/BiomeLava/Assets/Desert/DesertQuicksilver";

			string BlockTexture = Texture + "_Block";

			string SlopeTexture = Texture + "_Slope";

			string WaterfallTexture = Texture + "_Silverfall";

			Func<int, int, float, float, float, Vector3> ModifyLightDelegate = ModifyLight;

			Func<int> GetSplashDustDelegate = GetSplashDust;

			Func<int> GetDropletGoreDelegate = GetDropletGore;

			Func<bool> IsLavaActiveDelegate = IsLavaActive;

			Func<bool> lavafallGlowmaskDelegate = lavafallGlowmask;

			Func<Player, NPC, int, Action> InflictDebuffDelegate = InflictDebuff;

			Func<bool> InflictsOnFireDelegate = InflictsOnFire;

			DepthsModCalling.BiomeLavaMod.Call("ModLavaStyle", mod, Name, Texture, BlockTexture, SlopeTexture, WaterfallTexture, GetSplashDustDelegate, GetDropletGoreDelegate, ModifyLightDelegate, IsLavaActiveDelegate, lavafallGlowmaskDelegate, InflictDebuffDelegate, InflictsOnFireDelegate);

			static Vector3 ModifyLight(int i, int j, float r, float g, float b)
			{
				return new Vector3(0f, 0f, 0f);
			}

			static int GetSplashDust()
			{
				return ModContent.DustType<DesertQuicksilverBubble>();
			}

			static int GetDropletGore()
			{
				return ModContent.GoreType<DesertQuicksilverDroplet>();
			}

			static bool IsLavaActive()
			{
				return TheDepthsWorldGen.InDepths(Main.LocalPlayer) && !Main.LocalPlayer.ZoneUnderworldHeight && Main.LocalPlayer.ZoneDesert;
			}

			static bool lavafallGlowmask()
			{
				return false;
			}

			static Action InflictDebuff(Player player, NPC npc, int onfireDuration)
			{
				return null; //The Depths does its own buff affliction code, here as we need to use the Inflicts On Fire overload
			}

			static bool InflictsOnFire()
			{
				return false;
			}
		}
	}

	public abstract class QuicksilverBubbles : ModDust
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return DepthsModCalling.BiomeLavaMod != null;
		}

		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.1f;
			dust.velocity.Y = -0.5f;
		}

		public override bool Update(Dust dust)
		{
			Dust.lavaBubbles++;
			if (dust.velocity.Y > 0f)
				dust.velocity.Y -= 0.2f;

			if (dust.noGravity)
			{
				var noGravityLightStrength = dust.scale * 0.6f;
				if (noGravityLightStrength > 1f)
					noGravityLightStrength = 1f;
			}

			var lightStrength = dust.scale * 0.3f + 0.4f;
			if (lightStrength > 1f)
				lightStrength = 1f;

			return true;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			var lightStrength = (255 - dust.alpha) / 255f;
			lightStrength += 3;
			lightStrength /= 4f;

			var r = (int)(lightColor.R * lightStrength);
			var g = (int)(lightColor.G * lightStrength);
			var b = (int)(lightColor.B * lightStrength);
			var a = Math.Clamp(lightColor.A - dust.alpha, 0, 255);

			return new Color(r, g, b, a);
		}
	}

	public abstract class QuicksilverDroplets : ModGore
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return DepthsModCalling.BiomeLavaMod != null;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.numFrames = 15;
			gore.behindTiles = true;
			gore.timeLeft = Gore.goreTime * 3;
		}

		public override bool Update(Gore gore)
		{
			gore.alpha = gore.position.Y < (Main.worldSurface * 16.0) + 8.0
				? 0
				: 100;

			int frameDuration = 4;
			gore.frameCounter += 1;
			if (gore.frame <= 4)
			{
				int tileX = (int)(gore.position.X / 16f);
				int tileY = (int)(gore.position.Y / 16f) - 1;
				if (WorldGen.InWorld(tileX, tileY) && !Main.tile[tileX, tileY].HasTile)
				{
					gore.active = false;
				}

				if (gore.frame == 0 || gore.frame == 1 || gore.frame == 2)
				{
					frameDuration = 24 + Main.rand.Next(256);
				}

				if (gore.frame == 3)
				{
					frameDuration = 24 + Main.rand.Next(96);
				}

				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
					if (gore.frame == 5)
					{
						int droplet = Gore.NewGore(new EntitySource_Misc(nameof(QuicksilverDroplet)), gore.position, gore.velocity, gore.type);
						Main.gore[droplet].frame = 9;
						Main.gore[droplet].velocity *= 0f;
					}
				}
			}
			else if (gore.frame <= 6)
			{
				frameDuration = 8;
				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
					if (gore.frame == 7)
					{
						gore.active = false;
					}
				}
			}
			else if (gore.frame <= 9)
			{
				frameDuration = 6;
				gore.velocity.Y += 0.2f;
				if (gore.velocity.Y < 0.5f)
				{
					gore.velocity.Y = 0.5f;
				}

				if (gore.velocity.Y > 12f)
				{
					gore.velocity.Y = 12f;
				}

				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}

				if (gore.frame > 9)
				{
					gore.frame = 7;
				}
			}
			else
			{
				gore.velocity.Y += 0.1f;
				if (gore.frameCounter >= frameDuration)
				{
					gore.frameCounter = 0;
					gore.frame += 1;
				}

				gore.velocity *= 0f;
				if (gore.frame > 14)
				{
					gore.active = false;
				}
			}

			Vector2 oldVelocity = gore.velocity;
			gore.velocity = Collision.TileCollision(gore.position, gore.velocity, 16, 14);
			if (gore.velocity != oldVelocity)
			{
				if (gore.frame < 10)
				{
					gore.frame = 10;
					gore.frameCounter = 0;
					SoundEngine.PlaySound(SoundID.Drip, gore.position + new Vector2(8, 8));
				}
			}
			else if (Collision.WetCollision(gore.position + gore.velocity, 16, 14))
			{
				if (gore.frame < 10)
				{
					gore.frame = 10;
					gore.frameCounter = 0;
					SoundEngine.PlaySound(SoundID.Drip, gore.position + new Vector2(8, 8));
				}

				int tileX = (int)(gore.position.X + 8f) / 16;
				int tileY = (int)(gore.position.Y + 14f) / 16;
				if (Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].LiquidAmount > 0)
				{
					gore.velocity *= 0f;
					gore.position.Y = (tileY * 16) - (Main.tile[tileX, tileY].LiquidAmount / 16);
				}
			}

			gore.position += gore.velocity;
			return false;
		}
	}

	#region LavaBubblesandDrips
	public class CorruptQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Corruption/CorruptionQuicksilverBubble";
	}

	public class CorruptQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Corruption/CorruptionQuicksilverDrip";
	}

	public class CrimsonQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Crimson/CrimsonQuicksilverBubble";
	}

	public class CrimsonQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Crimson/CrimsonQuicksilverDrip";
	}

	public class HallowQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Hallow/HallowQuicksilverBubble";
	}

	public class HallowQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Hallow/HallowQuicksilverDrip";
	}

	public class JungleQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Jungle/JungleQuicksilverBubble";
	}

	public class JungleQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Jungle/JungleQuicksilverDrip";
	}

	public class IceQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Ice/IceQuicksilverBubble";
	}

	public class IceQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Ice/IceQuicksilverDrip";
	}

	public class DesertQuicksilverBubble : QuicksilverBubbles
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Desert/DesertQuicksilverBubble";
	}

	public class DesertQuicksilverDroplet : QuicksilverDroplets
	{
		public override string Texture => "TheDepths/ModSupport/BiomeLava/Assets/Desert/DesertQuicksilverDrip";
	}
	#endregion
}