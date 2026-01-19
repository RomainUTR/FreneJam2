using System.Collections;
using UnityEngine;

public class SunBehaviour : MonoBehaviour
{
    public Vector3 baseRadius = new Vector3(1f,1f,1f);
    public float targetRadius = 10f;
    public float delay = 10f;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        Vector3 targetScale = new Vector3(targetRadius, targetRadius, targetRadius);
        float delta = (Time.time - startTime) / delay;
        transform.localScale = Vector3.Lerp(baseRadius, targetScale, delta);
    }
}
