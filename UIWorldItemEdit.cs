using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.UI;

namespace TheDepths
{
    public class UIWorldItemEdit
    {
        public UIWorldItemEdit(WorldFileData data, int orderInList, bool canBePlayed)
        {
            UIElement WorldIcon = (UIElement)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(data);
            WorldFileData Data = (WorldFileData)typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(data);

            if (Data.DefeatedMoonlord)
            {
                UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/Item_100"))
                {
                    HAlign = 0.5f,
                    VAlign = 0.5f,
                    Top = new StyleDimension(-10f, 0f),
                    Left = new StyleDimension(-3f, 0f),
                    IgnoresMouseInteraction = true
                };
                WorldIcon.Append(element);
            }
        }
    }
}