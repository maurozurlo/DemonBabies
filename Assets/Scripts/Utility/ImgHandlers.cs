using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgHandlers : MonoBehaviour {
		Image img;
		RawImage img2;
		Text txt;
		Shadow shd;
		public bool runAtStart = true;
		public float delay;
		public AnimationCurve OpenOutCurve;
		float vrtxAngle;
		public float MaxYDim = 300;
		public float speedMultiplier;
		Color col;
		Color col2;
		public bool goBackOpaque = true;
		//esto que viene es un bardo, pero es necesario...
		public enum MyEnum
		{
			Vortex, OpacityRawImage, OpacityImage, Text, TextWShadow, SkinnedMeshToTransparent, SkinnedMeshToCutOut, SkinnedMeshToFade, MeshRendererToFade
		}

		public MyEnum varToControl;
		// Use this for initialization
		void Start () {
		if (runAtStart) {
			SetBaseValues ();
			StartCoroutine (_OpenOut ());
		}
		}


		void SetBaseValues(){
		switch (varToControl) {
		case MyEnum.OpacityRawImage:
			col = this.GetComponent<RawImage> ().material.color;
			break;
		case MyEnum.OpacityImage:
			col = this.GetComponent<Image> ().color;
			break;
		case MyEnum.Text:
			col = this.GetComponent<Text> ().color;
			break;
		case MyEnum.TextWShadow:
			col = this.GetComponent<Text> ().color;
			col2 = this.GetComponent<Shadow> ().effectColor;
			break;
		case MyEnum.SkinnedMeshToTransparent:
			col = this.GetComponent<SkinnedMeshRenderer> ().materials[0].color;
			foreach (Material item in this.GetComponent<SkinnedMeshRenderer> ().materials) {
				StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Transparent);
			}
			break;
		case MyEnum.SkinnedMeshToCutOut:
			col = this.GetComponent<SkinnedMeshRenderer> ().materials[0].color;
			foreach (Material item in this.GetComponent<SkinnedMeshRenderer> ().materials) {
				StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Cutout);
			}
			break;
		case MyEnum.SkinnedMeshToFade:
			col = this.GetComponent<SkinnedMeshRenderer> ().materials[0].color;
			foreach (Material item in this.GetComponent<SkinnedMeshRenderer> ().materials) {
				StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Fade);
			}
			break;
		case MyEnum.MeshRendererToFade:
			col = this.GetComponent<MeshRenderer> ().materials[0].color;
			foreach (Material item in this.GetComponent<MeshRenderer> ().materials) {
				StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Fade);
			}
			break;
		}
		}

		IEnumerator _OpenOut(){
		yield return new WaitForSeconds (delay);
		//SetBaseValues
		SetBaseValues ();
			float curveTime = 0f;
			float curveAmount = OpenOutCurve.Evaluate (curveTime);

			while (curveAmount < 1.0f) {
				curveTime += Time.deltaTime * speedMultiplier;
				curveAmount = OpenOutCurve.Evaluate (curveTime);
				vrtxAngle = curveAmount*MaxYDim;
				curveAnimHandler ();
				yield return null;
			}
			//Funcion que los vuelva a los skinned meshes a opaque...
		if (goBackOpaque) {
			GoBackToOpaque ();
		}
		}

		public void replayAnim(){
			StartCoroutine (_OpenOut ());
		}

	public void curveAnimHandler(){
		switch (varToControl) {
		case MyEnum.OpacityRawImage:
			this.GetComponent<RawImage> ().material.color = new Color(col.r,col.g,col.b, vrtxAngle);
			break;
		case MyEnum.OpacityImage:
			this.GetComponent<Image> ().color = new Color(col.r,col.g,col.b, vrtxAngle);
			break;
		case MyEnum.Text:
		case MyEnum.TextWShadow:
			this.GetComponent<Text> ().color = new Color (col.r, col.g, col.b, vrtxAngle);
			if (this.GetComponent<Shadow> () != null) {
				this.GetComponent<Shadow> ().effectColor = new Color (col2.r, col2.g, col2.b, vrtxAngle);
			}
			break;
		case MyEnum.SkinnedMeshToTransparent:
		case MyEnum.SkinnedMeshToCutOut:
		case MyEnum.SkinnedMeshToFade:
			foreach (Material item in this.GetComponent<SkinnedMeshRenderer> ().materials) {
				item.color = new Color (col.r, col.g, col.b, vrtxAngle);
			}
			break;
		case MyEnum.MeshRendererToFade:
			foreach (Material item in this.GetComponent<MeshRenderer> ().materials) {
				item.color = new Color (col.r, col.g, col.b, vrtxAngle);
			}
			break;
		}
	}


	void GoBackToOpaque(){
		switch (varToControl) {
		case MyEnum.SkinnedMeshToTransparent:
		case MyEnum.SkinnedMeshToCutOut:
			foreach (Material item in this.GetComponent<SkinnedMeshRenderer> ().materials) {
				StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Opaque);
			}
			break;
		case MyEnum.MeshRendererToFade:
		foreach (Material item in this.GetComponent<MeshRenderer> ().materials) {
			StandardShaderUtils.ChangeRenderMode (item, StandardShaderUtils.BlendMode.Opaque);
		}
		break;
	}

	}

}
