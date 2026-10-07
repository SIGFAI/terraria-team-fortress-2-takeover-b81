using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Sigf.Content;

public class Pyro : MercBase
{
	protected override float MaxSpeed => 1.5f;
	protected override int ShotDelay => 7;
	protected override float Range => 9f * 16f;
	protected override string[] Barks => new[] { "Mmmph mmph!", "Mmf mmmph!", "Hmmph!" };

	public override void SetDefaults()
	{
		Setup(22, 48, 200, 20, 2, 0.4f, 500);
		NPC.buffImmune[BuffID.OnFire] = true; NPC.buffImmune[BuffID.OnFire3] = true;
	}

	protected override void Fire(Player t)
	{
		Vector2 from = Muzzle(t, 24f);
		Mix.Shoot(ProjectileID.Flames, from, Mix.Aim(from, t.Center, 7f).RotatedByRandom(0.1f), 14, 0.5f, hostile: true);
		if (Main.rand.NextBool(3)) SoundEngine.PlaySound(SoundID.Item34, NPC.Center);
	}
}
