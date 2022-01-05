using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public GameObject Player;

    private Vector3 _initialOffset;
    // Start is called before the first frame update
    void Start()
    {
        _initialOffset = Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position - _initialOffset;
    }
}
