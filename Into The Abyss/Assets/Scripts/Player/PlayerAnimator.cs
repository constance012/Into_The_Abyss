using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Animator animator;

	private Dictionary<PlayerAnimatorParameters, int> _parameterHashMap = new Dictionary<PlayerAnimatorParameters, int>()
	{
		[PlayerAnimatorParameters.VelocityX] = Animator.StringToHash("VelocityX"),
		[PlayerAnimatorParameters.VelocityY] = Animator.StringToHash("VelocityY"),
		[PlayerAnimatorParameters.IsDigging] = Animator.StringToHash("IsDigging"),
		[PlayerAnimatorParameters.IsGrounded] = Animator.StringToHash("IsGrounded"),
		[PlayerAnimatorParameters.WasGrounded] = Animator.StringToHash("WasGrounded")
	};

	public void SetFloat(PlayerAnimatorParameters parameter, float value)
	{
		animator.SetFloat(_parameterHashMap[parameter], value);
	}

	public void SetBool(PlayerAnimatorParameters parameter, bool value)
	{
		animator.SetBool(_parameterHashMap[parameter], value);
	}

	public void SetInteger(PlayerAnimatorParameters parameter, int value)
	{
		animator.SetInteger(_parameterHashMap[parameter], value);
	}
	
	public void SetTrigger(PlayerAnimatorParameters parameter)
	{
		animator.SetTrigger(_parameterHashMap[parameter]);
	}
}

public enum PlayerAnimatorParameters
{
	VelocityX,
	VelocityY,
	IsDigging,
	IsGrounded,
	WasGrounded
}