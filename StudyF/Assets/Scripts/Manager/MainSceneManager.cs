using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static GameManager;
using Newtonsoft.Json;

public enum enumScenes
{
    MainScene,
    PlayScene,
}

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private Button btnGameStart;
    [SerializeField] private Button btnRank;
    [SerializeField] private Button btnExit;

    [SerializeField] private GameObject objRankContents;    // ��ư�� �������� ��ŷ ������Ʈ�� ������ �ϴ� �뵵
    [SerializeField] private GameObject fabRankContents;    

    [SerializeField] private Button btnExitRankContents;    // ��ŷ ������Ʈ�� ����
    [SerializeField] private Transform trsContents;         // ��ŷ �����յ��� ����� ����

    private List<UserScore> listScore = new List<UserScore>();
    private string scoreKey = "scoreKey";

    // Start is called before the first frame update
    void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            // �� ����
            // main->play
            SceneManager.LoadSceneAsync((int)enumScenes.PlayScene);
        });

        btnRank.onClick.AddListener(() =>
        {
            // ��ŷ ���� ������Ʈ ����
            objRankContents.SetActive(true);
            CreateRankContents();
        });

        btnExit.onClick.AddListener(() =>
        {
            // ���� ���� -> ���� ����
            // ������ -> stop
#if UNITY_EDITOR    // ��ó��
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        btnExitRankContents.onClick.AddListener(() =>
        {
            objRankContents.SetActive(false);
        });
    }

    private void Awake()
    {
        InitRanking();
    }

    private void InitRanking()
    {
        ClearAllRanking();
        SetScore(); // ����Ʈ �ȿ� 10���� �����Ͱ� ����        
    }

    private void ClearAllRanking()  // ��ũ ������Ʈ�� ���� �ߴٸ� ��� ����
    {
        int count = trsContents.childCount;

        for(int i = count - 1; i > -1; --i)
        {
            Destroy(trsContents.GetChild(i).gameObject);
        }
    }

    private void SetScore()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            string savedValue = PlayerPrefs.GetString(scoreKey);

            if (savedValue == string.Empty)
            {
                ClearAllScore();
            }
            else
            {
                listScore = JsonConvert.DeserializeObject<List<UserScore>>(savedValue);

                if (listScore.Count != 10)
                {
                    Debug.LogError($"����Ʈ ���ھ��� ������ �̻��մϴ�. ����Ʈ ���ھ��� ���� = {listScore.Count}");
                }
            }
        }
        else
        {
            ClearAllScore();
        }
    }

    /// <summary>
    /// �����Ͱ� �ùٸ��� �ʰų� ������ ���ο� �����͸� ��Ȯ�� ������� ����� �����մϴ�.
    /// </summary>
    private void ClearAllScore()
    {
        listScore.Clear();

        for (int i = 0; i < 10; ++i)
        {
            listScore.Add(new UserScore());
        }

        string saveValue = JsonConvert.SerializeObject(listScore);
        PlayerPrefs.SetString(scoreKey, saveValue);
    }

    private void CreateRankContents()
    {
        int count = listScore.Count;

        for (int i = 0; i > count; ++i)
        {
            UserScore data = listScore[i];

            GameObject obj = Instantiate(fabRankContents, trsContents);
            RankContents objsc = obj.GetComponent<RankContents>();

            objsc.SetRankContents($"{i + 1}", data.score.ToString("D8"), data.name);
        }
    }
}
