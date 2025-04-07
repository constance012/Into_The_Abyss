using UnityEngine;

public class Chest : Interactable
{
	[Header("Chest References"), Space]
	[SerializeField] private Animator animator;

	public override void Interact()
	{
		_isInteracted = !_isInteracted;

		if (_isInteracted)
		{
			animator.Play("Open");
			GameManager.Instance.GameVictory();
		}
		else
		{
			animator.Play("Close");
		}
	}

	protected override void CreatePopupLabel()
	{
		Transform foundLabel = _worldCanvas.transform.Find("Popup Label");

		string itemName = "Chest";
		int quantity = 1;
		Color textColor = Color.white;

		// Create a clone if not already exists.
		if (foundLabel == null)
		{
			base.CreatePopupLabel();
			_popupLabel.SetLabelName(itemName, quantity, textColor);
		}

		// Otherwise, append to the existing one.
		else
		{
			_popupLabel = foundLabel.GetComponent<InteractionPopupLabel>();

			_popupLabel.SetLabelName(itemName, quantity, textColor, true);
			_popupLabel.RestartAnimation();
		}
	}
}