using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    public Vector2 max, min;
    private float orgSpeed;
    public float speed;
    public bool hitting;
    public float secondsToWait;
    Sprite spriteOrg;
    SpriteRenderer spriteRenderer;
    public GameObject wasp;
    AudioSource AS;
    public AudioClip swat;
    GameController gameController;
    public static PlayerMovement control;
    public bool DEBUG_CONTROLS;

    void Awake() {
        // Singleton Pattern
        if (!control) control = this;
        else Destroy(gameObject);

        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = GameContext.control.GetVolume("fx");

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteOrg = spriteRenderer.sprite;
        orgSpeed = speed;
        gameController = GameController.control;
    }

    void Update() {
        if (gameController.gameState != GameController.GameState.play) return;
        Movement();
        HitRoutine();
    }

    void Movement() {
        float hor, ver;

        if (!DEBUG_CONTROLS) {
            hor = -CrossPlatformInputManager.VirtualAxisReference("Horizontal").GetValue;
            ver = CrossPlatformInputManager.VirtualAxisReference("Vertical").GetValue;
        }
        else {
            hor = -Input.GetAxisRaw("Horizontal");
            ver = Input.GetAxisRaw("Vertical");
        }

        Vector3 movementX = new Vector3(hor, 0) * Time.deltaTime * speed;
        Vector3 movementY = new Vector3(0, ver) * Time.deltaTime * speed;
        if (IsWithinRange(transform.position.x + movementX.x, min.x, max.x)) transform.position += movementX;
        if (IsWithinRange(transform.position.y + movementY.y, min.y, max.y)) transform.position += movementY;
    }

    void HitRoutine() {
        if (Input.GetKeyDown(KeyCode.Space)) Hit();
    }

    public bool IsWithinRange(float val, float min, float max) {
        return val >= min && val <= max;
    }

    public void Hit() // Needed to call from Eventhandler in the Inspector
    {
        if (gameController.gameState != GameController.GameState.play) return;
        StartCoroutine("HitWrapper");
    }

    IEnumerator HitWrapper() {
        if (hitting) yield break;
        hitting = true;
        wasp.GetComponent<Animator>().SetBool("hit", true);
        if (!AS.isPlaying) AS.PlayOneShot(swat);
        yield return new WaitForSeconds(secondsToWait);
        wasp.GetComponent<Animator>().SetBool("hit", false);
        yield return new WaitForSeconds(secondsToWait);
        hitting = false;
    }

    public void ChangeVisuals(Sprite overrideSprite) {
        Debug.Log("About to change player visuals...");
        spriteRenderer.sprite = overrideSprite;
    }

    public void ChangeSpeed(float toSpeed) {
        Debug.Log("About to change player speed...");
        speed = toSpeed;
    }

    public void ReturnVisualsToNormal() {
        ChangeVisuals(spriteOrg);
    }

    public void ReturnToNormalSpeed() {
        ChangeSpeed(orgSpeed);
    }
}
