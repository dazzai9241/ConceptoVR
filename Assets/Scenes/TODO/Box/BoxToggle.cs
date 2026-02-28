using UnityEngine;

public class BoxToggle : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBox()
    {
        bool current = animator.GetBool("IsOpen");
        animator.SetBool("IsOpen", !current);
    }
}