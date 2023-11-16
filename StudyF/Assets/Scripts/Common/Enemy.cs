using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float hp = 5.0f;
    [SerializeField] private float speed = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }
    private void Moving()
    {
        transform.position += -transform.up * Time.deltaTime * speed;
    }
}
