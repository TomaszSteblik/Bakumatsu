using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator _parentAnimator;
    void Start()
    {
        _parentAnimator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parentAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (other.CompareTag("BodyBone"))
            {
                other.gameObject.SendMessageUpwards("GotHit",gameObject);
            }
            else if (other.CompareTag("Weapon"))
            {
                if (other.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Block"))
                {
                    ResetAnimation();
                }
            }
        }
    }

    private void ResetAnimation()
    {
        _parentAnimator.ResetTrigger("ATTACK_1");
        _parentAnimator.ResetTrigger("ATTACK_2");
        _parentAnimator.ResetTrigger("ATTACK_3");
        _parentAnimator.Play("Idle");
    }
}
