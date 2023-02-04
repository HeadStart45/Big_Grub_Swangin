using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers {

    public class MainMenu : MonoBehaviour {
        public void PlayGame() {
            Debug.Log("Play Game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame() {
            Debug.Log("Quit Game");
            Application.Quit();
        }
    }
}