using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastBehaviour : MonoBehaviour
{
    bool hide = true;
    Image im;
    TextMeshProUGUI text;
    Color imColor,texColor;
    // Start is called before the first frame update
    void Start()
    {
        im = gameObject.GetComponent<Image>();
        text = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];

        imColor = im.color;
        texColor = text.color;

    }

    // Update is called once per frame
    void Update()
    {
        im.color = imColor;
        text.color = texColor;
        if(hide){
       imColor.a--;
       texColor.a--; 
        }
    }

    void Show(){
        hide = false;
        imColor.a=255;
        texColor.a=255;
    }
    void Hide(){
        hide = true;
    }

    public void Toast(string s){
        text.text = s;
        Show();
        CancelInvoke("Hide");
        Invoke("Hide",3.0f);
    }


}
