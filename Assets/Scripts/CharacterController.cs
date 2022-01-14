using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 0.1f;

    public Vector3 movementVector;
    public float RotationSpeed = 10f;

    public GameObject Camera;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        
        
        if(Input.GetKeyDown(KeyCode.J))
            _animator.SetTrigger("ATTACK_1");
        if(Input.GetKeyDown(KeyCode.K))
            _animator.SetTrigger("ATTACK_2");
        if(Input.GetKeyDown(KeyCode.L))
            _animator.SetTrigger("ATTACK_3");

    }

    private void FixedUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        movementVector = new Vector3(horizontalAxis, 0f, verticalAxis);
        movementVector = movementVector.normalized;

        if (movementVector != Vector3.zero)
        {
            movementVector = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f) * movementVector;
            transform.position += movementVector * Speed * Time.deltaTime;


            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                RotationSpeed * Time.deltaTime);
            _animator.SetFloat("Speed",1f);
        }
        else
        {
            _animator.SetFloat("Speed",0f);
        }

    }
}
