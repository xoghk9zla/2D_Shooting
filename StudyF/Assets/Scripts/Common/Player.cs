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
    private Transform trsShootPos;

    [Header("플레이어 데이터")]
    [SerializeField] private float maxHp = 3.0f;
    [SerializeField] private float curHp;
    [SerializeField] PlayerHp playerHp;
    [SerializeField, Range(1, 5)] private int level = 1;
    [SerializeField] private int levelMin = 1;
    [SerializeField] private int levelMax = 5;

    [SerializeField] private GameObject objExplosion;
    private SpriteRenderer sr;

    private void OnValidate()   // 인스펙터에서 값이 변경 되면 이 함수가 호출
    {
        if (playerHp != null)
        {
            playerHp.SetPlayerHp(curHp, maxHp);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameTag.Enemy.ToString())
        {
            Hit(1.0f);
            Enemy enemySc = collision.GetComponent<Enemy>();
            enemySc.Hit(0.0f, true);
        }
        else if (collision.tag == GameTag.Item.ToString())
        {
            // 어떤 아이템을 먹었는지 체크 후 아이템 타입에 맞는 기능을 실행\
            Item itemSc = collision.GetComponent<Item>();
            Item.ItemType itemType = itemSc.GetItemType();

            if (itemType == Item.ItemType.PowerUp)
            {
                level++;
                if (level > levelMax)
                {
                    level = levelMax;
                }
            }
            else if(itemType == Item.ItemType.HpRecovery)
            {
                curHp++;
                if(curHp > maxHp)
                {
                    curHp = maxHp;
                }
                playerHp.SetPlayerHp(curHp, maxHp);
            }            
            Destroy(collision.gameObject);
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        curHp = maxHp;
        trsShootPos = transform.Find("ShootPos");

    }

    // Start is called before the first frame update
    void Start()
    {
        //maincam = GameObject.Find("Main Camera").GetComponent<Camera>();
        maincam = Camera.main;
        animator = transform.GetComponent<Animator>();

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
        switch (level)
        {
            case 1:
                {
                    CreateBullet(trsShootPos.position, Vector3.zero);
                }
                break;
            case 2:
                {
                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), Vector3.zero);
                }
                break;
            case 3:
                {
                    CreateBullet(trsShootPos.position, Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), Vector3.zero);
                }
                break;
            case 4:
                {

                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), new Vector3(0.0f, 0.0f, 15.0f));
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), new Vector3(0.0f, 0.0f, -15.0f));
                }
                break;
            case 5:
                {
                    CreateBullet(trsShootPos.position, Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), Vector3.zero);
                    CreateBullet(trsShootPos.position + new Vector3(-0.25f, 0.0f), new Vector3(0.0f, 0.0f, 15.0f));
                    CreateBullet(trsShootPos.position + new Vector3(0.25f, 0.0f), new Vector3(0.0f, 0.0f, -15.0f));
                }
                break;
            default:
                Debug.LogError("레벨이 적용 할 수 있는 값을 초과했습니다. 확인해주세요.");
                break;
        }

    }

    private void CreateBullet(Vector3 pos, Vector3 rot)
    {
        GameObject obj = Instantiate(objBullet, pos, Quaternion.Euler(rot), layerDynamic);
        Bullet objSc = obj.GetComponent<Bullet>();
        objSc.SetDamege(true, bulletDamage);

    }

    public void Hit(float damage)
    {
        curHp -= damage;
        playerHp.SetPlayerHp(curHp, maxHp);

        if(curHp <= 0)
        {
            Destroy(gameObject);
            GameObject obj = Instantiate(objExplosion, transform.position, Quaternion.identity, layerDynamic);
            Explosion objSc = obj.GetComponent<Explosion>();
            float sizeWidth = sr.sprite.rect.width;
            objSc.SetAnimationSize(sizeWidth);
        }
        else
        {
            level--;
            if(level < levelMin)
            {
                level = levelMin;
            }
        }
    }

    public void SetPlayerHp(PlayerHp value)
    {
        playerHp = value;
        playerHp.SetPlayerHp(curHp, maxHp);
    }
}
