using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    Camera cam;

    [Header("PlayerHP")]
    public Transform heartSlot;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("Wave")]
    public Image waveFill;

    [Header("ButtonAction")]
    public Image[] attackCoolTimeShadow;
    public Image[] jumpCoolTimeShadow;
    public Image[] defenseCoolTimeShadow;
    public GameObject[] chargedButtonGroup;
    public EventTrigger[] actionButtonGroup;
    public EventTrigger[] chargedActionButtonGroup;
    public Image[] chargeGuageFill;

    [Header("Damage Text")]
    public Transform floatingTextLayer;

    [Header("Cover Panel")]
    public GameObject coverPanel;
    public Button coverButton;
    public TextMeshProUGUI coverText;

    void Awake()
    {
        instance = this;
        cam = Camera.main;
    }

    void Start()
    {
        InitButtonAction();
    }

    #region Action Button

    public void AddOnClickActionEvent(int buttonIndex, PlayerAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { action.OnClickActionButton(); });

        actionButtonGroup[buttonIndex].triggers.Add(entry);
    }

    void InitButtonAction()
    {
        InitCoolTimeShadow(jumpCoolTimeShadow);
        InitCoolTimeShadow(defenseCoolTimeShadow);
        InitCoolTimeShadow(attackCoolTimeShadow);

        void InitCoolTimeShadow(Image[] target)
        {
            foreach (Image image in target)
                image.fillAmount = 0;
        }
    }

    public void ButtonCoolTime(int buttonIndex, float coolTime)
    {
        switch (buttonIndex)
        {
            case 0:
                StartCoroutine(CoolTimeCor(jumpCoolTimeShadow));
                break;
            case 1:
                StartCoroutine(CoolTimeCor(defenseCoolTimeShadow));
                break;
            case 2:
                StartCoroutine(CoolTimeCor(attackCoolTimeShadow));
                break;
        }

        IEnumerator CoolTimeCor(Image[] targetImage)
        {
            foreach (Image image in targetImage)
                    image.fillAmount = 1f;

            float t = coolTime;
            while(t > 0)
            {
                t -= Time.deltaTime;
                yield return GlobalCache.update;

                foreach (Image image in targetImage)
                    image.fillAmount = t / coolTime;
            }

            foreach (Image image in targetImage)
                image.fillAmount = 0;
        }
    }

    #endregion

    #region Charged Button

    public void OnUpdateCharedGuage(int buttonIndex, int curStack, int maxStack)
    {
        chargeGuageFill[buttonIndex].fillAmount = ((float)curStack / (float)maxStack);
    }

    public void OpenChargedActionButton(int buttonIndex)
    {
        chargedButtonGroup[buttonIndex].SetActive(true);
    }

    public void CloseChargedActionButton(int buttonIndex)
    {
        chargedButtonGroup[buttonIndex].SetActive(false);
    }

    public void AddOnClickChargedActionEvent(int buttonIndex, PlayerAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { action.OnClickChargedAction(); });

        chargedActionButtonGroup[buttonIndex].triggers.Add(entry);
    }

    #endregion

    #region Damage Text
    public void FloatingDamageText(Vector2 pos, float damage, Color color)
    {
        DamageText text = ObjectPooler.SpawnFromPool("DamageText", Vector3.zero).GetComponent<DamageText>();
        text.transform.SetParent(floatingTextLayer);
        text.transform.position = cam.WorldToScreenPoint(pos);
        text.Play(damage, color);
    }
    #endregion

    #region Score

    public void OnUpdateScore(float score)
    {
        scoreText.text = ((int)score).ToString();
    }

    #endregion

    #region Gold

    public void OnUpdateGold(int gold)
    {
        StartCoroutine(UpdateCor());

        IEnumerator UpdateCor()
        {
            int temp = goldText.text == string.Empty ? 0 : int.Parse(goldText.text);
            while(temp != gold)
            {
                temp += (int)((gold - temp) * 0.1f);

                if (temp >= gold)
                    temp = gold;

                goldText.text = temp.ToString();

                yield return GlobalCache.update;
            }

            goldText.text = gold.ToString();
        }
    }

    #endregion

    #region Player Heart

    public void OnUpdatePlayerHeart(int heartCount)
    {
        for (int i = 0; i < heartSlot.childCount; i++)
            heartSlot.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < heartCount; i++)
            heartSlot.GetChild(i).gameObject.SetActive(true);
    }

    #endregion

    #region Cover Panel
    public void OpenCoverPanel(string text)
    {
        coverPanel.gameObject.SetActive(true);
        coverButton.onClick.RemoveAllListeners();
        coverText.text = text;
    }

    public void OpenCoverPanel(string text, UnityAction closePanelEvent)
    {
        coverPanel.gameObject.SetActive(true);
        coverButton.onClick.RemoveAllListeners();
        coverButton.onClick.AddListener(closePanelEvent);
        coverText.text = text;
    }

    public void CloseCoverPanel()
    {
        coverPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Wave

    public void OnUpdateWaveProgress(float curWave, float maxWave)
    {
        waveFill.fillAmount = curWave / maxWave;
    }

    #endregion
}
