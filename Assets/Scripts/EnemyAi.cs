using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform PlayerPosition;

    private Animator _animator;

    private Vector3 _movementVector;

    private float attackDelay;
    public float AttackRange = 1.5f;

    private int currentAttack = 1;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var enemyPosition = transform.position;
        var vectorBetween = PlayerPosition.position - enemyPosition;
        var distance = vectorBetween.magnitude;

        attackDelay -= Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(vectorBetween);
        if (distance < 10 && distance > AttackRange)
        {
            _movementVector = vectorBetween.normalized;
        } else {_movementVector = Vector3.zero;}

        if (distance <= AttackRange)
        {
            if(attackDelay <= 0f)
                switch (currentAttack)
                {
                    case 1:
                    {
                        _animator.SetTrigger("ATTACK_1");
                        currentAttack = 2;
                        attackDelay = 1f;
                        break;
                    }
                    case 2:
                    {
                        _animator.SetTrigger("ATTACK_2");
                        currentAttack = 3;
                        attackDelay = 1f;
                        break;
                    }
                    case 3:
                    {
                        _animator.SetTrigger("ATTACK_3");
                        currentAttack = 1;
                        attackDelay = 1f;
                        break;
                    }
                }
            
            
        }

        if (_movementVector != Vector3.zero)
        {
            _animator.SetFloat("Speed", 1f);
        }
        else
        {
            _animator.SetFloat("Speed", 0f);
        }
        
    }

    private void FixedUpdate()
    {
        transform.position += _movementVector * 8 * Time.deltaTime;
    }
}