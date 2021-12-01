using UnityEngine;

public class LSDFX : MonoBehaviour {
	public float min, max, speed;
	private float t;
	Material material;	
	void Start() {
		material = GetComponent<UnityEngine.UI.Image>().material;
	}
	void Update() {
		t += speed * Time.deltaTime;
		float actualVal = Mathf.Lerp(min, max, t);
		material.mainTextureOffset = new Vector2(actualVal, 1);
		if (t > 1.0f) {
			float temp = max;
			max = min;
			min = temp;
			t = 0;
		}
	}
}