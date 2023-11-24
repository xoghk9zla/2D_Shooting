using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤

    private Camera maincam;

    [Header("플레이어")]
    [SerializeField] GameObject objPlayer;

    [Header("적기 생성")]
    [SerializeField] private List<GameObject> listEnemys;
    [SerializeField, Range(0.1f, 1.0f)] private float timerSpawn;
    private float timer = 0.0f;
    [SerializeField] bool boolEnemySpawn = false;
    [SerializeField] Vector3 vecSpawnPos;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform layerEnemy;
    [SerializeField] Transform layerDynamic;

    [Header("아이템 생성")]
    [SerializeField, Range(0.0f, 100.0f)] float dropRate = 0.0f;
    [SerializeField] private List<GameObject> listItem;

    [Header("게이지")]
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] Image sliderFillImage;

    [SerializeField] float bossSpawnTime = 60.0f;
    [SerializeField] private float curtime = 0.0f;
    private bool bossSpawn = false;
    [SerializeField] GameObject objBoss;
    [SerializeField] Color sliderTimerColor;
    [SerializeField] Color sliderBossHpColor;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maincam = Camera.main;
        SetNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawn();
        CheckBossTimer();

        CheckSliderColor();
    }

    private void CheckSpawn()
    {
        if (boolEnemySpawn == false || bossSpawn == true)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= timerSpawn)
        {
            EnemySpawn();
            timer = 0.0f;
        }
    }

    private void CheckBossTimer()
    {
            
        if (bossSpawn == true)
        {
            return;
        }       

        curtime += Time.deltaTime;

        if(curtime >= bossSpawnTime)
        {
            bossSpawn = true;
            EnemyBossSpawn();
            DestroyAllEnemy();
        }

        // todo 수정
        SetSliderText();
        SetSlider();
    }

    private void DestroyAllEnemy()
    {
        int count = layerEnemy.childCount;

        for(int i = count - 1; i > -1; i--)
        {
            Transform objEnemy = layerEnemy.GetChild(i);
            Enemy objSc = objEnemy.GetComponent<Enemy>();

            if(objSc.IsBoos == false)
            {
                Destroy(objEnemy.gameObject);
            }
        }

    }

    private void CheckSliderColor()
    {
        if(bossSpawn == false && sliderFillImage.color != sliderTimerColor)
        {
            sliderFillImage.color = sliderTimerColor;
        }
        else if(bossSpawn == true && sliderFillImage.color != sliderBossHpColor)
        {
            sliderFillImage.color = sliderBossHpColor;
        }
    }

    private void SetSliderText()
    {
        string timerValue = $"{(int)curtime} / {(int)bossSpawnTime}";
        sliderText.text = timerValue;
    }

    private void SetSlider()
    {
        slider.value = curtime;
    }

    private void SetSliderDefault()
    {
        slider.maxValue = bossSpawnTime;
        slider.minValue = 0.0f;
    }

    // 새 게임, 다음 게임
    private void SetNewGame()
    {
        curtime = 0.0f;

        SetSliderDefault();
        SetSliderText();
        SetSlider();
    }

    private void EnemySpawn()
    {
        int iRand = Random.Range(0, listEnemys.Count);
        GameObject objEnemy = listEnemys[iRand];

        var limitPosX = GetLimitGameScene();
        float xPos = Random.Range(limitPosX._min, limitPosX._max);
        Vector3 instPos = new Vector3(xPos, trsSpawnPos.position.y, 0.0f);

        GameObject obj = Instantiate(objEnemy, instPos, Quaternion.identity, layerEnemy);
        Enemy objSc = obj.GetComponent<Enemy>();

        float rate = Random.Range(0.0f, 100.0f);
        if(rate <= dropRate)
        {
            objSc.SetHaveItem();
        }
    }

    private void EnemyBossSpawn()
    {
        GameObject obj = Instantiate(objBoss, trsSpawnPos.position, Quaternion.identity, layerEnemy);
        Enemy objSc = obj.GetComponent<Enemy>();
    }

    private (float _min, float _max) GetLimitGameScene()
    {
        float minValue = maincam.ViewportToWorldPoint(new Vector3(0.1f, 0.0f, 0.0f)).x;
        float maxValue = maincam.ViewportToWorldPoint(new Vector3(0.9f, 0.0f, 0.0f)).x;

        return (minValue, maxValue);
    }

    public Transform GetLayerDynamic()
    {
        return layerDynamic;
    }

    public GameObject GetPlayerGameObject()
    {
        return objPlayer;
    }

    public void CreateItem(Vector3 createPos)
    {
        int rand = Random.Range(0, listItem.Count);
        Instantiate(listItem[rand], createPos, Quaternion.identity, layerDynamic);
    }
}
