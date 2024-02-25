// Copyright 2023-2024 BasicallyIAmFox
// 
//    Licensed under the Apache License, Version 2.0 (the "License")
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace TheDepths;

public sealed class DefaultRenderTargetOverrider {
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly struct DefaultOverride : IDisposable {
		public DefaultOverride(RenderTarget2D value) {
			_overrideDefaultValue = value;
		}

		public void Dispose() {
			_overrideDefaultValue = null;
		}
	}
	
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly struct SpecificOverride : IDisposable {
		private readonly RenderTarget2D value;
		
		public SpecificOverride(RenderTarget2D value, RenderTarget2D? valueToOverride) {
			this.value = value;
			
			_overrideSpecificValues.Add(value, valueToOverride);
		}
		
		public void Dispose() {
			_overrideSpecificValues.Remove(value);
		}
	}

	private static RenderTarget2D? _overrideDefaultValue;
	private static readonly Dictionary<RenderTarget2D, RenderTarget2D?> _overrideSpecificValues = new();

	public static DefaultOverride OverrideDefault(RenderTarget2D value) {
		return new DefaultOverride(value);
	}
	
	public static SpecificOverride OverrideSpecific(RenderTarget2D value, RenderTarget2D? valueToOverride) {
		return new SpecificOverride(value, valueToOverride);
	}

	internal static void Patch() {
		Type[] type = new Type[] { typeof(RenderTarget2D) };
		var setRenderTargetsMethodInfo = typeof(GraphicsDevice).GetMethod(nameof(GraphicsDevice.SetRenderTarget), BindingFlags.Instance | BindingFlags.Public, type);
		
		Debug.Assert(setRenderTargetsMethodInfo is not null);
		
		MonoModHooks.Add(setRenderTargetsMethodInfo, (Action<GraphicsDevice, RenderTarget2D?> orig, GraphicsDevice self, RenderTarget2D? renderTarget) => {
			if (renderTarget != null && _overrideSpecificValues.TryGetValue(renderTarget, out var newRenderTarget))
			{
				renderTarget = newRenderTarget;
			}
			
			// renderTarget is null whenever it tries to set to 'draw to screen' thing.
			renderTarget ??= _overrideDefaultValue;
			
			orig(self, renderTarget);
		});
	}
}
