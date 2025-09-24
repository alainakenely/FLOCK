using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Home Screen");
    }
}