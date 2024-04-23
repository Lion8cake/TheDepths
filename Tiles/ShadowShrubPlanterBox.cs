using TheDepths.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System.Collections.Generic;
using Terraria.GameContent;
using System;
using Terraria.GameContent.Personalities;
using System.Reflection;
using Terraria.Map;
using Terraria.GameContent.Skies;
using Microsoft.Xna.Framework.Graphics;

namespace TheDepths.Tiles
{
	public class ShadowShrubPlanterBox : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			AddMapEntry(new Color(191, 142, 111));
			TileID.Sets.DisableSmartCursor[Type] = true;
			//TileID.Sets.PlanterBoxes[Type] = true; //for the future
			AdjTiles = new int[] { TileID.PlanterBox };
		}

		public override bool Slope(int i, int j)
		{
			return false;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile left = Main.tile[i - 1, j];
			Tile right = Main.tile[i + 1, j];
			int x = i - Main.tile[i, j].TileFrameX / 18;
			int y = j - Main.tile[i, j].TileFrameY / 18;
			if (WorldGen.InWorld(x, y))
			{
				if (left.TileType == Type && right.TileType == Type)
				{
					Main.tile[i, j].TileFrameX = 18;
				}
				else if (left.TileType == Type && right.TileType != Type)
				{
					Main.tile[i, j].TileFrameX = 36;
				}
				else if (left.TileType != Type && right.TileType == Type)
				{
					Main.tile[i, j].TileFrameX = 0;
				}
				else
				{
					Main.tile[i, j].TileFrameX = 54;
				}
			}
			return false;
		}

		public override void Load()
		{
			On_SmartCursorHelper.Step_PlanterBox += smartCursor;
		}

		public override void Unload()
		{
			On_SmartCursorHelper.Step_PlanterBox -= smartCursor;
		}

		private void smartCursor(On_SmartCursorHelper.orig_Step_PlanterBox orig, object providedInfo, ref int focusedX, ref int focusedY)
		{
			var SmartCursorUsageInfo = typeof(SmartCursorHelper).GetNestedType("SmartCursorUsageInfo", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			Item item = (Item)SmartCursorUsageInfo.GetField("item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetX = (int)SmartCursorUsageInfo.GetField("screenTargetX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int screenTargetY = (int)SmartCursorUsageInfo.GetField("screenTargetY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartX = (int)SmartCursorUsageInfo.GetField("reachableStartX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndX = (int)SmartCursorUsageInfo.GetField("reachableEndX", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableStartY = (int)SmartCursorUsageInfo.GetField("reachableStartY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			int reachableEndY = (int)SmartCursorUsageInfo.GetField("reachableEndY", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			Vector2 mouse = (Vector2)SmartCursorUsageInfo.GetField("mouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).GetValue(providedInfo);
			List<Tuple<int, int>> _targets = (List<Tuple<int, int>>)typeof(SmartCursorHelper).GetField("_targets", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).GetValue(null);
			if (item.createTile == TileID.PlanterBox && item.createTile != Type)
			{
				orig.Invoke(providedInfo, ref focusedX, ref focusedY);
			}
			if (item.createTile != Type || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[screenTargetX, screenTargetY].HasTile && (Main.tile[screenTargetX, screenTargetY].TileType == Type || Main.tile[screenTargetX, screenTargetY].TileType == TileID.PlanterBox))
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = reachableStartX; i <= reachableEndX; i++)
				{
					for (int j = reachableStartY; j <= reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.HasTile && tile.TileType == Type)
						{
							if (!Main.tile[i - 1, j].HasTile || Main.tileCut[Main.tile[i - 1, j].TileType] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].TileType])
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].HasTile || Main.tileCut[Main.tile[i + 1, j].TileType] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].TileType])
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY) && num != -1f)
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}
	}
}