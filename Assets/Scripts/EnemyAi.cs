using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAi : MonoBehaviour, IBlockable
{
    public GameObject Player;
    private Transform PlayerPosition;
    private CharacterController PlayerControler;

    private Animator _animator;

    private Vector3 _movementVector;

    private float attackDelay;
    public float AttackRange = 1.5f;

    public bool blocked;
    public float blockedTimeout;
    
    public float blockTimeout;

    public bool isBlocking { get; set; }

    private int currentAttack = 1;
    // Start is called before the first frame update
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;

    public bool dead;
    public int health = 100;
    public HealthBar healthBar;

    void Start()
    {
        PlayerPosition = Player.transform;
        PlayerControler = Player.GetComponent<CharacterController>();
        _particleSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        health = 100;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        var enemyPosition = transform.position;
        var vectorBetween = PlayerPosition.position - enemyPosition;
        var distance = vectorBetween.magnitude;
        
        if (dead)
        {
            return;
        }


        if (health < 0)
        {
            dead = true;
            transform.rotation = Quaternion.Euler(0, 0, 90);
            GetComponent<Rigidbody>().isKinematic = true;
            return;
        }
        
        if (distance > 25) return;

        

        if (blockedTimeout >= 0)
        {
            blockedTimeout -= Time.deltaTime;
            blocked = true;
        }
        else
        {
            blocked = false;
            
        }
        
        if (blockTimeout >= 0f) blockTimeout -= Time.deltaTime;
        
        

        if (blockTimeout <= 0f)
        {
            isBlocking = false;
        }
        

        attackDelay -= Time.deltaTime;

        if (distance < 20)
            transform.rotation = Quaternion.LookRotation(vectorBetween);
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCK"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCKED"))
        {
            if(!blocked)
                blockedTimeout = 1f;
            return;

        }

        if (attackDelay <= 0f)
        {
            if (distance < 10 && distance > AttackRange)
            {
                _movementVector = vectorBetween.normalized;
            } else {_movementVector = Vector3.zero;}
        }

        if (distance <= AttackRange)
        {

            if (attackDelay <= 0f)
            {
                
                
                
                if (PlayerControler.isAttacking)
                {
                    _animator.SetTrigger("BLOCK");
                    attackDelay = 1f;
                    isBlocking = true;
                    blockTimeout = 0.6f;
                    return;
                }
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
                
            
            
        }

        if (_movementVector != Vector3.zero)
        {
            if (_audioSource.isPlaying is false)
                _audioSource.Play();
            _animator.SetFloat("Speed", 1f);
        }
        else
        {
            _animator.SetFloat("Speed", 0f);
            if (_audioSource.isPlaying is true)
                _audioSource.Stop();
        }
        
    }

    private void FixedUpdate()
    {
        transform.position += _movementVector * 8 * Time.deltaTime;
    }

    public void GotHit(GameObject sender)
    {
        var parentId = GetComponentInParent<Guid>().Id;
        var senderId = sender.gameObject.GetComponentInParent<Guid>().Id;

        if (isBlocking) return;

        if (parentId != senderId)
        {
            _particleSystem.Play();
            health -= 40;
            healthBar.SetHealth(health);
        }
    }
}