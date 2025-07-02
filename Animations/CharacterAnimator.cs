using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void TakeSwing()
    {
        animator.SetTrigger("TakeSwing");
    }
}
