using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Image textBox;
    public Text bodyText;
    public TextStorage textStorage;
    // Start is called before the first frame update
    void Start()
    {
        Enable(false);
    }

    public void Enable(bool val)
    {
        textBox.enabled = val;
        bodyText.enabled = val;
    }

    public void SetBody(int i)
    {
        bodyText.text = textStorage.Retrieve(i);
    }
}
