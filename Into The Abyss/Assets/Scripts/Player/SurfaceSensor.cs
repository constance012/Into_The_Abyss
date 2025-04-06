using UnityEngine;

public class SurfaceSensor : MonoBehaviour
{
    public bool grounded { get; private set; }

    public Collider2D colCheck;

    public LayerMask groundLayer;

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        grounded = Physics2D.OverlapAreaAll(colCheck.bounds.min, colCheck.bounds.max, groundLayer).Length > 0;
    }
}
