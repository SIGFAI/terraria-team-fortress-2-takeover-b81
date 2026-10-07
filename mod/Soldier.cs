using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Sigf.Content;

public class Soldier : MercBase
{
	protected override float MaxSpeed => 1.1f;
	protected override int ShotDelay => 140;
	protected override float Range => 26f * 16f;
	protected override string[] Barks => new[] { "Rocket time!", "March, maggot!", "Incoming!" };

	public override void SetDefaults() => Setup(24, 50, 220, 22, 4, 0.5f, 400);

	protected override void Fire(Player t)
	{
		Vector2 from = Muzzle(t, 20f);
		Mix.Shoot<Rocket>(from, Mix.Aim(from, t.Center, 7f), 26, 6f, hostile: true);
		Mix.Sound("rocketshot", NPC.Center);
		Mix.Burst(from, DustID.Smoke, 8, 6f, 2f);
	}
}
