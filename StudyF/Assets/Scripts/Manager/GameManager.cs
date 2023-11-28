using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // �̱���

    private Camera maincam;

    [Header("�÷��̾�")]
    [SerializeField] GameObject objPlayer;

    [Header("���� ����")]
    [SerializeField] private List<GameObject> listEnemys;
    [SerializeField, Range(0.1f, 1.0f)] private float timerSpawn;
    private float timer = 0.0f;
    [SerializeField] bool boolEnemySpawn = false;
    [SerializeField] Vector3 vecSpawnPos;
    [SerializeField] Transform trsSpawnPos;
    [SerializeField] Transform layerEnemy;
    [SerializeField] Transform layerDynamic;

    [Header("������ ����")]
    [SerializeField, Range(0.0f, 100.0f)] float dropRate = 0.0f;
    [SerializeField] private List<GameObject> listItem;

    [Header("������")]
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text sliderText;
    [SerializeField] Image sliderFillImage;

    [SerializeField] float bossSpawnTime = 60.0f;
    [SerializeField] private float curtime = 0.0f;
    private bool bossSpawn = false;
    [SerializeField] GameObject objBoss;
    [SerializeField] Color sliderTimerColor;
    [SerializeField] Color sliderBossHpColor;

    [SerializeField] TMP_Text scoreText;
    private int curScore = 0;
    private List<UserScore> listScore = new List<UserScore>();
    private string scoreKey = "scoreKey";

    public class UserScore
    {
        public int score;
        public string name;
    }

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
        SetScore();
    }

    private void SetScore()
    {
        if(PlayerPrefs.HasKey(scoreKey))
        {
            string savedValue = PlayerPrefs.GetString(scoreKey);

            if(savedValue == string.Empty)
            {
                ClearAllScore();
            }
            else
            {
                listScore = JsonConvert.DeserializeObject<List<UserScore>>(savedValue);
            }
        }
        else
        {
            ClearAllScore();
        }
    }

    private void ClearAllScore()
    {
        listScore.Clear();

        for(int i = 0; i < 10; ++i)
        {
            listScore.Add(new UserScore());
        }
        
        string saveValue = JsonConvert.SerializeObject(listScore);
        PlayerPrefs.SetString(scoreKey, saveValue);
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

        // todo ����
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
                //Destroy(objEnemy.gameObject);
                if(objEnemy.position.x > 0.0f)
                {
                    objEnemy.transform.position += new Vector3(1.0f, 0.0f, 0.0f);
                }
                else if(objEnemy.position.x < 0.0f)
                {
                    objEnemy.transform.position += new Vector3(-1.0f, 0.0f, 0.0f);
                }
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

    // �� ����, ���� ����
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


    /// <summary>
    /// ������ ���� ü�� �� �ִ� ü���� �������� ����մϴ�
    /// </summary>
    /// <param name="_curHp">����ü��</param>
    /// <param name="_maxHp">�ִ�ü��</param>
    public void SetBossHP(float _curHp, float _maxHp)
    {
        if (slider.maxValue != _maxHp)
        {
            slider.maxValue = _maxHp;
        }

        slider.value = _curHp;
        sliderText.text = $"{(int)_curHp} / {(int)_maxHp}";
    }

    public void GameCoutinue()
    {
        bossSpawn = false;
        bossSpawnTime += 10.0f;
        SetNewGame();
    }

    public void ShowScore(int _score)
    {
        curScore += _score;
        scoreText.text = curScore.ToString("D8");
    }

    public void GameOver()
    {

        SetNewGame();
    }
}

