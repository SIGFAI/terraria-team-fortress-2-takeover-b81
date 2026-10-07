using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class Flamethrower : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 44; Item.height = 18;
		Item.damage = 17; Item.DamageType = DamageClass.Ranged; Item.knockBack = 0.5f;
		Item.useTime = 6; Item.useAnimation = 6; Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true; Item.autoReuse = true;
		Item.UseSound = SoundID.Item34;
		Item.rare = ItemRarityID.Orange; Item.value = Item.buyPrice(gold: 4);
		Item.shoot = ProjectileID.Flames; Item.shootSpeed = 7.5f;
	}

	public override Vector2? HoldoutOffset() => new Vector2(-6f, 0f);

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		position += velocity.SafeNormalize(Vector2.UnitX) * 34f;
		velocity = velocity.RotatedByRandom(0.08f);
	}
}
