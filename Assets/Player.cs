using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInitializer initializer;

    void Start()
    {
        Vector3 startPos = transform.position + new Vector3(2, 0, 0);
        initializer.Init(startPos, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
