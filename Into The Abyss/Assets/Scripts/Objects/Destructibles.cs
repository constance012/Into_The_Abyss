using UnityEngine;

public class Destructibles : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private GameObject hpBottle;

	[Header("Drop Settings"), Space]
	[SerializeField, Range(0f, 1f)] private float healthDropChance;

	public void Break()
	{
		AudioManager.Instance.PlayWithRandomPitch("Crate Break", .7f, 1.2f);

		if(Random.value < healthDropChance)
		{
			GameObject healthPotion = Instantiate(hpBottle, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
			healthPotion.name = hpBottle.name;
		}
		
		Destroy(gameObject);
	}
}
