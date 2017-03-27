using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {

    public static GameSceneManager instance;

    private int currentSceneIndex;
   

	void Start () {
        if (instance != null)
        {
            Debug.Log("More Than One GameSceneManager");
            return;
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadRandomLevel()
    {
        SceneManager.LoadScene(Random.Range(1, 6));
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
