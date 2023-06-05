using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButtonController : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("We go back!");
    }

    public void Animate()
    {
        animator.Play("ButtonFade");
    }
}
