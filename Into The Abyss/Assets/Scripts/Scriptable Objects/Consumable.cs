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
			var healable = GameObject.FindWithTag("Player").GetComponent<IHealable>();
			healable?.Heal(healingAmount);

			AudioManager.Instance.Play("Healing");

			return true;
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
