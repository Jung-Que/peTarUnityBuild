using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class modeChg : MonoBehaviour
{
    public static int mode = 0; // 0 place 1 ctrl
    public GameObject uiObject; // 연결된 UI 오브젝트
    public float displayDuration = 1f; // 표시 지속 시간
    public Text text;
    private bool isDisplaying = false; // UI 표시 여부
    private float displayTimer = 0f; // 표시 타이머

    void Start()
    {
        HideUI(); // 시작 시 UI를 숨깁니다.
    }

    void Update()
    {
        if(mode ==0)
        text.text = "Mode : Place";
        else
        text.text = "Mode : Ctrl";
        if (isDisplaying)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0f)
            {
                HideUI();
            }
        }
    }

    public void onClicked()
    {
        if (mode == 0)
        {
            mode = 1;
            ShowUI();
        }
        else
        {
            mode = 0;
            ShowUI();
        }

        displayTimer = displayDuration;
        isDisplaying = true;
    }

    private void ShowUI()
    {
        uiObject.SetActive(true);
    }

    private void HideUI()
    {
        uiObject.SetActive(false);
        isDisplaying = false;
    }
}
