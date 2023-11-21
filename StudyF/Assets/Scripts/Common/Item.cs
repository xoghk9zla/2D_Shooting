using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public enum ItemType
    {
        HpRecovery,
        PowerUp,
    }

    [SerializeField] ItemType itemType;
    private Vector3 moveDir;    // 움직일 방향
    private float moveSpeed;  // 움직일 속도

    private Camera camMain;

    [SerializeField] float dirMin = -1.0f;
    [SerializeField] float dirMax = 1.0f;
    [SerializeField] float speedMin = 2.0f;
    [SerializeField] float speedMax = 5.0f;

    private void Awake()
    {
        moveDir = new Vector2(Random.Range(dirMax, dirMax), Random.Range(dirMin, dirMax));
        moveSpeed = Random.Range(speedMin, speedMax);
    }
    private void Start()
    {
        camMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        CheckPos();
    }

    /// <summary>
    /// 경계에 닿으면  반대로 튕깁니다
    /// </summary>
    private void CheckPos()
    {
        Vector3 curPos = camMain.WorldToViewportPoint(transform.position);

        if (curPos.x < 0.1f)
        {
            moveDir = Vector3.Reflect(moveDir, Vector3.left);
        }
        else if(curPos.x > 0.9f)
        {
            moveDir = Vector3.Reflect(moveDir, Vector3.right);
        }

        if(curPos.y < 0.1f)
        {
            moveDir = Vector3.Reflect(moveDir, Vector3.down);
        }
        else if(curPos.y > 0.9f)
        {
            moveDir = Vector3.Reflect(moveDir, Vector3.up);
        }
    }

    public ItemType GetItemType()
    {
        return itemType;
    }
}
