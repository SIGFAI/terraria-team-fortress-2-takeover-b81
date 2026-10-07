using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Sigf.Content;

public class Heavy : MercBase
{
	private int burst;
	protected override float MaxSpeed => 0.7f;
	protected override int ShotDelay => 6;
	protected override float Range => 18f * 16f;
	protected override string[] Barks => new[] { "Cry some more!", "Big gun, small men!", "Sandvich time?" };

	public override void SetDefaults()
	{
		Setup(34, 56, 420, 24, 8, 0.15f, 900);
		NPC.HitSound = SoundID.NPCHit4;
	}

	public override void AI()
	{
		base.AI();
		if (burst > 0) NPC.velocity.X *= 0.85f;        // plants his feet while the burst fires
	}

	protected override void Fire(Player t)
	{
		// 24 bullets per burst, then a pause (shotTimer negative = firing).
		burst++;
		Vector2 from = Muzzle(t, 24f);
		Mix.Shoot(ProjectileID.Bullet, from, Mix.Aim(from, t.Center, 9f).RotatedByRandom(0.1f), 12, 1f, hostile: true);
		SoundEngine.PlaySound(SoundID.Item11, NPC.Center);
		Dust.NewDustPerfect(from, DustID.YellowTorch, new Vector2(NPC.direction * 2f, 0), 0, default, 1.4f).noGravity = true;
		if (burst >= 24) { burst = 0; shotTimer = -120; } else shotTimer = 0;
	}
}
