using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using TheDepths.Tiles;

namespace TheDepths
{
    public class TheDepthsModSystem : ModSystem
    {
        public override void OnWorldUnload()
        {
            Gemforge.RubyRelicIsOnForge = 1;
        }
    }
}