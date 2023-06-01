using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace TheDepths.Buffs
{
	//Credit to Direwolf for sharing this code (this, the other minecart buff and the minecart mount)
	public sealed class MinecartBuffLoader : ILoadable
	{
		private static IEnumerable<Type> FindTypes(Mod mod)
		{
			return AssemblyManager.GetLoadableTypes(mod.Code)
				.Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(MinecartBuffBase)));
		}

		private static MinecartBuffBase CreateInstance(Type type, bool left)
		{
			return (MinecartBuffBase)Activator.CreateInstance(type, new object[] { left });
		}

		public void Load(Mod mod)
		{
			foreach (var type in FindTypes(mod))
			{
				mod.AddContent(CreateInstance(type, true));
				mod.AddContent(CreateInstance(type, false));
			}
		}

		public void Unload()
		{
		}
	}

	public abstract class MinecartBuffBase : ModBuff
	{
		public bool Left { get; init; }

		public abstract int MountType { get; }

		public MinecartBuffBase(bool left)
		{
			Left = left;
		}

		public string Suffix => Left ? "Left" : "Right";

		public string TypeName => GetType().Name;

		public string LocalizationKey => Mod.GetLocalizationKey($"{LocalizationCategory}.{TypeName}");

		public override string Name => $"{TypeName}_{Suffix}";

		public override string Texture => (GetType().Namespace + "." + TypeName).Replace('.', '/');

		public override LocalizedText DisplayName => Language.GetOrRegister($"{LocalizationKey}.DisplayName");

		public override LocalizedText Description => Language.GetText($"BuffDescription.Minecart{Suffix}");

		public sealed override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;

			BuffID.Sets.BasicMountData[Type] = new BuffID.Sets.BuffMountData()
			{
				mountID = MountType,
				faceLeft = Left
			};
		}
	}
}
