using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
	public enum HealingType
	{
		Health,
		Mana
	}

	[Header("Healing amount"), Space]
	public HealingType healingType;
	public int healingAmount;


	public override bool Use(bool forced = false)
	{
		if (quantity > 0 && canBeUsed)
		{
			HealthPoint player = GameObject.FindWithTag("Player").GetComponent<HealthPoint>();

			if (healingType == HealingType.Health)
			{
				AudioManager.Instance.Play("Healing");
				player.ChangeHealth(healingAmount, Vector2.zero);
				quantity--;
				
				return true;
			}
		}
		else
			Debug.LogWarning($"This {displayName} can not be used or its quantity is 0!!");

		return false;
	}

	public override string ToString()
	{
		return base.ToString() + "\n" +
				$"<b> +{healingAmount} HP. </b>\n" +
				$"<b> Right Click to use. </b>";
	}
}
