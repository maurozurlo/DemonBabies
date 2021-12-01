using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene_Bird : MonoBehaviour
{
    public Vector2 speedMod = new Vector2(1, 1);
    public Vector2 sizeMod = new Vector2(1, 1);
    public Vector2 rotMod = new Vector2(1, 1);
    public float speed = 10;
    private RectTransform rect;
    public float screenEnd;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();

        rect.localScale = GetVectorFromRange(sizeMod);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(rotMod.x, rotMod.y)));

        speed *= Random.Range(speedMod.x,speedMod.y);
    }


    Vector2 GetVectorFromRange(Vector2 range) {
        float v = Random.Range(range.x, range.y);
        return new Vector2(v,v);
    }

    // Update is called once per frame
    void Update()
    {
        rect.position = new Vector2(rect.position.x + speed * Time.deltaTime, rect.position.y);
        if (rect.position.x > screenEnd) DestroyImmediate(gameObject);
    }
}
