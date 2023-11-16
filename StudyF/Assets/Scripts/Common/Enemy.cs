using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp = 5.0f;
    private float curHp = 5.0f;
    [SerializeField] private float speed = 4.0f;

    [SerializeField] private Sprite sprHit;
    private Sprite sprDefault;
    private SpriteRenderer sr;

    private void Awake()
    {
        curHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        sprDefault = sr.sprite;
    }

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

    public void Hit(float damage)
    {
        curHp -= damage;
        if (curHp <= 0 )
        {
            Destroy(gameObject);
        }
        else
        {
            sr.sprite = sprHit;
        }
    }
}
