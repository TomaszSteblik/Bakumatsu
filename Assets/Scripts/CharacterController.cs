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

    private bool comboLock;
    private float timeForAttack2;
    private float timeForAttack3;
    private float attack1Timeout;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
        //     return;
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
        //     return;
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
        //     return;

        if (attack1Timeout > 0) attack1Timeout -= Time.deltaTime;
        if (timeForAttack2 > 0) timeForAttack2 -= Time.deltaTime;
        if (timeForAttack3 > 0) timeForAttack3 -= Time.deltaTime;
        if (timeForAttack2 <= 0 && timeForAttack3 <= 0 && attack1Timeout <= 0) comboLock = false;
        
        

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!comboLock)
            {
                _animator.SetTrigger("ATTACK_1");
                timeForAttack2 = 1.5f;
                comboLock = true;
                attack1Timeout = 2.5f;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (timeForAttack2 > 0 && timeForAttack2 < 1f)
            {
                _animator.SetTrigger("ATTACK_2");
                timeForAttack3 = 1.5f;
            }

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (timeForAttack3 > 0 && timeForAttack3 < 1f)
            {
                _animator.SetTrigger("ATTACK_3");
            }   
        }

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
