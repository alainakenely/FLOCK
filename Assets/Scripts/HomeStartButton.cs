using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeStartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Mountains");
    }
}