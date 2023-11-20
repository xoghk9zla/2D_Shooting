using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    private Transform trsPlayer;    // 플레이어의 트랜스폼
    [SerializeField] private Image imgFrontHp;  // 실제 HP
    [SerializeField] private Image imgMidHp;    // 연출용 HP

    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameManager.Instance;
        GameObject obj = manager.GetPlayerGameObject();
        Player objSc = obj.GetComponent<Player>();
        objSc.SetPlayerHp(this);
        trsPlayer = obj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPos();
        CheckPlayerHP();    // 만약 MidHp가 FrontHp와 값이 다르면 같게, 천천히
        IsDestroying();
    }

    private void CheckPlayerPos()
    {
        transform.position = trsPlayer.position - new Vector3(0.0f, 0.65f, 0.0f);
    }

    private void CheckPlayerHP()
    {
        float amountFront = imgFrontHp.fillAmount;
        float amountMid = imgMidHp.fillAmount;

        if(amountFront < amountMid) // Mid가 깍여야함
        {
            imgMidHp.fillAmount -= Time.deltaTime * 0.5f;

            if(imgMidHp.fillAmount <= imgFrontHp.fillAmount)
            {
                imgMidHp.fillAmount = imgFrontHp.fillAmount;
            }
        }
        else if(amountFront > amountMid)    // 
        {
            imgMidHp.fillAmount = imgFrontHp.fillAmount;
        }
    }

    private void IsDestroying()
    {
        if(imgMidHp.fillAmount <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerHp(float curHp, float maxHp)
    {
        imgFrontHp.fillAmount = curHp / maxHp;
    }
}
