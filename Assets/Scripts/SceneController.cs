using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mtaka
{
    /// <summary>
    /// 場景控制
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}