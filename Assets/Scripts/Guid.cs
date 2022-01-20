using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guid : MonoBehaviour
{
    // Start is called before the first frame update
    public System.Guid Id { get; set; }
    void Start()
    {
        Id = System.Guid.NewGuid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
