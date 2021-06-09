using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDecrease_IEnumerator : MonoBehaviour
{
    public GameObject explosionTrigger;
    public AnimationCurve curve;
    public float speed = 0.2f;


    private void Start()
    {

    }

    private void Update()
    {
        if (explosionTrigger.activeSelf)
        {
            StartCoroutine(ScaleDownAnimation(1.0f));
        }
    }

    IEnumerator ScaleDownAnimation(float time)
    {
        float i = 0;
        float rate =  speed/ time;

        Vector3 fromScale = transform.localScale;
        Vector3 toScale = Vector3.zero;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(fromScale, toScale, curve.Evaluate(i));
            yield return 0;
        }
    }
}