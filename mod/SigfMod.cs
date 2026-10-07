using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Sigf.Content.Items;

namespace Sigf.Content;

public class SigfMod : ModSystem
{
	private static int weapon;

	static int Count(int type) { int n = 0; foreach (NPC x in Main.npc) if (x.active && x.type == type) n++; return n; }

	/// <summary>A merc steps out of a puff of magic-mirror dust a few tiles ahead of the player.</summary>
	static void Deploy<T>(double tiles) where T : ModNPC
	{
		Vector2 at = Mix.Ahead(tiles);
		Mix.Spawn<T>(at);
		Mix.Burst(at + new Vector2(0, -24), DustID.MagicMirror, 18, 16f, 3f);
	}

	static void CycleWeapon()
	{
		weapon = (weapon + 1) % 3;
		if (weapon == 0) Mix.Arm<RocketLauncher>();
		else if (weapon == 1) Mix.Arm<Flamethrower>();
		else Mix.Arm<Minigun>();
		Mix.Popup(Mix.Host.Top, new[] { "Rocket Launcher", "Flamethrower", "Minigun" }[weapon], Color.Orange);
	}

	public override void OnWorldLoad()
	{
		weapon = -1;
		Mix.WanderTiles = 14;
		Mix.Demo(0.5, () => { CycleWeapon(); Mix.Title("TEAM FORTRESS 2", "BLU team has invaded Terraria!", 4); });
		Mix.Demo(1.5, () => { Deploy<Soldier>(9); Deploy<Scout>(14); Deploy<Heavy>(-12); });
		Mix.Demo(8, () => { Deploy<Pyro>(8); Deploy<Sentry>(16); });
		Mix.Demo(9, () => { for (int i = 0; i < 3; i++) Mix.Shoot<Rocket>(Mix.Host.Center + new Vector2(280, -90 - i * 20), new Vector2(-6.5f, 3f), 26, 6f, hostile: true); });

		Mix.Demo(22, () => { if (Count(ModContent.NPCType<Tank>()) == 0) { Mix.Sound("alarm"); Mix.Title("MANN VS MACHINE", "The Tank is rolling in!", 3); var tk = Mix.Spawn<Tank>(Mix.Ahead(28)); Mix.Log("tank " + (tk == null ? "null" : tk.Center.ToString())); } });

		Mix.Every(4, () => {
			if (Mix.Hostiles(Mix.Host.Center, 40).Count >= 7) return;
			switch (Main.rand.Next(5)) {
				case 0: Deploy<Soldier>(Main.rand.Next(-14, 14)); break;
				case 1: Deploy<Scout>(Main.rand.Next(-16, 16)); break;
				case 2: Deploy<Pyro>(Main.rand.Next(-14, 14)); break;
				case 3: Deploy<Heavy>(Main.rand.Next(-14, 14)); break;
				default: Deploy<Scout>(Main.rand.Next(-16, 16)); break;
			}
		});
		Mix.Every(14, () => CycleWeapon());
		Mix.Every(30, () => { if (Count(ModContent.NPCType<Sentry>()) < 2) Deploy<Sentry>(15); });
		Mix.Every(55, () => {
			if (Count(ModContent.NPCType<Tank>()) > 0) return;
			Mix.Sound("alarm"); Mix.Title("MANN VS MACHINE", "The Tank is rolling in!", 3);
			Mix.Spawn<Tank>(Mix.Ahead(30));
		});
	}
}
