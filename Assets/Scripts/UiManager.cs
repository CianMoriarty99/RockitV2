using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UiManager : MonoBehaviour
{
    public Vector2 selectedLevel;
    public TMP_Text levelNameText, levelObjectiveText;
    public float fadeTime;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public Transform startingPos, targetPos;

    private bool settingsPanelActive;

    private static UiManager _instance;
    public static UiManager Instance { get { return _instance; } }

    public GameObject startButton, objectivesPanel, settingsPanel;

    //Camera stuff
    public GameObject currentPlayer;

    public Camera mainCamera;
    private float camMotionSpeed = 5f;
    private float camRotationSpeed = 50f;
    private Vector3 cameraDefaultPos = new Vector3(0, 0, -20); 

    private readonly string[,] levelNameTexts = new string[5, 5] 
    {
        {"Ground Zero","Obstacle","Double Trouble","Duck and Weave","Gauntlet"},
        {"L.A.S.E.R","Spectral Assault","Laser Focus","Photon Maze","Lethal Luminance"}, 
        {"Snake?","Metal Cog Hard","Serpentine!","Sidewinder","Constrictor Crossway"},
        {"King of the Thrill","Crestfall","Rocky Terrain","Peak Difficulty","Submit to Summit"},
        {"Elite Perilous","Rattled!","Death Mountain","The Pit","Everest"} 
    };

    //LOCALISATION - English, Russian, Chinese T, Chinese S, German, Brazilian, French, Polish, Spanish  

    public int currentLanguage; //0 - 8
    public TMP_Text languageTmpText, unlockTmpText, unlockSubTmpText, bestTimeTmpText, startTmpText, volumeTmpText, quitTmpText;

    public TMP_FontAsset englishFont, latinFont, russianFont, chineseTraditionalFont, chineseSimplifiedFont;

    private readonly string[] languageText = new string [9]
    {
        "ENGLISH", "РУССКИЙ", "繁体中文", "简体中文", "DEUTSCH", "PORTUGUÊS (BR)", "FRANÇAIS", "POLSKI", "ESPAÑOL"
    };
    private readonly string[] unlockText = new string [9]
    {
        "Unlock", "Открыть", "開鎖", "开锁", "Freischalten", "Desbloquear", "Déverrouiller", "Odblokuj", "Desbloquear"
    };

    private readonly string[] unlockSubText = new string [9]
    {
        "Survive 5 seconds in an adjacent room", "Выжить 5 секунд в соседней комнате.", 
        "在相鄰房間存活 5 秒", "在相邻房间生存 5 秒", 
        "Überlebe 5 Sekunden in einem Nachbarraum", "Sobreviva 5 segundos em uma sala adjacente", 
        "Survivre 5 secondes dans une pièce adjacente", "Przetrwaj 5 sekund w sasiednim pokoju",
        "Sobrevive 5 segundos en una habitación adyacente"
    };

    private readonly string[] bestTimeText = new string [9]
    {
        "Best", "Лучший", "最好的", "最好的", "Beste", "Melhor", "Meilleur", "Najlepiej", "Mejor"
    };

    private readonly string[] startText = new string [9]
    {
        "Start", "Начало", "開始", "开始", "Start", "Começar", "Commencer", "Start", "Comenzar"
    };

    //LOCALISATION - English, Russian, Chinese T, Chinese S, German, Brazilian, French, Polish, Spanish 
    private readonly string[] volumeText = new string [9]
    {
        "Volume", "Громкость", "音量", "音量", "Lautstärke", "Volume", "Volume", "Glosnosc", "Volumen"
    };

    private readonly string[] quitText = new string [9]
    {
        "Quit", "Выход", "退出", "退出", "Beenden", "Sair", "Quitter", "Wyjdz", "Cerrar"
    };

    //END LOCAL

    //AUDIO
    public Slider volumeSlider;

    public float volumeValue;

    public AudioMixer audioMixer;

    //

    public GameObject levelSelect;
    private GameObject  levelSelectButtonDown, levelSelectButtonUp, levelSelectButtonLeft, levelSelectButtonRight;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLanguage = 0;
        settingsPanelActive = false;
        levelSelectButtonDown = levelSelect.transform.GetChild(0).gameObject;
        levelSelectButtonUp = levelSelect.transform.GetChild(1).gameObject;
        levelSelectButtonLeft = levelSelect.transform.GetChild(2).gameObject;
        levelSelectButtonRight = levelSelect.transform.GetChild(3).gameObject;
        mainCamera = Camera.main;
        PanelFadeIn();
        SetNewSelectedLevel(new Vector2(0,0));

    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleSettingsPanel();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraSmoothFollow();
    }

    public void CameraSmoothFollow()
    {
        if(currentPlayer)
        {
            Vector3 newCamPosition = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, mainCamera.transform.position.z) ;
            newCamPosition = Vector3.Slerp(mainCamera.transform.position, newCamPosition, Time.smoothDeltaTime * camMotionSpeed);
            mainCamera.transform.position = newCamPosition;
        } else 
        {
            Vector3 newCamPosition = Vector3.Slerp(mainCamera.transform.position, cameraDefaultPos, Time.smoothDeltaTime * camMotionSpeed); 
            mainCamera.transform.position = newCamPosition;
        }

    }

    public void setNewPlayerCameraTarget(GameObject newPlayer)
    {
        currentPlayer = newPlayer;
    }

    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.DOAnchorPosX(targetPos.position.x, fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(1, fadeTime); 
        levelSelect.gameObject.SetActive(true);
        levelSelectButtonDown.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(1, fadeTime); 
        levelSelectButtonUp.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(1, fadeTime); 
        levelSelectButtonLeft.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(1, fadeTime); 
        levelSelectButtonRight.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(1, fadeTime); 
        StartCoroutine(ZoomCamera(1.9f));
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.DOAnchorPosX(startingPos.position.x, fadeTime, false).SetEase(Ease.InOutQuint);
        levelSelect.gameObject.SetActive(false);
        levelSelectButtonDown.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(0, fadeTime); 
        levelSelectButtonUp.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(0, fadeTime); 
        levelSelectButtonLeft.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(0, fadeTime); 
        levelSelectButtonRight.gameObject.GetComponent<UnityEngine.UI.Image>().DOFade(0, fadeTime);
    }

    public void ToggleSettingsPanel()
    {
        settingsPanelActive = !settingsPanelActive;
        settingsPanel.SetActive(settingsPanelActive);
    }

    public void HideNavigationButtons()
    {
        levelSelectButtonDown.gameObject.SetActive(true);
        levelSelectButtonUp.gameObject.SetActive(true);
        levelSelectButtonLeft.gameObject.SetActive(true);
        levelSelectButtonRight.gameObject.SetActive(true);

        if(selectedLevel.x == 0)
        {
            levelSelectButtonLeft.gameObject.SetActive(false);
        }
        if(selectedLevel.x == 4)
        {
            levelSelectButtonRight.gameObject.SetActive(false);
        }
        if(selectedLevel.y == 0)
        {
            levelSelectButtonDown.gameObject.SetActive(false);
        }
        if(selectedLevel.y == 4)
        {
            levelSelectButtonUp.gameObject.SetActive(false);
        }
    }

    public void ResetObjectivesText()
    {

        if(LevelManager.Instance.levelUnlockedStatus[(int)selectedLevel.x,(int)selectedLevel.y] == false)
        {
            startButton.SetActive(false);
            objectivesPanel.SetActive(true);
        }
        else
        {
            startButton.SetActive(true);
            objectivesPanel.SetActive(false);
        }

    }

    public void SetNewSelectedLevel(Vector2 t)
    {
        //Recalibrate the text so it looks cool
        selectedLevel = t;
        ResetLevelText();
        HideNavigationButtons();
        ResetObjectivesText();
    }

    void ResetLevelText()
    {
        string temp = levelNameTexts[(int)selectedLevel.y, (int)selectedLevel.x];
        StartTypewriter(levelNameText, temp);
    }

    private void StartTypewriter(TMP_Text txt, string writer)
	{
		StopAllCoroutines();
		
		if (txt != null)
		{
			txt.text = "";

			StartCoroutine(TypeWriterTMP(txt, writer));
		}
	}

	IEnumerator TypeWriterTMP(TMP_Text txt, string writer)
    {
        yield return new WaitForSeconds(0.2f);

		foreach (char c in writer)
		{
            TypingAudioController.Instance.playTypingClip();
			if (txt.text.Length > 0)
			{
				txt.text = txt.text.Substring(0, txt.text.Length);
			}
			txt.text += c;
			yield return new WaitForSeconds(0.05f);
		}
	}

    IEnumerator ZoomCamera(float endSize)
    {
        float startingSize = mainCamera.orthographicSize;
        float elapsedTime = 0;
        float timeToComplete = 0.3f;


        while (elapsedTime < timeToComplete)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startingSize, endSize, elapsedTime/timeToComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = endSize;


        yield return null;
    }

    //LOCALISATION

    public void ChangeLanguageUp()
    {
        if(currentLanguage < 8)
        {
            currentLanguage += 1;
        } else 
        {
            currentLanguage = 0;
        }

        ResetLocalisationTexts();
        SoundEffectPlayer.Instance.playUISelectClip();


    }

    public void ChangeLanguageDown()
    {
        if(currentLanguage > 0)
        {
            currentLanguage -= 1;
        } else 
        {
            currentLanguage = 8;
        }

        ResetLocalisationTexts();
        SoundEffectPlayer.Instance.playUISelectClip();

    }

    void ResetLocalisationTexts()
    {
        switch(currentLanguage) 
        {
        case 0:
            // English
            languageTmpText.font = englishFont;
            unlockTmpText.font = englishFont;
            unlockSubTmpText.font = englishFont;
            bestTimeTmpText.font = englishFont;
            startTmpText.font = englishFont;
            quitTmpText.font = englishFont;
            volumeTmpText.font = englishFont;
            break;
        case 1:
            // Russian
            languageTmpText.font = russianFont;
            unlockTmpText.font = russianFont;
            unlockSubTmpText.font = russianFont;
            bestTimeTmpText.font = russianFont;
            startTmpText.font = russianFont;
            quitTmpText.font = russianFont;
            volumeTmpText.font = russianFont;
            break;
        case 2:
            // Chinese - Trad
            languageTmpText.font = chineseTraditionalFont;
            unlockTmpText.font = chineseTraditionalFont;
            unlockSubTmpText.font = chineseTraditionalFont;
            bestTimeTmpText.font = chineseTraditionalFont;
            startTmpText.font = chineseTraditionalFont;
            quitTmpText.font = chineseTraditionalFont;
            volumeTmpText.font = chineseTraditionalFont;
            break;
        case 3:
            // Chinese - Simp
            languageTmpText.font = chineseSimplifiedFont;
            unlockTmpText.font = chineseSimplifiedFont;
            unlockSubTmpText.font = chineseSimplifiedFont;
            bestTimeTmpText.font = chineseSimplifiedFont;
            startTmpText.font = chineseSimplifiedFont;
            quitTmpText.font = chineseSimplifiedFont;
            volumeTmpText.font = chineseSimplifiedFont;
            break;
        default:
            // Latin
            languageTmpText.font = latinFont;
            unlockTmpText.font = latinFont;
            unlockSubTmpText.font = latinFont;
            bestTimeTmpText.font = latinFont;
            startTmpText.font = latinFont;
            quitTmpText.font = latinFont;
            volumeTmpText.font = latinFont;
            break;
        }

        languageTmpText.text = languageText[currentLanguage];
        unlockTmpText.text = unlockText[currentLanguage];
        unlockSubTmpText.text = unlockSubText[currentLanguage];
        bestTimeTmpText.text = bestTimeText[currentLanguage];
        startTmpText.text = startText[currentLanguage];
        quitTmpText.text = quitText[currentLanguage];
        volumeTmpText.text = volumeText[currentLanguage];
    }

    public void SlideVolume()
    {
        audioMixer.SetFloat("MasterSlider", volumeSlider.value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

