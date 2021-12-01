using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource AS;
    public static MusicController control;
    // Start is called before the first frame update
    void Start()
    {
        if (!control) control = this;
        else Destroy(this);
        GameConfig config = GameContext.control.GetGameConfig();
        
        AS = GetComponent<AudioSource>();
        if (config.music) AS.clip = config.music;
        else if (!config.music) Debug.LogWarning("Level has no music");
        AS.Play();
    }

    public void ChangePitch(float pitch)
    {
        AS.pitch = pitch;
    }

    public void ReturnPitchToNormal()
    {
        AS.pitch = 1;
    }
}
