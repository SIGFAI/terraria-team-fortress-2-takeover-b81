using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Sigf.Content;

public class Scout : MercBase
{
	protected override float MaxSpeed => 3.6f;
	protected override string[] Barks => new[] { "Too fast!", "Boston, baby!", "Run run run!" };

	public override void SetDefaults() => Setup(22, 46, 130, 18, 0, 0.8f, 300);

	public override void AI()
	{
		base.AI();
		// Fast runner: keep accelerating toward the player, hop over small steps.
		if (NPC.velocity.Y == 0f) NPC.velocity.X += NPC.direction * 0.12f;
		if (Main.rand.NextBool(3) && Math.Abs(NPC.velocity.X) > 2f)
			Dust.NewDustPerfect(NPC.Bottom + new Vector2(0, -2), DustID.Smoke, new Vector2(-NPC.direction * 0.8f, -0.3f), 120, default, 0.8f).noGravity = true;
	}
}
