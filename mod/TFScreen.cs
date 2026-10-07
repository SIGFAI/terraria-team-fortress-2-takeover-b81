using Terraria;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Zooms the camera in so the mercs read on a stream, and clears the minimap off the action.</summary>
public class TFScreen : ModSystem
{
	public override void PostUpdateEverything()
	{
		Main.GameZoomTarget = 1.5f;
		Main.mapEnabled = false;
	}
}
