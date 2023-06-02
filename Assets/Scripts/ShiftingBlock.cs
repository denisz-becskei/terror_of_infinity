using UnityEngine;

public class ShiftingBlock : MonoBehaviour
{
    private Animator animator;
    private bool state;

    private void Start()
    {
        animator = GetComponent<Animator>();
        state = WorldWideScripts.Chance(35);
        Shift();
    }

    public void Shift()
    {
        if(!state)
        {
            animator.Play("Shift");
            state = true;
        } else
        {
            animator.Play("ReverseShift");
            state = false;
        }
    }
}
