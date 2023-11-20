using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void SetAnimationSize(float size)
    {
        Vector3 scale = transform.localScale;
        scale *= size / 24.0f;
        transform.localScale = scale;
    }

    public void DoDestroyEvent()
    {
        Destroy(gameObject);
    }
}
