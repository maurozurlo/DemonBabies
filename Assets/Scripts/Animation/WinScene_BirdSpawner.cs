using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScene_BirdSpawner : MonoBehaviour
{
    public GameObject birbPrefab;
    public GameObject spawnPoint;
    public Vector2 spawnDelay;

    Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("InstantiateBirb");
        spawnPos = spawnPoint.transform.position;
    }

    IEnumerator InstantiateBirb() {
        yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.y));
        float p = Random.Range(0, 1);
        if (p < .5f) {
            GameObject go = Instantiate(birbPrefab, spawnPos, Quaternion.identity);
            go.transform.SetParent(spawnPoint.transform);
        }
        StartCoroutine("InstantiateBirb");
    }
}
