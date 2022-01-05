using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 0.1f;

    public float RotationSpeed = 10f;

    public GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        var movementVector = new Vector3(horizontalAxis, 0f, verticalAxis);

        if (movementVector != Vector3.zero)
        {
            movementVector = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f) * movementVector;

            transform.position += movementVector * Speed;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                Time.deltaTime * RotationSpeed
            );
        }

    }
}
