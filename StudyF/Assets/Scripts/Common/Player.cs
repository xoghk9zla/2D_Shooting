using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 기초 데이터
    [Header("기초 데이터")]
    [SerializeField] private Camera maincam;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform layerDynamic;

    // 플레이어 데이터
    [Header("플레이어 데이터")]
    [SerializeField, Tooltip("플레이어의 이동속도")][Range(0.5f, 100.0f)] private float speedPlayer = 1.0f;
    [SerializeField] float horizontal;
    [SerializeField] float vertical;

    [Header("프리팹")]
    [SerializeField] private GameObject objBullet;

    [Header("총알세팅")]
    [SerializeField] private bool autoBullet = false;
    [SerializeField, Range(0.0f, 3.0f)] private float timerShoot = 0.5f;
    private float timer = 0.0f;
    [SerializeField] private float bulletDamage = 0.0f;

    [Header("플레이어 데이터")]
    [SerializeField] private float maxHp = 3.0f;
    [SerializeField] private float curHp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == GameTag.Enemy.ToString())
        {
            Hit(1.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        maincam = Camera.main;
        animator = transform.GetComponent<Animator>();

        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        CheckMovePosition();
        DoAnimation();

        CheckShootBullet();
    }

    /// <summary>
    /// 플레이어 이동 기능
    /// </summary>
    private void Moving()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(horizontal, vertical, 0) * Time.deltaTime * speedPlayer;
  
    }

    private void CheckMovePosition()
    {
        Vector3 currentPlayerPos = maincam.WorldToViewportPoint(transform.position);
        
        if (currentPlayerPos.x < 0.1f)
        {
            currentPlayerPos.x = 0.1f;
        }
        else if (currentPlayerPos.x > 0.9f)
        {
            currentPlayerPos.x = 0.9f;
        }
        else if (currentPlayerPos.y < 0.1f)
        {
            currentPlayerPos.y = 0.1f;
        }
        else if (currentPlayerPos.y > 0.9f)
        {
            currentPlayerPos.y = 0.9f;
        }

        Vector3 fixedplayerpos = maincam.ViewportToWorldPoint(currentPlayerPos);
        transform.position = fixedplayerpos;
    }

    private void DoAnimation()
    {
        int fixedHorizontal = (int)horizontal;
        animator.SetInteger("keyHorizontal", fixedHorizontal);
    }
    void CheckShootBullet()
    {
        if (autoBullet != true && Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
        else if (autoBullet == true)
        {
            timer += Time.deltaTime;

            if (timerShoot <= timer)
            {
                ShootBullet();
                timer = 0.0f;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject obj = Instantiate(objBullet, transform.position, Quaternion.identity, layerDynamic);
        Bullet objSc = obj.GetComponent<Bullet>();
        objSc.SetDamege(true, bulletDamage);
    }

    public void Hit(float damage)
    {
        curHp -= damage;
        if(curHp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {

        }
    }
}
