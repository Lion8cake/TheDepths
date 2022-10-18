using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;

namespace TheDepths
{
    public class LargeGem : PlayerDrawLayer
    {


        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.CaptureTheGem);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = TheDepths.mod;
            TheDepthsPlayer depthPlayer = drawPlayer.GetModPlayer<TheDepthsPlayer>();
            if (depthPlayer.hasLargeGems == 0)
                return;
            BitsByte ownedLargeGems = depthPlayer.ownedLargeGems;
            BitsByte hasLargeGems = depthPlayer.hasLargeGems;
            int num1 = 0;
            for (int key = 0; key < 8; ++key)
            {
                if (ownedLargeGems[key])
                    ++num1;
                if (hasLargeGems[key])
                    ++num1;
            }
            float num2 = (float)(1.0 - num1 * 0.0450000017881393);
            float num3 = (float)((num1 - 1.0) * 4.0);
            switch (num1)
            {
                case 2:
                    num3 += 10f;
                    break;
                case 3:
                    num3 += 8f;
                    break;
                case 4:
                case 5:
                    num3 += 6f;
                    break;
                case 6:
                    num3 += 2f;
                    break;
                case 7:
                    num3 += 0.0f;
                    break;
                case 8:
                    num3 -= 2f;
                    break;
                case 9:
                    num3 -= 4f;
                    break;
                case 10:
                    num3 -= 6f;
                    break;
                case 11:
                    num3 -= 8f;
                    break;
            }
            float num4 = (float)(drawPlayer.miscCounter / 300.0 * 6.28318548202515);
            if (num1 <= 0)
                return;
            float num5 = 6.283185f / num1;
            int num6 = 0;
            List<DrawData> drawDataList = new List<DrawData>();
            string[] strArray = new string[1]
            {
                    "TheDepths/Items/LargeOnyx_Glow",
            };
            for (int key = 0; key < 16; ++key)
            {
                if (key < 8 && !ownedLargeGems[key] || key >= 8 && !hasLargeGems[key - 8])
                {
                    ++num6;
                }
                else
                {
                    Vector2 rotationVector2 = Utils.ToRotationVector2(num4 + num5 * (key - num6));
                    Vector2 vector2 = Vector2.Add(Utils.Floor(Vector2.Add(Vector2.Subtract(drawInfo.Position, Main.screenPosition), new Vector2(drawPlayer.width / 2, drawPlayer.height - 80))), Vector2.Multiply(rotationVector2, num3));
                    Texture2D texture2D = key >= 8 ? ModContent.Request<Texture2D>(strArray[key - 8]).Value : TextureAssets.Gem[key].Value;
                    drawDataList.Add(new DrawData(texture2D, vector2, new Rectangle?(), new Color(250, 250, 250, Main.mouseTextColor / 2), 0.0f, Vector2.Divide(Utils.Size(texture2D), 2f), (float)(Main.mouseTextColor / 1000.0 + 0.800000011920929) * num2, 0, 0));
                }
            }
            drawInfo.DrawDataCache.AddRange(drawDataList);
        }
    }
}