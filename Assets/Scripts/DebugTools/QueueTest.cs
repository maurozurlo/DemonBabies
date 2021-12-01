using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueTest : MonoBehaviour
{
    public Queue<PowerUp> powerUps = new Queue<PowerUp>();
    public InputField input;
    public Text text;

    int timeToConvert = 923;

    public class PowerUp {
        public string name { get; set; }

        public PowerUp(string name){
            this.name = name;
        }
    }
    // Start is called before the first frame update

    public void AddQueueItem() {
        string pw = input.text;
        PowerUp power = new PowerUp(pw);
        powerUps.Enqueue(power);
        CurrQueueItem();
    }

    public void DeleteQueueItem() {
        powerUps.Dequeue();
        CurrQueueItem();
    }

    public void CurrQueueItem() {
        Debug.Log(powerUps.Peek());
        text.text = powerUps.Peek().name;
    }

    private void Start() {
        TimeSpan interval = new TimeSpan(0,0,0,0,timeToConvert * 10);
        Debug.Log(interval.ToString(@"mm\:ss\.ff"));
    }


}
