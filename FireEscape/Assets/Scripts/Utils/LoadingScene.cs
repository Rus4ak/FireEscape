using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public void Loading(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
