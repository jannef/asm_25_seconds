using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField] public string[] CreditNames;
    [SerializeField] public Text TextField;
    public float LetterDelay = 0.1f;
    public float NameDelay = 1.0f;

    private void Awake()
    {
        StartCoroutine(DisplayName(0, CreditNames));
    }

    private IEnumerator DisplayName(int index, string[] names)
    {
        var name = "";
        foreach (var c in names[index])
        {
            yield return new WaitForSeconds(LetterDelay);
            name += c.ToString();
            TextField.text = name;
        }

        yield return new WaitForSeconds(NameDelay);
        StartCoroutine(DisplayName((index + 1) % names.Length, names));
    }
}
