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
    [Header("������ ����")]
    [SerializeField] private float startPosY;   //������ġ
    private bool isStartMoving;                 // ù ���۽� �̵� ����
    private float ratioY = 0.0f;                // �̵� ��� ���� ����
    private bool isSwayRight = false;           //������ �·� �����ϴ��� ��� �����ϴ���

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

            // ���� �ٵ� �������� ���� �ڵ��� �������� ��������
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
