using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator player1Animator;
    public Animator player2Animator;

    public IEnumerator PlayRevealAnimation()
    {
        player1Animator.SetTrigger("Reveal");
        player2Animator.SetTrigger("Reveal");

        yield return new WaitForSeconds(0.5f);
    }

    public void TestAnimation()
    {
        StartCoroutine(PlayRevealAnimation());
    }
}