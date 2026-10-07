using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class RocketLauncher : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 44; Item.height = 18;
		Item.damage = 58; Item.DamageType = DamageClass.Ranged; Item.knockBack = 8f;
		Item.useTime = 26; Item.useAnimation = 26; Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true; Item.autoReuse = true;
		Item.UseSound = Mix.Style("rocketshot");
		Item.rare = ItemRarityID.Orange; Item.value = Item.buyPrice(gold: 5);
		Item.shoot = ModContent.ProjectileType<Rocket>(); Item.shootSpeed = 9f;
	}

	public override Vector2? HoldoutOffset() => new Vector2(-6f, 0f);

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		position += velocity.SafeNormalize(Vector2.UnitX) * 26f;
		for (int i = 0; i < 8; i++) {
			Dust d = Dust.NewDustPerfect(position, DustID.Smoke, velocity.RotatedByRandom(0.5f) * Main.rand.NextFloat(0.1f, 0.4f), 100, default, 1.6f);
			d.noGravity = true;
		}
		for (int i = 0; i < 6; i++)
			Dust.NewDustPerfect(position, DustID.Torch, velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.2f, 0.7f), 0, default, 2f).noGravity = true;
	}
}
