using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pause;

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
        pause.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToMenu()
    {
		Time.timeScale = 1f;
		SceneManager.LoadSceneAsync(0);
    }

    //!!!TEST FUNCTION! DO NOT USE IN REAL PROJECT!!!
    public void GatherAndProcessInput()
    {
        //Debug.Log("Frame passed!");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause.active)
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
