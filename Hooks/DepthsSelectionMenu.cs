using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using TheDepths.UI;

namespace TheDepths.Hooks;

internal static class DepthsSelectionMenu {
    private static readonly GroupOptionButton<UnderworldOptions>[] UnderworldButtons =
        new GroupOptionButton<UnderworldOptions>[Enum.GetValues<UnderworldOptions>().Length];

    public static void ILBuildPage(ILContext il) {
        var c = new ILCursor(il);

        // Increase world gen container size
        c.GotoNext(i => i.MatchStloc(0));
        c.Emit(OpCodes.Ldc_I4, 48);
        c.Emit(OpCodes.Add);
    }

    public static void ILMakeInfoMenu(ILContext il) {
        var c = new ILCursor(il);

        // Getting spacing indexes for copying later
        c.GotoNext(i => i.MatchLdstr("evil"));
        c.GotoNext(i => i.MatchLdloc(1));
        int startOfSpacing = c.Index;
        c.GotoNext(i => i.MatchCall(out _));
        int endOfSpacing = c.Index + 1;

        // Navigate to position to add options 
        c.Index = c.Instrs.Count - 1;
        c.GotoPrev(i => i.MatchLdcR4(48));
        c.GotoNext(i => i.MatchCall(out _));
        c.Index++;

        // Adding underworld options
        c.Emit(OpCodes.Ldarg_0); // self
        c.Emit(OpCodes.Ldloc_0); // container
        c.Emit(OpCodes.Ldloc_1); // accumulatedHeight
        c.Emit(OpCodes.Ldloc, 10); // usableWidthPercent
        c.EmitDelegate((UIWorldCreation self, UIElement container, float accumulatedHeight, float usableWidthPercent) =>
            AddUnderworldOptions(self, container, accumulatedHeight, ClickUnderworldOption, "underworld",
                usableWidthPercent));

        // Copying IL for spacing and horizontal bar
        c.Instrs.InsertRange(c.Index, c.Instrs.ToArray()[startOfSpacing..endOfSpacing]);
    }

    public static void OnSetDefaultOptions(On_UIWorldCreation.orig_SetDefaultOptions orig, UIWorldCreation self) {
        orig(self);
        
        ModContent.GetInstance<TheDepthsWorldGen>().SelectedUnderworldOption = UnderworldOptions.Random;
        foreach (GroupOptionButton<UnderworldOptions> underworldButton in UnderworldButtons) {
            underworldButton.SetCurrentOption(UnderworldOptions.Random);
        }
    }

    public static void ILShowOptionDescription(ILContext il) {
        var c = new ILCursor(il);
        
        // Navigate to before final break
        c.Index = c.Instrs.Count - 1;
        c.GotoPrev(i => i.MatchBrfalse(out _));

        // Add description handling logic
        c.Emit(OpCodes.Pop);
        c.Emit(OpCodes.Ldloc_0); // localizedText
        c.Emit(OpCodes.Ldarg_2); // listeningElement
        c.EmitDelegate((LocalizedText localizedText, UIElement listeningElement) => 
            listeningElement is not GroupOptionButton<UnderworldOptions> underworldButton ? localizedText : underworldButton.Description);
        c.Emit(OpCodes.Stloc_0);
        c.Emit(OpCodes.Ldloc_0);
    }

    private static void AddUnderworldOptions(UIWorldCreation self, UIElement container, float accumulatedHeight,
                                             UIElement.MouseEvent clickEvent, string tagGroup,
                                             float usableWidthPercent) {
        LocalizedText[] titles = {
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Random.Title")),
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Underworld.Title")),
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Depths.Title")),
        };
        LocalizedText[] descriptions = {
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Random.Description")),
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Underworld.Description")),
            Language.GetText(TheDepths.mod.GetLocalizationKey("UnderworldSelection.Depths.Description")),
        };
        Color[] colors = {
            Color.White,
            Color.OrangeRed,
            Color.BlueViolet,
        };
        Asset<Texture2D>[] icons = {
            Main.Assets.Request<Texture2D>("Images/UI/WorldCreation/IconEvilRandom"),
            ModContent.Request<Texture2D>("TheDepths/Assets/WorldCreation/IconCoreUnderworld"),
            ModContent.Request<Texture2D>("TheDepths/Assets/WorldCreation/IconCoreDepths"),
        };
        for (int i = 0; i < UnderworldButtons.Length; i++) {
            var groupOptionButton = new global::TheDepths.UI.GroupOptionButton<UnderworldOptions>(
                Enum.GetValues<UnderworldOptions>()[i],
                titles[i],
                descriptions[i],
                colors[i],
                icons[i],
                1f,
                1f,
                16f) {
                Width = StyleDimension.FromPixelsAndPercent(
                    -4 * (UnderworldButtons.Length - 1),
                    1f / UnderworldButtons.Length * usableWidthPercent),
                Left = StyleDimension.FromPercent(1f - usableWidthPercent),
                HAlign = i / (float) (UnderworldButtons.Length - 1),
            };
            groupOptionButton.Top.Set(accumulatedHeight, 0f);
            groupOptionButton.OnLeftMouseDown += clickEvent;
            groupOptionButton.OnMouseOver += self.ShowOptionDescription;
            groupOptionButton.OnMouseOut += self.ClearOptionDescription;
            groupOptionButton.SetSnapPoint(tagGroup, i);
            container.Append(groupOptionButton);
            UnderworldButtons[i] = groupOptionButton;
        }
    }

    private static void ClickUnderworldOption(UIMouseEvent evt, UIElement listeningElement) {
        var groupOptionButton = (GroupOptionButton<UnderworldOptions>)listeningElement;
        ModContent.GetInstance<TheDepthsWorldGen>().SelectedUnderworldOption = groupOptionButton.OptionValue;

        foreach (GroupOptionButton<UnderworldOptions> underworldButton in UnderworldButtons) {
            underworldButton.SetCurrentOption(groupOptionButton.OptionValue);
        }
    }
}