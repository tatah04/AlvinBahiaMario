using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TimeAndScoreTracker : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float maxTime;
    float totalTime;
    [SerializeField] RectTransform PlayUI;
    [SerializeField] RectTransform PlayButton;
    [SerializeField] RectTransform GameOverUI;
    [SerializeField] RectTransform ActualGameOverUI;
    // [SerializeField] int playerLives;
    [SerializeField] int maxplayerLives;
    [SerializeField] TextMeshProUGUI livesStr;
    [SerializeField] int totalScore;
    [SerializeField] TextMeshProUGUI rank1Name;
    [SerializeField] TextMeshProUGUI rank1Score;
    [SerializeField] TextMeshProUGUI rank2Name;
    [SerializeField] TextMeshProUGUI rank2Score;
    [SerializeField] TextMeshProUGUI rank3Name;
    [SerializeField] TextMeshProUGUI rank3Score;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI congrats;
    public int Coin;
    public int enemiesKilled;
    public bool isGameOver;
    [SerializeField] Image pauseImage;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite playSprite;
    [SerializeField] bool isPaused;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject pauseTextObj;
    void Awake()
    {
        StopAllCoroutines();
        checkFirstTime();
        isGameOver = false;
        Time.timeScale = 0;
        if (Stats.lives == 0)
        {
            Stats.lives = maxplayerLives;
            Stats.coinCount = 0;
            Stats.score = 0;
        }
        else
        {
            Coin = Stats.coinCount;
        }
       
        livesStr.text = Stats.lives.ToString();
        isPaused = false;
    }
    private void FixedUpdate()
    {
        totalScore = Stats.score + ((int)totalTime + (Coin * 10) + (enemiesKilled * 20));
        scoreText.text = "Score:" + totalScore.ToString();
    }



    public void startGame()
    {
        SoundManager.PlaySfx(11);
        PlayButton.gameObject.SetActive(false);
        PlayUI.gameObject.SetActive(false);
        Time.timeScale = 1;
        pauseImage.gameObject.SetActive(true);
        StartCoroutine(timerCo());

      //  Debug.Log(PlayerPrefs.GetString("Rank1str") + ":" + PlayerPrefs.GetInt("Rank1").ToString());
       // Debug.Log(PlayerPrefs.GetString("Rank2str") + ":" + PlayerPrefs.GetInt("Rank2").ToString());
      //  Debug.Log(PlayerPrefs.GetString("Rank3str") + ":" + PlayerPrefs.GetInt("Rank3").ToString());
    }

    public void GameOver()
    {
        Stats.lives--;
        if (Stats.lives == 0)
        {
            ActualGameOver();
            return;
        }
        SoundManager.soundManagerInstance.bgmSource.loop = false;
        SoundManager.PlayBgm(5);
        Time.timeScale = 0;
        GameOverUI.gameObject.SetActive(true);
        PlayUI.gameObject.SetActive(true);
        Stats.lastState = PlayerState.small;
    }
    public void ActualGameOver()
    {
        SoundManager.soundManagerInstance.bgmSource.loop = false;
        SoundManager.PlayBgm(5);
        Time.timeScale = 0;
        PlayUI.gameObject.SetActive(true);
        ActualGameOverUI.gameObject.SetActive(true);
        CheckScores();
        Stats.lastState = PlayerState.small;
    }

    public void CheckScores()
    {
        rank1Name.text = PlayerPrefs.GetString("Rank1str");
        rank2Name.text = PlayerPrefs.GetString("Rank2str");
        rank3Name.text = PlayerPrefs.GetString("Rank3str");
        rank1Score.text = PlayerPrefs.GetInt("Rank1").ToString();
        rank2Score.text = PlayerPrefs.GetInt("Rank2").ToString();
        rank3Score.text = PlayerPrefs.GetInt("Rank3").ToString();
      //  Debug.Log(Stats.score);
        if (Stats.score > PlayerPrefs.GetInt("Rank1"))
        {
            askName();
            congrats.text = "You got Rank1!";
        }
        else if (Stats.score > PlayerPrefs.GetInt("Rank2"))
        {
            askName();
            congrats.text = "You got Rank2!";
        }
        else if (Stats.score > PlayerPrefs.GetInt("Rank3"))
        {
            askName();
            congrats.text = "You got Rank3!";
        }
        else
        {
            inputField.gameObject.SetActive(false);
            congrats.text = "Game Over!";
        }

    }
    public void askName()
    {
        inputField.gameObject.SetActive(true);
    }
    public void SubmitScore()
    {
        RecordScore();

        if (Stats.score > PlayerPrefs.GetInt("Rank1"))
        {
            PlayerPrefs.SetInt("Rank2", PlayerPrefs.GetInt("Rank1"));
            PlayerPrefs.SetString("Rank2str", PlayerPrefs.GetString("Rank1str"));
            rank2Name.text = PlayerPrefs.GetString("Rank2str");
            rank2Score.text = PlayerPrefs.GetInt("Rank2").ToString();

            PlayerPrefs.SetInt("Rank1", Stats.score);
            PlayerPrefs.SetString("Rank1str", inputField.text);
            rank1Name.text = PlayerPrefs.GetString("Rank1str");
            rank1Score.text = PlayerPrefs.GetInt("Rank1").ToString();
        }
        else if (Stats.score > PlayerPrefs.GetInt("Rank2"))
        {
            PlayerPrefs.SetInt("Rank3", PlayerPrefs.GetInt("Rank2"));
            PlayerPrefs.SetString("Rank3str", PlayerPrefs.GetString("Rank2str"));
            rank3Name.text = PlayerPrefs.GetString("Rank3str");
            rank3Score.text = PlayerPrefs.GetInt("Rank3").ToString();

            PlayerPrefs.SetInt("Rank2", Stats.score);
            PlayerPrefs.SetString("Rank2str", inputField.text);
            rank2Name.text = PlayerPrefs.GetString("Rank2str");
            rank2Score.text = PlayerPrefs.GetInt("Rank2").ToString();
        }
        else if (Stats.score > PlayerPrefs.GetInt("Rank3"))
        {

            PlayerPrefs.SetInt("Rank3", Stats.score);
            PlayerPrefs.SetString("Rank3str", inputField.text);
            rank3Name.text = PlayerPrefs.GetString("Rank3str");
            rank3Score.text = PlayerPrefs.GetInt("Rank3").ToString();
        }
        inputField.gameObject.SetActive(false);
    }

    public void checkFirstTime()
    {
        if (PlayerPrefs.HasKey("Rank1") == false &&
            PlayerPrefs.HasKey("Rank2") == false &&
            PlayerPrefs.HasKey("Rank3") == false)
        {
            PlayerPrefs.SetInt("Rank1", 0);
            PlayerPrefs.SetInt("Rank2", 0);
            PlayerPrefs.SetInt("Rank3", 0);
            PlayerPrefs.SetString("Rank1str", "Rank1");
            PlayerPrefs.SetString("Rank2str", "Rank2");
            PlayerPrefs.SetString("Rank3str", "Rank3");
        }
    }

    public void RecordScore()
    {
        totalScore = (int)totalTime + (Coin * 10) + (enemiesKilled * 20);
        Stats.score = Stats.score+ totalScore;
        Stats.coinCount = Coin;
    }

    public void Retry()
    {
        SoundManager.PlaySfx(11);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RetryFromFirst()
    {
        SoundManager.PlaySfx(11);
        SceneManager.LoadScene(0);
    }

    public IEnumerator timerCo()
    {
        totalTime = maxTime;
        while (totalTime > 0)
        {
            totalTime--;
            yield return new WaitForSeconds(1);
            timeText.text = totalTime.ToString();
            if (totalTime == 15)
            {
                SoundManager.PlayBgm(15);
            }
            if (totalTime == 0)
            {
                GameOver();
            }
        }
    }
    public void wonLevel()
    {
        StartCoroutine(flagCo());
    }
    public void checkNextLevel()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex;
        if (sceneNum != 2)
        {
            SceneManager.LoadScene(sceneNum + 1);
        }
        else
        {
            ActualGameOver();
        }
        SoundManager.PlayBgm(16);
        Stats.score = totalScore;
        Stats.coinCount = Coin;
  
    }
    public IEnumerator flagCo()
    {
        checkNextLevel();
        yield return new WaitForSeconds(5f);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseImage.sprite = playSprite;
            PlayUI.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
            Time.timeScale = 0;
            pauseTextObj.SetActive(true);
          //  pauseImage.gameObject.SetActive(true);
        }
        else
        {
            pauseImage.sprite = pauseSprite;
            PlayUI.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(false);
         //   pauseImage.gameObject.SetActive(false);
            Time.timeScale = 1;
            pauseTextObj.SetActive(false);

        }
    }

}
