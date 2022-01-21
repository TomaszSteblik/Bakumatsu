using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CharacterController : MonoBehaviour, IBlockable
{
    // Start is called before the first frame update
    public float Speed = 8f;
    private float _actualSpeed;

    public Vector3 movementVector;
    public float RotationSpeed = 10f;

    public GameObject Camera;

    private Animator _animator;

    public bool isAttacking;
    public bool isBlocking { get; set; }
    
    public bool blocked;
    public float blockedTimeout;


    public bool inputLock;
    public float inputTimeout;

    public float attack1Timeout;
    public float attack2Timeout;
    public float attack3Timeout;
    public float blockTimeout;

    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
        _actualSpeed = Speed;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        movementVector = new Vector3(horizontalAxis, 0f, verticalAxis);
        movementVector = movementVector.normalized;

        if (attack1Timeout >= 0f) attack1Timeout -= Time.deltaTime;
        if (attack2Timeout >= 0f) attack2Timeout -= Time.deltaTime;
        if (attack3Timeout >= 0f) attack3Timeout -= Time.deltaTime;
        if (blockTimeout >= 0f) blockTimeout -= Time.deltaTime;
        if (blockedTimeout >= 0f) blockedTimeout -= Time.deltaTime;

        if (blockedTimeout >= 0f)
        {
            return;
        }
        

        if (blockTimeout <= 0f)
        {
            isBlocking = false;
        }

        if (inputTimeout <= 0f)
        {
            inputLock = false;
            isAttacking = false;
        }
        else
        {
            inputLock = true;
            inputTimeout -= Time.deltaTime;
        }

        if (inputLock) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _actualSpeed = Speed * 1.75f;
        }
        else
        {
            _actualSpeed = Speed;
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCKED"))
        {
            if(!blocked)
                blockedTimeout = 1f;
            return;

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            _animator.SetTrigger("BLOCK");
            inputTimeout = 0.3f;
            isBlocking = true;
            blockTimeout = 0.6f;
            
            return;
        }
        
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (attack1Timeout <= 0f)
            {
                _animator.SetTrigger("ATTACK_1");
                attack1Timeout = 0.6f;
                inputTimeout = 0.3f;
                isAttacking = true;
                return;
            }

        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if(attack2Timeout <= 0f){
                _animator.SetTrigger("ATTACK_2");
                attack2Timeout = 0.6f;
                inputTimeout = 0.3f;
                isAttacking = true;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if(attack3Timeout <= 0f){
                _animator.SetTrigger("ATTACK_3");
                attack3Timeout = 0.6f;
                inputTimeout = 0.3f;
                isAttacking = true;
                return;
            }
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_1"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_2"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_3"))
            return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BLOCK"))
            return;

        if (attack1Timeout > 0) return;
        if (attack2Timeout > 0) return;
        if (attack3Timeout > 0) return;
        
        
        if (movementVector != Vector3.zero)
        {
            movementVector = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f) * movementVector;
            transform.position += movementVector * _actualSpeed * Time.deltaTime;
            

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                RotationSpeed * Time.deltaTime);
            _animator.SetFloat("Speed",_actualSpeed);
            
            if (_audioSource.isPlaying is false)
                _audioSource.Play();
            
        }
        else
        {
            _animator.SetFloat("Speed",0f);
            if (_audioSource.isPlaying is true)
                _audioSource.Stop();
        }

    }

    public void GotHit(GameObject sender)
    {
        var parentId = GetComponentInParent<Guid>().Id;
        var senderId = sender.gameObject.GetComponentInParent<Guid>().Id;
        
        if(parentId != senderId)    
            _particleSystem.Play();
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
