using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Material matBottom;
    private Material matMiddle;
    private Material matTop;

    [SerializeField] private float speedBottom;
    [SerializeField] private float speedMiddle;
    [SerializeField] private float speedTop;

    // Start is called before the first frame update
    void Start()
    {
        Transform trsBottom = transform.Find("SpriteBottom");
        Transform trsMiddle = transform.Find("SpriteMiddle");
        Transform trsTop = transform.Find("SpriteTop");

        SpriteRenderer sprBottom = trsBottom.GetComponent<SpriteRenderer>();
        SpriteRenderer sprMiddle = trsMiddle.GetComponent<SpriteRenderer>();
        SpriteRenderer sprTop = trsTop.GetComponent<SpriteRenderer>();

        matBottom = sprBottom.material;
        matMiddle = sprMiddle.material;
        matTop = sprTop.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vecBottom = matBottom.mainTextureOffset;
        Vector2 vecMiddle = matMiddle.mainTextureOffset;
        Vector2 vecTop = matTop.mainTextureOffset;

        vecBottom += new Vector2(0, speedBottom * Time.deltaTime);
        vecMiddle += new Vector2(0, speedMiddle * Time.deltaTime);
        vecTop += new Vector2(0, speedTop * Time.deltaTime);

        vecBottom.y = Mathf.Repeat(vecBottom.y, 1);
        vecMiddle.y = Mathf.Repeat(vecMiddle.y, 1);
        vecTop.y = Mathf.Repeat(vecTop.y, 1);

        matBottom.mainTextureOffset = vecBottom;
        matMiddle.mainTextureOffset = vecMiddle;
        matTop.mainTextureOffset = vecTop;

    }

    
}
