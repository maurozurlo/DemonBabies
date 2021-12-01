using UnityEngine;

public class GameInit : MonoBehaviour {
    public GameObject Grass;
    private GameConfig config;
    public static GameInit control;

    void Awake() {
        config = GameContext.control.GetGameConfig();
        //Singleton Pattern
        if (!control) control = this;
        else DestroyImmediate(this);
        // TODO: Probably some sort of fade

        Grass.GetComponent<MeshRenderer>().material = config.grass;
        RenderSettings.skybox = config.skybox;
        Camera.main.transform.position = config.cameraPosition;
        if (config.isUpsideDown) UpsideDownWorld();

        if(config.hasParticleSpawner && config.particleSpawner) {
            Instantiate(config.particleSpawner, config.particleSpawnerPosition, Quaternion.Euler(new Vector3(180, 180, 90)));
        }

        if (config.hasFog) {
            RenderSettings.fog = true;
            RenderSettings.fogMode = config.fogMode;
            RenderSettings.fogColor = config.fogColor;
            RenderSettings.fogDensity = config.fogDensity;
        }

        if (config.clouds) {
            Instantiate(config.clouds, new Vector3(-0.3f, -0.272f, -21.8f), Quaternion.Euler(new Vector3(0,90,0)));
        }
    }

    public GameConfig GetConfig() {
        return config;
    }

    void UpsideDownWorld() {
        Camera.main.transform.Rotate(new Vector3(0, 0, -180));
    }
}
