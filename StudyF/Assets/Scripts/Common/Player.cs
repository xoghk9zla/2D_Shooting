using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ���� ������
    [Header("���� ������")]
    [SerializeField] private Camera maincam;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform layerDynamic;

    // �÷��̾� ������
    [Header("�÷��̾� ������")]
    [SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")][Range(0.5f, 100.0f)] private float speedPlayer = 1.0f;
    [SerializeField] float horizontal;
    [SerializeField] float vertical;

    [Header("������")]
    [SerializeField] private GameObject objBullet;

    [Header("�Ѿ˼���")]
    [SerializeField] private bool autoBullet = false;
    [SerializeField, Range(0.0f, 3.0f)] private float timerShoot = 0.5f;
    private float timer = 0.0f;
    [SerializeField] private float bulletDamage = 0.0f;

    [Header("�÷��̾� ������")]
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
    /// �÷��̾� �̵� ���
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
