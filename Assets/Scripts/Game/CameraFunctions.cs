using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public bool isShaking;
    float shakeX, shakeY;
    Vector3 shakeValue;
    public Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        if (isShaking)
        {
            shakeValue = new Vector3(shakeX, shakeY);
            transform.position = startPosition + shakeValue;
        }
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0;

        while (elapsed < duration)
        {
            shakeX = Random.Range(-1, 1) * magnitude;
            shakeY = Random.Range(-1, 1) * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeValue = Vector3.zero;
        transform.position = startPosition;
        isShaking = false;
    }
}
