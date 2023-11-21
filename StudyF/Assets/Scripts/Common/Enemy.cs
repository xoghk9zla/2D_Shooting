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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        curHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        sprDefault = sr.sprite;
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
        transform.position += -transform.up * Time.deltaTime * speed;
    }

    public void Hit(float damage, bool bodyslam = false)
    {
        curHp -= damage;

        if (curHp <= 0 || bodyslam == true)
        {
            Destroy(gameObject);
            GameObject obj = Instantiate(objExplosion, transform.position, Quaternion.identity, layerDynamic);
            Explosion objSc = obj.GetComponent<Explosion>();
            float sizeWidth = sr.sprite.rect.width;
            objSc.SetAnimationSize(sizeWidth);

            // 만약 바디 슬램으로 들어온 코드라면 아이템을 주지않음
            if(haveItem == true && bodyslam ==false)
            {
                gameManager.CreateItem(transform.position);
            }

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
