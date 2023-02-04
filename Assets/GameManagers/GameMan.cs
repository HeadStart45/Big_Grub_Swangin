using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    public class GameMan : MonoBehaviour
    {
        //private static string hUrl = "http://dreamlo.com/lb/-3VGZRffn0GynG_bvLq3NgUNbGP26PDE6_pivNC-b6iA";
        //private static string pCode = "-3VGZRffn0GynG_bvLq3NgUNbGP26PDE6_pivNC-b6iA";
        //private static string pubCode = "63ddd8ab8f40bb08f4c33072";
        public int score;
        [SerializeField]
        private TMP_Text scoreText;

        
        public static GameMan Instance;
        [SerializeField]
        private GameObject DeathScreen;
        [SerializeField]
        private GameObject InGameUI;
        [SerializeField]
        private GameObject HighscoreSystem;

        [Header("TEST MODE")]
        [SerializeField]
        public bool Testing;
        
        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            if (Testing)
            {
                scoreText.text = "TESTING";
            }

        }

        public void IncrementScore(float _distanceTravelled)
        {
            if (!Testing)
            {
                if (score < _distanceTravelled)
                {
                    score = (int)_distanceTravelled;
                }
                scoreText.text = score.ToString() + "m";
            }

            
        }

        public void PlayerHasDied()
        {
            if (!Testing)
            {
                DeathScreen.SetActive(true);
                InGameUI.SetActive(false);
                Instantiate(HighscoreSystem);
            }
        }

        public void Restart()
        {
            if (!Testing)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                score = 0;
            }
        }
    }
}

