using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timeDestroy = 0.5f;
    private float damage = 0.0f;
    private bool playerBullet = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerBullet == true && collision.gameObject.tag == GameTag.Enemy.ToString())
        {
            Enemy enemySc = collision.GetComponent<Enemy>();
            enemySc.Hit(damage);
            Destroy(gameObject);
        }            
        else if(playerBullet == false && collision.gameObject.tag == GameTag.Player.ToString())
        {
            Player playerSc = collision.GetComponent<Player>();
            playerSc.Hit(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
    }

    public void SetDamege(bool _isPlayer, float _damage, float _speed = -1)
    {
        playerBullet = _isPlayer;
        this.damage = _damage;

        if (speed != -1)
        {
            speed = _speed;
        }
    }
}
