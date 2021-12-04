using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg_Initiate : MonoBehaviour
{
    public float prob = 100;
    public bool hasEasterEggAppearedAlready;
    public GameObject spawnPoint;
    public GameObject easterEgg;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnEasterEgg", 0, 15);   
    }

    void spawnEasterEgg() {
        bool shouldSpawn = Random.Range(0, 100) <= prob;
        if (shouldSpawn && !hasEasterEggAppearedAlready) {
            GameObject _ = Instantiate(easterEgg, spawnPoint.transform.position, spawnPoint.transform.rotation);
            _.name = "easteregg";
            hasEasterEggAppearedAlready = true;
        }
        else if(shouldSpawn && hasEasterEggAppearedAlready) {
            if(GameObject.Find("easteregg") == null) {
                hasEasterEggAppearedAlready = false;
            }
        }

    }
}
