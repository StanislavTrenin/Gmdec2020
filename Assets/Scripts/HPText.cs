using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPText : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (true)
        {
            Color color = text.color;
            color.a -= 1f / 255;
            if (color.a < 0)
            {
                DestroyImmediate(gameObject);
                break;
            }

            text.color = color;
            yield return new WaitForSeconds(1f / 255);
        }
    }

}
