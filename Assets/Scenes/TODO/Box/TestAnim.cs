using UnityEngine;

public class TestAnim : MonoBehaviour
{
    public Animator animator;
    public Animation animation;

    public void TestAnimator()
    {
        animator.SetTrigger("Open");
    }
}
