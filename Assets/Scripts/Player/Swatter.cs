using UnityEngine;

public class Swatter : MonoBehaviour {
    PlayerMovement playerMovement;

    private void Start() {
        playerMovement = gameObject.transform.parent.GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider col) {
        if (playerMovement.hitting) {
            if (col.CompareTag("Enemy")) {
                col.gameObject.GetComponentInParent<Baby>().KillWithSwatter();
            }
            else if (col.CompareTag("PowerUp")) {
                col.gameObject.GetComponent<PowerUp>().KillWithSwatter();
            }
        }
    }
}
