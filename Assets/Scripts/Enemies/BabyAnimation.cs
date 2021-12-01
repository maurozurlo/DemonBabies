using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAnimation : MonoBehaviour {

    public Texture2D[] frames;
    public Texture2D[] hitFrame;
    public TextMesh comboText;
    public TextMesh multiplierText, shadowText;
    public float period = 0.1f;
    public float time = 0;
    public int imgWereAt = 0;
    public bool alive = true;
    public bool shake;
    public float speed = 1.0f;
    public float amount = 1.0f;
    public AudioClip combo1, combo2, combo3, combo4;
    private Vector3 orgPos;
    AudioSource AS;
    public AudioClip[] cry;
    int scoreToAdd;
    public GameObject exploPrefab, pivotPoint;
    GameController gameController;
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;

    void Start() {
        gameController = GameController.control;
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponentInParent<BoxCollider>();
        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = GameContext.control.GetVolume("fx");
        // Implement later...
        //if (GameController.control.upsideDownWorld) {
        //	comboText.gameObject.transform.Rotate(new Vector3(0,0,-180));
        //	shadowText.gameObject.transform.Rotate(new Vector3(0,0,-180));
        //	multiplierText.gameObject.transform.Rotate(new Vector3(0,0,-180));
        //}
    }

    // Use this for initialization
    void Update() {
        if (alive) WalkAnimation();
        if (shake) ShakeAnimation();
    }


    // Update is called once per frame
    public void Hit() {
        alive = false;
        meshRenderer.material.mainTexture = hitFrame[Random.Range(0, hitFrame.Length)];
        AS.PlayOneShot(cry[Random.Range(0, cry.Length)]);
        orgPos = transform.position;
        shake = true;
        StartCoroutine("KillMe");
    }

    public void HideAndDestroy() {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        Destroy(transform.parent.gameObject, 2);
    }

    IEnumerator KillMe() {
        Instantiate(exploPrefab, pivotPoint.transform.position, Quaternion.identity);
        boxCollider.enabled = false;
        Combo();
        yield return new WaitForSeconds(2);
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void Combo() {
        int typeOfCombo = gameController.GetScoreCombo();
        if (typeOfCombo < 11) {
            switch (typeOfCombo) {
                default:
                scoreToAdd = 1;
                break;
                case 2:
                DisplayCombo(Color.magenta, "EXCELLENT", 2, combo1);
                scoreToAdd = 2;
                break;
                case 3:
                case 4:
                case 5:
                DisplayCombo(Color.yellow, "OUTSTANDING", 3, combo2);
                scoreToAdd = 3;
                break;
                case 6:
                case 7:
                case 8:
                case 9:
                DisplayCombo(Color.blue, "COMBO", 4, combo3);
                scoreToAdd = 4;
                break;
                case 10:
                DisplayCombo(Color.red, "ULTRA COMBO", 5, combo4);
                scoreToAdd = 5;
                break;
            }
        }
        else {
            DisplayCombo(Color.red, "ULTRA COMBO", 5, null);
            scoreToAdd = 7;
        }
        GameController.control.AddScorePoints(scoreToAdd);
    }

    public void DisplayCombo(Color color, string text, int score, AudioClip clip) {
        comboText.color = color;
        comboText.text = text;
        shadowText.text = text;
        multiplierText.text = GetBonusText(score);

        if (clip != null)
            AS.PlayOneShot(clip);
    }

    string GetBonusText(int scoreToAdd) {
        return "Bonus" + System.Environment.NewLine + "(+" + (scoreToAdd - 1).ToString() + ")";
    }

    void WalkAnimation() {
        time += Time.deltaTime;
        if (time >= period) {
            time -= period;
            if (imgWereAt < frames.Length - 1) {
                imgWereAt++;
                meshRenderer.material.mainTexture = frames[imgWereAt];
            }
            else {
                imgWereAt = 0;
                meshRenderer.material.mainTexture = frames[imgWereAt];
            }
        }
    }

    void ShakeAnimation() {
        float shk = Mathf.Sin(Time.deltaTime * speed) * amount;
        transform.position = orgPos + new Vector3(shk, shk, 0);
    }

}
