using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace GameManagers
{
    public class GameMan : MonoBehaviour
    {
        public int score;
        [SerializeField]
        private TMP_Text scoreText;
        [SerializeField] 
        private TMP_Text deathScoreText;
        
        public static GameMan Instance;
        [SerializeField]
        private GameObject DeathScreen;
        [SerializeField]
        private GameObject InGameUI;

        public Vector3 Playerstart;
        
        // Start is called before the first frame update
        void Start()
        {
            score = 0;
            if (Instance == null)
            {
                Instance = this;
            }
            Object.DontDestroyOnLoad(this);
        }

        public void IncrementScore(float _distanceTravelled)
        {
            if (score < _distanceTravelled)
            {
                score = (int)_distanceTravelled;
            }
            
            
            
            scoreText.text = score.ToString() + "m";
        }

        public void PlayerHasDied()
        {
            DeathScreen.SetActive(true);
            InGameUI.SetActive(false);
            deathScoreText.text = score.ToString() + "m";
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            score = 0;
        }
    }
}

