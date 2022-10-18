using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheDepths.Tiles
{
    public class GemlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;
            TileID.Sets.AvoidedByNPCs[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
			TileID.Sets.FramesOnKillWall[Type] = true;
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            DustType = 7;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("");
            AddMapEntry(new Color(120, 120, 120), name);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
        public static void ShowItemIcon(int tX, int tY, int itemType)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = itemType;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int gemLock = 0;
            int gem = SelectGem((short)frameX);
            switch (frameX / 54)
            {
                case 0:
                    gemLock = Mod.Find<ModItem>("OnyxGemLock").Type;
                    break;
            }
            if (gemLock > 0)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 54, 32, gemLock);
                if (frameY >= 54)
				{
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 54, 32, gem);
				}
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            bool IsOn = Main.tile[i, j].TileFrameY / 54 == 1;
            int gem = SelectGem(Main.tile[i, j].TileFrameX);
            if (gem > 0 && (IsOn || player.HasItem(gem)))
            {
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = gem;
            }
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            bool IsOn = Main.tile[i, j].TileFrameY / 54 == 1;
            int gem = SelectGem(Main.tile[i, j].TileFrameX);
            if (!IsOn && player.selectedItem != 58 && gem > 0 && player.ConsumeItem(gem) || IsOn)
            {
                player.GamepadEnableGrappleCooldown();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Toggle(i, j, !IsOn);
                else
                {
                    ModPacket packet = Mod.GetPacket(255);
                    packet.Write((byte)0);
                    packet.Write((short)i);
                    packet.Write((short)j);
                    packet.Write(!IsOn);
                    packet.Send();
                }
            }
            return true;
        }

        public static void Toggle(int i, int j, bool turnOn)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            Mod mod = ModLoader.GetMod("TheDepths");
            if (!tile.HasTile || tile.TileType != mod.Find<ModTile>("GemlockTile").Type)
                return;

            bool IsOn = tile.TileFrameY / 54 == 1;
            if (IsOn == turnOn)
                return;
            
            int left = i - Main.tile[i, j].TileFrameX % 54 / 18;
            int top = j - Main.tile[i, j].TileFrameY % 54 / 18;

            for (int k = left; k < left + 3; k++)
            {
                for (int l = top; l < top + 3; l++)
                {
                    Main.tile[k, l].TileFrameY = (short)((Main.tile[k, l].TileFrameY + 54) % 108);
                }
            }
            
            if (!turnOn)
            {
                int gem = SelectGem(Main.tile[i, j].TileFrameX);
                if (gem > 0)
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, gem);
            }

            WorldGen.SquareTileFrame(i, j);
            NetMessage.SendTileSquare(-1, left + 1, top + 1, 3);
            TripWire(left, top);
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = mod.GetPacket(256);
                packet.Write(1);
                packet.Write((short)left);
                packet.Write((short)top);
                packet.Send();
            }
        }

        public static void TripWire(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16 + 16, j * 16 + 16));
            Wiring.TripWire(i, j, 3, 3);
        }
        
		public static int SelectGem(short frameX)
		{
			Mod mod = ModLoader.GetMod("TheDepths");
			int gem = 0;
			switch (frameX / 54)
			{
				case 0:
					gem = mod.Find<ModItem>("LargeOnyx").Type;
					break;
			}
			return gem;
		}
    }
}