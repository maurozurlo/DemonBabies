using System.Collections;
using UnityEngine;

public abstract class MovingTarget : MonoBehaviour
{
    AudioSource AS;
    float toZ = 4.5f;
    float toY = -2.17f;
    Vector2 boundsX = new Vector2(1.26f, -2.69f);
    Vector2 boundsZ = new Vector2(-20, -17);
    float speed = 4;

    public enum animState
    {
        idle, rising, walking
    }

    public animState AnimState = animState.idle;

    public void ChangeGround(float value)
    {
        toY = value;
    }

    private void Start()
    {
        AS = gameObject.AddComponent<AudioSource>();
        AS.volume = GameContext.control.GetVolume("fx");
        speed = GameController.control.GetEnemySpeed();
        Init();
        transform.position = new Vector3(Random.Range(boundsX.x, boundsX.y), -3.53f, Random.Range(boundsZ.x, boundsZ.y));
        AnimState = animState.rising;
    }

    void RiseUp()
    {
        if (AnimState != animState.rising) return;
        if(transform.position.y > toY) {
            AnimState = animState.walking;
            StartCoroutine("PlaySound");
            return;
        } 
        transform.position += new Vector3(0, 5, 0) * Time.deltaTime;
        
    }

    void Walk()
    {
        if (AnimState != animState.walking) return;
        if (transform.position.z > toZ) return;
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
    }

    void Update()
    {
        RiseUp();
        Walk();
    }

    public void StopMoving()
    {
        AS.Pause();
        AnimState = animState.idle;
    }

    public void Restart()
    {
        AnimState = animState.walking;
    }

    public abstract IEnumerator PlaySound();
    public abstract void Init();

    public void PlayOneShot(AudioClip clip, bool overrideSound = false)
    {
        if (!overrideSound && AS.isPlaying) return;
        AS.Stop();
        AS.PlayOneShot(clip);
    }

    public void PlaySoundInLoop(AudioClip clip)
    {
        AS.loop = true;
        AS.clip = clip;
        AS.Play();
    }

    public bool IsPlaying()
    {
        return AS.isPlaying;
    }

    public void StopAudio()
    {
        AS.Stop();
    }
}
