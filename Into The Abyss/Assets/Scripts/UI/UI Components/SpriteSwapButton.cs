using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapButton : Button
{
	[SerializeField] private RectTransform textTransform;

	private Vector2 _originalTextPosition;
	private bool _isPressedOnce;

	protected override void Start()
	{
		base.Start();

		_originalTextPosition = textTransform.anchoredPosition;
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		base.DoStateTransition(state, instant);

		switch (state)
		{
			case SelectionState.Normal:
			case SelectionState.Highlighted:
			case SelectionState.Disabled:
				textTransform.anchoredPosition = _originalTextPosition;
				_isPressedOnce = false;
				break;

			case SelectionState.Pressed:
				if (!_isPressedOnce)
				{
					textTransform.anchoredPosition -= new Vector2(0, 10);
					_isPressedOnce = true;
				}
				break;
		}
	}
}
