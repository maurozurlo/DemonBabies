using System.Collections;
using UnityEngine;

public abstract class MovingTarget : MonoBehaviour
{
    AudioSource AS;
    float toZ = 4.5f;
    float toY = -2.72f;
    Vector2 boundsX = new Vector2(0, 3.33f);
    Vector2 boundsZ = new Vector2(-20, -17);
    float speed = 4;

    public void ChangeGround(float value) {
        toY = value;
    }

    private void Start() {
        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = GameContext.control.GetVolume("fx");
        speed = GameController.control.GetEnemySpeed();
        Init();
        transform.position = new Vector3(Random.Range(boundsX.x, boundsX.y), -3.53f, Random.Range(boundsZ.x, boundsZ.y));
        StartCoroutine("RiseUp");
    }
    IEnumerator RiseUp() // Replace with an animation...
    {
        while (transform.position.y < toY) {
            transform.position += new Vector3(0, .04f, 0);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine("StartWalking");
        StartCoroutine("PlaySound");
    }

    IEnumerator StartWalking() // Get baby speed from game controller
    {
        while (transform.position.z < toZ) {
            transform.position += new Vector3(0, 0, speed / 100);
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopMoving() {
        StopAllCoroutines();
        AS.Pause();
    }

    public void Restart() {
        StartCoroutine("StartWalking");
        StartCoroutine("PlaySound");
    }

    public abstract IEnumerator PlaySound();
    public abstract void Init();

    public void PlayOneShot(AudioClip clip, bool overrideSound = false) {
        if (!overrideSound && AS.isPlaying) return;
        AS.Stop();
        AS.PlayOneShot(clip);
    }

    public void PlaySoundInLoop(AudioClip clip) {
        AS.loop = true;
        AS.clip = clip;
        AS.Play();
    }

    public bool IsPlaying() {
        return AS.isPlaying;
    }

    public void StopAudio() {
        AS.Stop();
    }
}
