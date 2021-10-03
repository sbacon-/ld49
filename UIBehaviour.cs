using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    GlobalControlScript gcs;
    public Transform slider;
    float slideTarget = 0f;
    float tagFontMax=15, tagFontMin=10;
    bool tagShrink =false;

    TextMeshProUGUI tagLine;


    // Start is called before the first frame update
    void Start()
    {
       gcs = GameObject.Find("GlobalControl").GetComponent<GlobalControlScript>();
       tagLine = GameObject.Find("TAGLINE").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(slider.localPosition.x != slideTarget) Slide();
        if(tagLine != null){
            tagLine.fontSize += (tagShrink?-1:1) * 0.05f;
            if(tagLine.fontSize > tagFontMax || tagLine.fontSize < tagFontMin) tagShrink = !tagShrink;

        } 
    }

    public void SlideLeft(){
        gcs.PlaySound("SELECT");
        slideTarget+=800;
        DisplayOptions();
    }
    public void SlideRight(){
        gcs.PlaySound("SELECT");
        slideTarget-=800;
        DisplayOptions();
    }
    void Slide(){
        float factor = slider.localPosition.x>slideTarget?-20f:20f;
        slider.localPosition = Vector3.right * (factor + slider.localPosition.x);
    }
    
    //AUDIO
    void DisplayOptions(){
        GameObject.Find("KBL").GetComponent<Dropdown>().value = gcs.kbL;
        GameObject.Find("MUSIC").GetComponent<Slider>().SetValueWithoutNotify(gcs.musicVol);
        GameObject.Find("SFX").GetComponent<Slider>().SetValueWithoutNotify(gcs.sfxVol);
        GameObject.Find("MUTE").GetComponent<Toggle>().SetIsOnWithoutNotify(gcs.mute);
    }
    public void UpdateAudio(){
        gcs.musicVol = GameObject.Find("MUSIC").GetComponent<Slider>().value;
        gcs.sfxVol = GameObject.Find("SFX").GetComponent<Slider>().value;
        gcs.mute = GameObject.Find("MUTE").GetComponent<Toggle>().isOn;
        gcs.UpdateAudio();
        DisplayOptions();
    }
    public void UpdateKBL(){
        gcs.kbL = GameObject.Find("KBL").GetComponent<Dropdown>().value;
        switch(gcs.kbL){
            case 0:
                gcs.SetQwerty();
                break;
            case 1:
                gcs.SetAzerty();
                break;
            case 2:
                gcs.SetDvorak();
                break;
        }
    }

    public void StartGame(){
        SceneManager.LoadScene("GameScene");
    }
}
