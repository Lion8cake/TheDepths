using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheDepths.Items.Armor;
using TheDepths.Projectiles.Chasme;
using TheDepths.Biomes;
using Terraria.GameContent.Bestiary;
using TheDepths.Worldgen;
using TheDepths.Items;
using TheDepths.Items.Weapons;
using Terraria.GameContent.UI.Elements;
using TheDepths.Projectiles.Summons;

namespace TheDepths.NPCs.Chasme;

[AutoloadBossHead] // For loading "ChasmeHeart_Head_Boss" automatically
public class ChasmeHeart : ModNPC
{
	int[] ChasmePartIDs = new int[4];

	public override void SetStaticDefaults()
	{
		NPCID.Sets.BossBestiaryPriority.Add(Type);

		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;

		NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
		{
			CustomTexturePath = "TheDepths/NPCs/Chasme/ChasmeSoul",
			Position = new Vector2(0f, 30f),
			PortraitPositionYOverride = 0f
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
	}

	public override void SetDefaults()
	{
		NPC.npcSlots = 10f;
		NPC.width = 32;
		NPC.height = 24;
		NPC.aiStyle = -1;
		NPC.defense = 15;//30;
		NPC.lifeMax = 5500;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.knockBackResist = 0f;
		NPC.boss = true;
		NPC.value = 80000;
		NPC.HitSound = SoundID.Item30;
		NPC.DeathSound = SoundID.NPCDeath7;
		NPC.ScaleStats_UseStrengthMultiplier(0.6f); //dont scale like a regular npc in different gamemodes
		if (!Main.dedServ)
		{
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Chasme");
		}
		SpawnModBiomes = new int[1] { ModContent.GetInstance<DepthsBiome>().Type };
	}

	public override void AI()
	{
		//1 head
		//2 body
		//3 hand left
		//4 hand right

		//Head spawning
		if (Main.npc[ChasmePartIDs[1]].type != ModContent.NPCType<ChasmeHead>())
		{
			int head = NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ChasmeHead>());
			(Main.npc[head].ModNPC as ChasmeHead).HeartID = NPC.whoAmI;
			ChasmePartIDs[1] = Main.npc[head].whoAmI;
		}
		//Body Spawning
		if (Main.npc[ChasmePartIDs[2]].type != ModContent.NPCType<ChasmeBody>())
		{
			int body = NPC.NewNPC(new EntitySource_Misc(""), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ChasmeBody>());
			(Main.npc[body].ModNPC as ChasmeBody).HeartID = NPC.whoAmI;
			ChasmePartIDs[2] = Main.npc[body].whoAmI;
		}

		NPC.TargetClosestUpgraded();
		Player player = Main.player[NPC.target];

		//Movement, will change maybe
		Vector2 direction = NPC.DirectionTo(player.Center + new Vector2(1600 * NPC.Center.X > player.Center.X ? 1 : -1, 0));
		direction *= 3f;
		NPC.velocity = (NPC.velocity * (20f - 1) + direction) / 20f;
		if (NPC.velocity == Vector2.Zero)
		{
			NPC.velocity = new Vector2(0.1f, 0.1f);
		}
	}

	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			new FlavorTextBestiaryInfoElement("Mods.TheDepths.Bestiary.ChasmeSoul")
		});
	}

    public override void BossLoot(ref string name, ref int potionType)
    {
		potionType = ItemID.HealingPotion;
    }

	public override bool CheckActive()
	{
		for (int i = 0; 0 < Main.maxPlayers; i++)
		{
			if (Main.player[i].active && (!Main.player[i].dead || !Main.player[i].ghost))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		//Most likely will go unused and removed sometime soon
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

		// Retrieve reference to shader //TODO compile the shader in the effects file that i stole and uncomment/debug this part
		// 
		/*var deathShader = GameShaders.Misc["TheDepths:ChasmeDeath"];
		// Reset back to default value.
        deathShader.UseOpacity(1f);
        // We use npc.ai[3] as a counter since the real death.
        if (NPC.ai[3] > 30f)
        {
            // Our shader uses the Opacity register to drive the effect. See ExampleEffectDeath.fx to see how the Opacity parameter factors into the shader math. 
            deathShader.UseOpacity(1f - (NPC.ai[3] - 30f) / 150f);
        }
		// Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
		//deathShader.Apply();
		*/
		return true;
	}

	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

		Texture2D ChasmeSoul = ModContent.Request<Texture2D>("TheDepths/NPCs/Chasme/ChasmeSoul").Value;

		Color color = new(195, 136, 251);
		Vector2 DrawPos = NPC.Center - screenPos + new Vector2(-27, -50);
		Rectangle Source = new Rectangle(0, 0, ChasmeSoul.Width, ChasmeSoul.Height);
		SpriteEffects fx = (NPC.direction == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

		SpriteEffects effects = SpriteEffects.None;
		Vector2 position1 = DrawPos - new Vector2(-27, -50) + new Vector2(0, -30);


		Texture2D texture2D2 = TextureAssets.Extra[98].Value;
		Vector2 origin2 = texture2D2.Size() / 2f;
		float num9 = (float)((double)Utils.GetLerpValue(15f, 30f, 0/*drawTimer*/, true) * (double)Utils.GetLerpValue(240f, 200f, 0/*drawTimer*/, true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
		Vector2 scale1 = new Vector2(0.5f, 5f) * 2 * num9;
		Vector2 scale2 = new Vector2(0.5f, 2f) * 2 * num9;


		float height = 7;
		//drawTimer++;

		/*if (open)
		{
			if (drawTimer >= 20)
			{
				alpha = (float)(Math.Clamp(0.6375f * Math.Pow((drawTimer - 20), 2), 0, 1));
				height = (float)(-3 * Math.Cos(MathHelper.Pi * (drawTimer - 15) / 45) + 7);
				spriteBatch.Draw(ChasmeSoul, DrawPos - Vector2.UnitY * height, Source, Color.White * alpha, 0, Vector2.Zero, 1, fx, 0f);
			}
			if (drawTimer < 50)
			{
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1 * 0.6f, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2 * 0.6f, effects, 0);
			}


			fadeTimer = 45;
		}
		else
		{
			fadeTimer--;
			if (fadeTimer >= 0)
			{
				num9 = (float)((double)Utils.GetLerpValue(15f, 30f, fadeTimer, true) * (double)Utils.GetLerpValue(240f, 200f, fadeTimer, true) * (1.0 + 0.200000002980232 * Math.Cos((double)Main.GlobalTimeWrappedHourly % 30.0 / 0.5 * 6.28318548202515 * 3.0)) * 0.800000011920929);
				scale1 = new Vector2(0.5f, 5f) * 2 * num9;
				scale2 = new Vector2(0.5f, 2f) * 2 * num9;

				Color color1 = new Color(255, 255, 255) * ((float)fadeTimer / 45);
				color = new Color(195, 136, 251) * ((float)fadeTimer / 45);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 1.570796f, origin2, scale1 * 0.6f, effects, 0);
				spriteBatch.Draw(texture2D2, position1, new Rectangle?(), color, 0.0f, origin2, scale2 * 0.6f, effects, 0);
				spriteBatch.Draw(ChasmeSoul, DrawPos - Vector2.UnitY * height, Source, color1, 0, Vector2.Zero, 1, fx, 0f);
			}

		}*/
	}

	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref TheDepthsWorldGen.downedChasme, -1);

		if (Main.netMode != NetmodeID.MultiplayerClient)
		{
			int CentreX = (int)(NPC.position.X + (12)) / 16;
			int CentreY = (int)(NPC.position.Y + (12)) / 16;
			int HalfLength = 3 + 1;
			for (int k = CentreX - HalfLength; k <= CentreX + HalfLength; k++)
			{
				for (int l = CentreY - HalfLength; l <= CentreY + HalfLength; l++)
				{
					if ((k == CentreX - HalfLength || k == CentreX + HalfLength || l == CentreY - HalfLength || l == CentreY + HalfLength) && !Main.tile[k, l].HasTile)
					{
						Tile tile = Main.tile[k, l];
						Main.tile[k, l].TileType = (ushort)ModContent.TileType<Tiles.ShadowBrick>();
						tile.HasTile = true;
					}
					Main.tile[k, l].LiquidAmount = 0;
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendTileSquare(-1, k, l, 1);
					}
					else
					{
						WorldGen.SquareTileFrame(k, l, true);
					}
				}
			}
		}

		if (!Main.hardMode)
		{
			WorldGen.StartHardmode();
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		//Death animation
		// Slow down
		// Crack small
		// Crack Big
		// Cracks glow white
		// Screen flash
		// Lots of stone and heart gores
		// Drop a pendant
		// Pendant falls after a while, smashing and starting hardmode/spawning the lootbox
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.ChasmeBag>()));

		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.ChasmeTrophy>(), 10));

		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.ChasmeRelic>()));

		npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MidnightHorseshoe>(), 4));

		LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ShadowChasmeMask>(), 7));
		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ChasmeSoulMask>(), 7));

		notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.POWHammer>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<Items.Weapons.ShadeBlade>(), ModContent.ItemType<Items.Weapons.QuartzCannon>(), ModContent.ItemType<Items.Weapons.ShadowClaw>(), ModContent.ItemType<StaffOfAThousandYears>()));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemID.WarriorEmblem, ItemID.RangerEmblem, ItemID.SorcererEmblem, ItemID.SummonerEmblem));

		notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(2, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond, ItemID.Amber, ModContent.ItemType<Items.Placeable.Onyx>()));

		npcLoot.Add(notExpertRule);
	}
}
