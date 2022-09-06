using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [Header("Btn and Text Refs")]
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI item0txt;
    [SerializeField] TextMeshProUGUI item1txt;
    [SerializeField] Button item0;
    [SerializeField] Button item1;
    [SerializeField] Image loadingBar;
    [Header("Game Object Refs")]
    [SerializeField] GameObject hardModeBTN;
    [SerializeField] GameObject splashScreen;
    [SerializeField] GameObject menuBig;
    [SerializeField] GameObject menuSelect;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject shopMenu;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] GameObject guideScreen;
    [SerializeField] Slider mainSlider;
    AudioManager am;
    

    Dictionary<int, int> itemRef = new Dictionary<int, int>();

    void Start()
    {
        am = AudioManager.amInstance;
        Time.timeScale = 1;

        PlayerPrefs.SetInt("returned", 0);
        if (PlayerPrefs.HasKey("returned")) { if (PlayerPrefs.GetInt("returned") == 1) {
                menuBig.SetActive(true);
                splashScreen.SetActive(false);
                PlayerPrefs.SetInt("returned", 0);
            } 
            else
            {
                menuBig.SetActive(false);
                splashScreen.SetActive(true);
            }
        } 
        updateCoin(0);
        itemRef.Add(0, 100); itemRef.Add(1, 1000);
        itemCheck(0);itemCheck(1);//Should be in loop if there were more items

        if (!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.SetInt("highscore", 0);
        }
        if(PlayerPrefs.GetInt("item1")==1)
        {
            hardModeBTN.SetActive(true);
        }   
        else
        {
            hardModeBTN.SetActive(false);
            
        }
        PlayerPrefs.SetInt("hmActive", 0);


        //MUSIC
        volumeCheck();
        am.plyBGM("menuBGM");
        offGuideScreen();
    }
    //-------------------------------------------------------END of START----------------------------------------------------------------------
    public void updateCoin(int amt)
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            int newAmt = PlayerPrefs.GetInt("Coins") + amt;
            PlayerPrefs.SetInt("Coins", newAmt);
            
        }
        else
        {
            PlayerPrefs.SetInt("Coins", amt);
        }
        coins.text = $"Coins:{PlayerPrefs.GetInt("Coins")}";
    }

    public void itemCheck(int itemInd)
    {
        if (PlayerPrefs.HasKey($"item{itemInd}"))
        {
            if (PlayerPrefs.GetInt($"item{itemInd}")==1)
            {
                switch(itemInd)
                {
                    case 0:
                        item0.interactable = false;
                        item0txt.text = "Sold";
                        break;
                    case 1:
                        item1.interactable = false;
                        item1txt.text = "Sold";
                        break;
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt($"item{itemInd}", 0);//0 means not bought
        }
    }
    public void volumeCheck()
    {
        if(PlayerPrefs.HasKey("volume"))
        {
            mainSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
    }
    //-------------------------------------------------------------Menu Button Functions------------------------------------------
    public void exitSplash()
    {
        splashScreen.SetActive(false);
        menuBig.SetActive(true);
        menuSelect.SetActive(true);
        shopMenu.SetActive(false);
        settings.SetActive(false);

        am.plySF("startSplash");
    }
    public void menuToGame()
    {
        menuSelect.SetActive(false);
        loadingScreen.SetActive(true);
        coins.gameObject.SetActive(false);
        StartCoroutine(loadMainScene());

        am.plySF("btnpress");
        
    }
    public void menuToHardGame()
    {
        PlayerPrefs.SetInt("hmActive", 1);
        menuSelect.SetActive(false);
        loadingScreen.SetActive(true);
        coins.gameObject.SetActive(false);
        StartCoroutine(loadMainScene());

        am.plySF("btnpress");
    }
    public void menuToShop()
    {
        menuSelect.SetActive(false);
        shopMenu.SetActive(true);
        am.plySF("btnpress");
    }
    public void menuToSettings()
    {
        menuSelect.SetActive(false);
        settings.SetActive(true);
        am.plySF("btnpress");
    }

    public void shopToMenu()
    {
        menuSelect.SetActive(true);
        shopMenu.SetActive(false);
        am.plySF("btnpress");
    }
    public void settingsToMenu()
    {
        menuSelect.SetActive(true);
        settings.SetActive(false);
        am.plySF("btnpress");
    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void clearAllData()
    {
        PlayerPrefs.DeleteAll();
        updateCoin(0);
    }
    public void sliderChange()
    {
        float newValue = mainSlider.value;
        AudioListener.volume = newValue;
        PlayerPrefs.SetFloat("volume", newValue);
    }
    public void onGuideScreen()
    {
        guideScreen.SetActive(true);
        am.plySF("btnpress");
    }
    public void offGuideScreen()
    {
        guideScreen.SetActive(false);
        am.plySF("btnpress");
    }
    //-------------------------------------------------------------Others------------------------------------------
    IEnumerator loadMainScene()
    {
        am.stopBGM("menuBGM");
        AsyncOperation loadlevel = SceneManager.LoadSceneAsync("MainScene");
        while (!loadlevel.isDone)
        {
            loadingBar.fillAmount = Mathf.Clamp01(loadlevel.progress/.9f);
            yield return null;
        }
        
    }

    public void buyItem(int itemIndex)
    {
        if (PlayerPrefs.GetInt("Coins")>=itemRef[itemIndex])
        {
            PlayerPrefs.SetInt($"item{itemIndex}", 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - itemRef[itemIndex]);
            itemCheck(itemIndex);
            am.plySF("purchase");
        }
    }
}
