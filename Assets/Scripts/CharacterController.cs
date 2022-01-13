using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    public float speed = 10;
    private Animator _animator;
    public Vector3 direction;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.zero;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        direction = new Vector3(horizontal, 0, vertical);
        if(Input.GetKeyDown(KeyCode.Q))
            _animator.SetTrigger("ATTACK_1");
        if(Input.GetKeyDown(KeyCode.E))
            _animator.SetTrigger("ATTACK_2");
        if(Input.GetKeyDown(KeyCode.R))
            _animator.SetTrigger("ATTACK_3");
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero)
        {
            _animator.SetFloat("Speed",0f);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
            _animator.SetFloat("Speed",1f);
        }
        
        
    }

}
