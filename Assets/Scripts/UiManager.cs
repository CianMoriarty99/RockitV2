using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public Vector2 selectedLevel;
    public TMP_Text levelNameText, levelObjectiveText, runTimeText, bestTimeText;
    public float fadeTime;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public Transform startingPos, targetPos;

    private static UiManager _instance;
    public static UiManager Instance { get { return _instance; } }



    //Camera stuff
    public GameObject currentPlayer;

    public Camera mainCamera;
    private float camMotionSpeed = 5f;
    private float camRotationSpeed = 50f;
    private Vector3 camOffset = new Vector3(0, 0, 0); 

    private readonly string[,] levelNameTexts = new string[5, 5] 
    {
        {"Ground Zero","L.A.S.E.R","Hot Hot Hot!","4","5"},
        {"6","7","8","9","10"}, 
        {"11","12","13","14","15"},
        {"16","17","18","19","20"},
        {"21","22","23","24","25"} 
    };


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
        levelSelectButtonDown = levelSelect.transform.GetChild(0).gameObject;
        levelSelectButtonUp = levelSelect.transform.GetChild(1).gameObject;
        levelSelectButtonLeft = levelSelect.transform.GetChild(2).gameObject;
        levelSelectButtonRight = levelSelect.transform.GetChild(3).gameObject;

        mainCamera = Camera.main;

        
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
            newCamPosition = Vector3.Slerp(mainCamera.transform.position, newCamPosition, Time.smoothDeltaTime * camMotionSpeed); //spherical lerp smoothing
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
        StartCoroutine(ZoomCamera(1.6f));
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

    public void SetNewSelectedLevel(Vector2 t)
    {
        //Recalibrate the text so it looks cool
        selectedLevel = t;

        ResetLevelText();
        HideNavigationButtons();
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
}

