using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[Header("Screens")]
	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private GameObject settingsScreen;
	[Header("Audio")]	
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private List<AudioParams> audioParameters;

	[Serializable]
	public struct AudioParams
	{
		public string groupName;
		public Slider slider;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public async void NewGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadSceneAsync(1);
	}

	public void Pause()
	{
		pauseScreen.SetActive(true);
		Time.timeScale = 0;
	}

	public void Resume()
	{
		pauseScreen.SetActive(false);
		Time.timeScale = 1f;
	}

	public void ToMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadSceneAsync(0);
	}

	public void Settings()
	{
		if (settingsScreen.active)
		{
			settingsScreen.SetActive(false);
			return;
		}
		settingsScreen.SetActive(true);
	}

	public void ChangeAudioVolume(int index)
	{
		AudioParams parameters = audioParameters[index];
		if (parameters.slider.value == 0)
		{
			audioMixer.SetFloat(parameters.groupName, -80f);
			return;
		}
		audioMixer.SetFloat(parameters.groupName, (-20 + (parameters.slider.value * 40)));
	}

	//!!!TEST FUNCTION! DO NOT USE IN REAL PROJECT!!!
	public void GatherAndProcessInput()
	{
		//Debug.Log("Frame passed!");
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!pauseScreen.active)
			{
				Pause();
			}
			else
			{
				Resume();
			}
		}
	}

	private void Update()
	{
		GatherAndProcessInput();
	}

}
