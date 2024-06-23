using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : PoolingObject
{
    TextMeshProUGUI text;
    RectTransform rect;

    float floatingSpeed = 300f;
    float fontSize = 150;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
    }

    public void Play(float damage, Color textColor)
    {
        StartCoroutine(PlayCor());

        IEnumerator PlayCor()
        {
            text.text = ((int)damage).ToString();
            text.color = textColor;
            text.fontSize = this.fontSize;

            while (text.fontSize <= this.fontSize * 1.1f)
            {
                text.fontSize += Time.deltaTime * 70f;
                rect.transform.position += Vector3.up * Time.deltaTime * floatingSpeed;

                yield return GlobalCache.update;
            }

            float a = 1.1f;
            while (text.fontSize >= this.fontSize * 0.8f)
            {
                text.fontSize -= Time.deltaTime * 50f;
                a -= Time.deltaTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, a);
                rect.transform.position += Vector3.up * Time.deltaTime * floatingSpeed;

                yield return GlobalCache.update;
            }

            gameObject.SetActive(false);
        }
    }
}
