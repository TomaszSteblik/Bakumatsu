using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 8f;
    private float _actualSpeed;

    public Vector3 movementVector;
    public float RotationSpeed = 10f;

    public GameObject Camera;

    private Animator _animator;

    public bool isAttacking;
    private float toAttackEnd = 0f;

    private bool comboLock;
    private float timeForAttack2;
    private float timeForAttack3;
    private float attack1Timeout;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _actualSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (attack1Timeout > 0) attack1Timeout -= Time.deltaTime;
        if (timeForAttack2 > 0) timeForAttack2 -= Time.deltaTime;
        if (timeForAttack3 > 0) timeForAttack3 -= Time.deltaTime;
        if (timeForAttack2 <= 0 && timeForAttack3 <= 0 && attack1Timeout <= 0) comboLock = false;
        if (toAttackEnd > 0) toAttackEnd -= Time.deltaTime;
        if (toAttackEnd <= 0f) isAttacking = false;
        else isAttacking = true;
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCK"))
            return;
        
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        movementVector = new Vector3(horizontalAxis, 0f, verticalAxis);
        movementVector = movementVector.normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _actualSpeed = Speed * 1.75f;
        }
        else
        {
            _actualSpeed = Speed;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            _animator.SetTrigger("BLOCK");
            return;
        }
        
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!comboLock)
            {
                _animator.SetTrigger("ATTACK_1");
                timeForAttack2 = 1.5f;
                comboLock = true;
                attack1Timeout = 2.5f;
                toAttackEnd = 1f;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (timeForAttack2 > 0 && timeForAttack2 < 1f)
            {
                timeForAttack2 = 0f;
                _animator.SetTrigger("ATTACK_2");
                timeForAttack3 = 1.5f;
                toAttackEnd = 1f;
            }

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (timeForAttack3 > 0 && timeForAttack3 < 1f)
            {
                timeForAttack3 = 0f;
                _animator.SetTrigger("ATTACK_3");
                toAttackEnd = 1f;
            }   
        }
        
        if (movementVector != Vector3.zero)
        {
            movementVector = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f) * movementVector;
            transform.position += movementVector * _actualSpeed * Time.deltaTime;
            

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                RotationSpeed * Time.deltaTime);
            _animator.SetFloat("Speed",_actualSpeed);
        }
        else
        {
            _animator.SetFloat("Speed",0f);
        }

    }

    public void GotHit(GameObject sender)
    {
        var parentId = GetComponentInParent<Guid>().Id;
        var senderId = sender.gameObject.GetComponentInParent<Guid>().Id;
        
        if(parentId != senderId)    
            Debug.Log("PLAYER HIT");
    }
    private void FixedUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        

        

    }
}
