using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp = 5.0f;
    private float curHp = 5.0f;
    [SerializeField] private float speed = 4.0f;

    [SerializeField] private Sprite sprHit;
    private Sprite sprDefault;

    private SpriteRenderer sr;
    [SerializeField] private GameObject objExplosion;

    private Transform layerDynamic;
    private GameManager gameManager;
    private bool haveItem = false;
    private bool isDeath = false;

    [SerializeField] bool isBoss = false;
    private Camera mainCam;

    [Header("보스용 패턴")]
    [SerializeField] private float startPosY;   //시작위치
    private bool isStartMoving;                 // 첫 시작시 이동 연출
    private float ratioY = 0.0f;                // 이동 기능 연출 비율
    private bool isSwayRight = false;           //보스가 좌로 가야하는지 우로 가야하는지

    [Header("보스 패턴")]
    // 앞으로 발사
    [SerializeField] private int pattern1Count = 8;
    [SerializeField] private float pattern1Reload = 1.0f;
    [SerializeField] private GameObject pattern1Bullet;
    // 샷건
    [SerializeField] private int pattern2Count = 8;
    [SerializeField] private float pattern2Reload = 1.0f;
    [SerializeField] private GameObject pattern2Bullet;
    // 조준 개틀링
    [SerializeField] private int pattern3Count = 8;
    [SerializeField] private float pattern3Reload = 1.0f;
    [SerializeField] private GameObject pattern3Bullet;

    private int pattern = 1;
    private int patternShootCount = 0;
    private float shootTimer = 0.0f;
    private bool patternChange = false;


    public bool IsBoos
    {
        get
        {
            return isBoss;
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        curHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        sprDefault = sr.sprite;
        startPosY = transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        //layerDynamic = GameObject.Find("LayerDynamic").transform;
        gameManager = GameManager.Instance;
        layerDynamic = gameManager.GetLayerDynamic();

        if (haveItem == true)
        {
            sr.color = new Color(0.0f, 0.7f, 1.0f);
        }

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        ShootPattern();
    }
    private void Moving()
    {
        if (isBoss == false)
        {
            transform.position += -transform.up * Time.deltaTime * speed;
        }
        else
        {
            if(isStartMoving == false)
            {
                BossStartMoving();
            }
            else
            {
                BossSwayMoving();
            }
        }
    }

    private void ShootPattern()
    {
        if (isBoss == false || isStartMoving == false)
        {
            return;
        }

        shootTimer += Time.deltaTime;

        // 패턴 변경 중
        if (patternChange == true)
        {
            if(shootTimer >= 3.0f)
            {
                shootTimer = 0.0f;
                patternChange = false;
            }
            return;
        }

        switch (pattern)
        {
            case 1: // 앞으로 발사
                {
                    if(shootTimer >= pattern1Reload)
                    {
                        shootTimer = 0.0f;

                        ShootStraight();

                        if (patternShootCount >= pattern1Count)
                        {
                            pattern++;
                            patternChange = true;
                            patternShootCount = 0;
                        }
                    }
                }
                break;
            case 2: // 샷건
                {
                    if (shootTimer >= pattern2Reload)
                    {
                        shootTimer = 0.0f;

                        ShootShotgun();

                        if (patternShootCount >= pattern2Count)
                        {
                            pattern++;
                            patternChange = true;
                            patternShootCount = 0;
                        }
                    }
                }
                break;
            case 3: // 조준 개틀링
                {
                    if (shootTimer >= pattern3Reload)
                    {
                        shootTimer = 0.0f;

                        shootGatling();

                        if (patternShootCount >= pattern3Count)
                        {
                            pattern = 1;
                            patternChange = true;
                            patternShootCount = 0;
                        }
                    }
                }
                break;
            default: 
                break;
        }
    }

    private void CreateBullet(GameObject _obj, Vector3 _pos, Vector3 _rot, float _speed)
    {        
        GameObject obj = Instantiate(_obj, _pos, Quaternion.Euler(_rot), layerDynamic);
        Bullet objSc = obj.GetComponent<Bullet>();

        objSc.SetDamege(false, 1.0f, _speed);
    }

    // 전방으로 총알 발사
    private void ShootStraight()
    {
        CreateBullet(pattern1Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f), 5.0f);
        CreateBullet(pattern1Bullet, transform.position + new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 180.0f), 5.0f);
        CreateBullet(pattern1Bullet, transform.position + new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 180.0f), 5.0f);

        patternShootCount++;
    }
    
    private void ShootShotgun()
    {
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f + 15.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f - 15.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f + 30.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f - 30.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f + 45.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f - 45.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f + 60.0f), 4.0f);
        CreateBullet(pattern2Bullet, transform.position, new Vector3(0.0f, 0.0f, 180.0f - 60.0f), 4.0f);


        patternShootCount++;
    }

    private void shootGatling()
    {
        Vector3 playerPos = gameManager.GetPlayerGameObject().transform.position;
        float angle = Quaternion.FromToRotation(Vector3.up, playerPos - transform.position).eulerAngles.z;

        CreateBullet(pattern3Bullet, transform.position, new Vector3(0.0f, 0.0f, angle), 6.0f);

        patternShootCount++;
    }

    private void BossStartMoving()
    {
        ratioY += Time.deltaTime * 0.3f;
        if (ratioY >= 1.0f)
        {
            isStartMoving = true;
        }

        Vector3 vecDestination = transform.localPosition;
        vecDestination.y = Mathf.SmoothStep(startPosY, 3.0f, ratioY);
        transform.localPosition = vecDestination;
    }

    private void BossSwayMoving()
    {
        if (isSwayRight == false)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
        }
        CheckBossMoving();
    }

    private void CheckBossMoving()
    {
        Vector3 curPos = mainCam.WorldToViewportPoint(transform.position);

        if(isSwayRight == true && curPos.x > 0.75f)
        {
            isSwayRight = false;
        }
        else if (isSwayRight == false && curPos.x < 0.25f)
        {
            isSwayRight = true;
        }
    }

    public void Hit(float damage, bool bodyslam = false)
    {
        curHp -= damage;

        if (curHp <= 0 || (bodyslam == true && isBoss == false))
        {
            
            Destroy(gameObject);
            GameObject obj = Instantiate(objExplosion, transform.position, Quaternion.identity, layerDynamic);
            Explosion objSc = obj.GetComponent<Explosion>();
            float sizeWidth = sr.sprite.rect.width;
            objSc.SetAnimationSize(sizeWidth);

            // 만약 바디 슬램으로 들어온 코드라면 아이템을 주지않음
            if(isDeath = true && haveItem == true && bodyslam ==false)
            {
                gameManager.CreateItem(transform.position);
            }
            isDeath = true;
        }
        else
        {
            sr.sprite = sprHit;
            Invoke("SetSpriteDefault", 0.1f);
        }
    }

    private void SetSpriteDefault()
    {
        sr.sprite = sprDefault;
    }

    public void SetHaveItem()
    {
        haveItem = true;

        
    }
}
