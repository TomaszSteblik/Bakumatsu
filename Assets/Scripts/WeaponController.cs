using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator _parentAnimator;
    private AudioSource _audioSource;

    void Start()
    {
        _parentAnimator = GetComponentInParent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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
                IBlockable otherChar = other.gameObject.GetComponentInParent<CharacterController>();
                if (otherChar is null) otherChar = other.gameObject.GetComponentInParent<EnemyAi>();
                if (otherChar.isBlocking)
                {
                    _parentAnimator.SetTrigger("BLOCKED");
                    Debug.Log("blocked");
                    if (_audioSource.isPlaying is false)
                        _audioSource.Play();
                }
            }
        }
    }

}
