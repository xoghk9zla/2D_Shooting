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
    [SerializeField] private GameObject objExplosion;

    private Transform layerDynamic;
    private GameManager gameManager;
    private bool haveItem = false;
    private bool isDeath = false;

    [SerializeField] bool isBoss = false;
    [Header("보스용 패턴")]
    [SerializeField] private float startPosY;   //시작위치
    private bool isStartMoving;                 // 첫 시작시 이동 연출
    private float ratioY = 0.0f;                // 이동 기능 연출 비율
    private bool isSwayRight = false;           //보스가 좌로 가야하는지 우로 가야하는지

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
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
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
    
    private void BossStartMoving()
    {
        ratioY += Time.deltaTime * 0.3f;
        if (ratioY >= 1.0f)
        {
            isStartMoving = true;
        }

        Vector3 vecDestination = transform.localPosition;
        vecDestination.y = Mathf.SmoothStep(startPosY, 2.5f, ratioY);
        transform.localPosition = vecDestination;
    }

    private void BossSwayMoving()
    {

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
