using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneWithProgress : MonoBehaviour
{
    public Image progressBar;

    public LoadSceneMode loadSceneMode = LoadSceneMode.Additive;
	public ThreadPriority loadThreadPriority;

	// If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
	public AudioListener audioListener;

	public float waitOnLoadEnd = 0.25f;

	AsyncOperation operation;
    Scene currentScene;

	public void LoadSceneNow(string sceneToLoad)
	{
		Application.backgroundLoadingPriority = ThreadPriority.High;
		currentScene = SceneManager.GetActiveScene();
		StartCoroutine(LoadAsync(sceneToLoad));
	}

	private IEnumerator LoadAsync(string levelNum)
	{
		progressBar.fillAmount = 0f;

		yield return null;

		StartOperation(levelNum);

		float lastProgress = 0f;

		// operation does not auto-activate scene, so it's stuck at 0.9
		while (DoneLoading() == false)
		{
			yield return null;

			if (Mathf.Approximately(operation.progress, lastProgress) == false)
			{
				progressBar.fillAmount = operation.progress;
				lastProgress = operation.progress;
			}
		}

		if (loadSceneMode == LoadSceneMode.Additive)
			audioListener.enabled = false;

		yield return new WaitForSeconds(waitOnLoadEnd);
		
		if (loadSceneMode == LoadSceneMode.Additive)
			SceneManager.UnloadSceneAsync(currentScene.name);
		else
			operation.allowSceneActivation = true;
	}

	private void StartOperation(string levelNum)
	{
		Application.backgroundLoadingPriority = loadThreadPriority;
		operation = SceneManager.LoadSceneAsync(levelNum, loadSceneMode);


		if (loadSceneMode == LoadSceneMode.Single)
			operation.allowSceneActivation = false;
	}

	private bool DoneLoading()
	{
		return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f);
	}
}
