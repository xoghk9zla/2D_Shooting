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

    [SerializeField] private GameObject objRankContents;    // 버튼이 눌러지면 랭킹 오브젝트가 켜지게 하는 용도
    [SerializeField] private GameObject fabRankContents;    

    [SerializeField] private Button btnExitRankContents;    // 랭킹 오브젝트를 꺼줌
    [SerializeField] private Transform trsContents;         // 랭킹 프리팹들이 저장될 공간

    private List<UserScore> listScore = new List<UserScore>();
    private string scoreKey = "scoreKey";

    // Start is called before the first frame update
    void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            // 씬 변경
            // main->play
            SceneManager.LoadSceneAsync((int)enumScenes.PlayScene);
        });

        btnRank.onClick.AddListener(() =>
        {
            // 랭킹 관련 오브젝트 켜줌
            objRankContents.SetActive(true);
            CreateRankContents();
        });

        btnExit.onClick.AddListener(() =>
        {
            // 빌드 상태 -> 게임 종료
            // 에디터 -> stop
#if UNITY_EDITOR    // 전처리
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
        SetScore(); // 리스트 안에 10개의 데이터가 저장        
    }

    private void ClearAllRanking()  // 랭크 오브젝트가 존재 했다면 모두 삭제
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
                    Debug.LogError($"리스트 스코어의 갯수가 이상합니다. 리스트 스코어의 갯수 = {listScore.Count}");
                }
            }
        }
        else
        {
            ClearAllScore();
        }
    }

    /// <summary>
    /// 데이터가 올바르지 않거나 없을때 새로운 데이터를 정확한 양식으로 만들고 저장합니다.
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
