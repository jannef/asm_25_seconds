using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeAfterSeconds : MonoBehaviour {

    public float Delay = 5.0f;
    public float FadeDuration = 2.0f;
    Text txt;
    // Use this for initialization
    private void Awake()
    {
        txt = GetComponent<Text>();
    }

	void Start () {
        Invoke("StartFade", Delay);
	}
    void StartFade()
    {
        StartCoroutine(FadeAway());
    }
    IEnumerator FadeAway()
    {
        float fadeStep = FadeDuration * Time.deltaTime;
        while (txt.color.a > 0)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - fadeStep);
            yield return null;
        }
    }
}
