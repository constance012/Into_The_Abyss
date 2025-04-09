using System;
using UnityEngine;

public class SquirrelMovement : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Transform firstPoint;
    [SerializeField] private Transform secondPoint;
	[SerializeField] private Animator animator;

    [Header("Attributes"), Space]
    [SerializeField] private float moveSpeed;
	[SerializeField] private Vector2 idleTimer;

    private Vector3 _target;
	private float _idleTimer;
	private float _originalScaleX;
	private int _isRunningHash;

    void Start()
    {
        _target = firstPoint.position;
		_originalScaleX = transform.localScale.x;
		_isRunningHash = Animator.StringToHash("IsRunning");
    }

    // Update is called once per frame
    void Update()
    {
		if (_idleTimer > 0)
		{
			_idleTimer -= Time.deltaTime;
			animator.SetBool(_isRunningHash, false);
		}
		else
		{
			animator.SetBool(_isRunningHash, true);

			if(Vector2.Distance(_target, transform.position) < 0.1f)
			{
				_idleTimer = idleTimer.RandomBetweenEnds();
				if(_target == firstPoint.position)
				{
					_target = secondPoint.position;
				}
				else
				{
					_target = firstPoint.position;
				}
			}

			int sign = transform.position.x - _target.x > 0 ? 1 : -1;
			transform.localScale = new Vector3(_originalScaleX * sign, transform.localScale.y, transform.localScale.z);

			transform.position = Vector2.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
		}
    }
}
