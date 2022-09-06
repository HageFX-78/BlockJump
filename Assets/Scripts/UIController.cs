using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverUi;

    [SerializeField] TextMeshProUGUI highscore;
    [SerializeField] TextMeshProUGUI myscore;
    [SerializeField] TextMeshProUGUI coinsearned;

    [SerializeField] GameObject boosted;
    [SerializeField] GameObject hardmText;

    [SerializeField] Slider pSlider;
    AudioManager am;
    private void Awake()
    {
        am = AudioManager.amInstance;
    }
    private void Start()
    {      
        Time.timeScale = 1;
        volumeCheck();
    }
    public void pauseScene()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        am.plySF("btnpress");
    }

    public void unpauseScene()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        am.plySF("btnpress");
    }

    public void reloadScene()
    {
        am.plySF("btnpress");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnToMainMenu()
    {
        am.stopBGM("gameBGM");
        am.plySF("btnpress");
        PlayerPrefs.SetInt("hmActive", 0);
        SceneManager.LoadSceneAsync("MainMenu");
        
    }
    public void volumeCheck()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            pSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
    }
    public void sliderChange()
    {
        float newValue = pSlider.value;
        AudioListener.volume = newValue;
        PlayerPrefs.SetFloat("volume", newValue);
    }
    public void gameOver(int coinAmt, int plyscore)
    {
        am.stopBGM("gameBGM");
        Time.timeScale = 0;
        gameOverUi.SetActive(true);
        if (PlayerPrefs.HasKey("highscore"))
        {
            if(PlayerPrefs.GetInt("highscore")<plyscore)
            {
                PlayerPrefs.SetInt("highscore", plyscore);
            }
        }
        else
        {
            PlayerPrefs.SetInt("highscore", plyscore);
        }       
        if(PlayerPrefs.HasKey("Coins"))
        {
            int newCoinCount = PlayerPrefs.GetInt("Coins") + coinAmt;
            PlayerPrefs.SetInt("Coins", newCoinCount);
        }
        else
        {
            PlayerPrefs.SetInt("Coins", coinAmt);
        }
        highscore.text = PlayerPrefs.GetInt("highscore").ToString("n0");
        myscore.text = plyscore.ToString("n0");
        coinsearned.text = $"Coins Earned:\n{coinAmt}\n<color=#CDCDCD><size=80%>(Total: {PlayerPrefs.GetInt("Coins")})";


        //Text displays
        if (PlayerPrefs.GetInt("item0")==1)
        {
            boosted.SetActive(true);
        }
        else
        {
            boosted.SetActive(false);
        }
        PlayerPrefs.SetInt("item0", 0);
        if (PlayerPrefs.GetInt("hmActive") == 1)
        {
            hardmText.SetActive(true);
        }
        else
        {
            hardmText.SetActive(false);
        }

        PlayerPrefs.SetInt("returned", 1);
    }
}
