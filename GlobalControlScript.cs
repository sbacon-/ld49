using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControlScript : MonoBehaviour
{
    public static GlobalControlScript Instance;
    public Sounds[] sounds;

    private void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        foreach (Sounds s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isMusic;
        }
    }

    //[SETTINGS]
    public int kbL;
    public float musicVol, sfxVol;
    public bool mute, pause;

    private void Start() {
        kbL = 0;
        musicVol = 0.25f;
        sfxVol = 0.75f;
        mute = false;
        UpdateAudio();
        InitializeDictionary();
        //PlaySound("MAIN_MENU");
    }

    //AUDIO
    public void UpdateAudio() {
        print(musicVol);
        foreach (Sounds s in sounds) {
            if (s.isMusic) s.source.volume = musicVol;
            else s.source.volume = sfxVol;
            s.source.mute = mute;
        }
    }
    public void PlaySound(string name) {
        print(name);
        Sounds s = Array.Find(sounds, s => s.name == name);
        if(s==null){
            print("Unable to find sound : "+name);
            return;
        }else if (!s.source.isPlaying) { 
            if (s.isMusic) StopMusic();
            s.source.Stop();
            s.source.Play();
        }if(s.source.isPlaying&&!s.isMusic){
            s.source.Stop();
            s.source.Play();
        }
    }

    void StopMusic() {
        foreach(Sounds s in sounds) {
            if (s.isMusic && s.source.isPlaying) s.source.Stop();
        }
        
    }

    public void Pause(){
        pause=!pause;
        Time.timeScale = pause? 0 : 1;
        PlaySound(pause?"PAUSE":"UNPAUSE");
    }

    //KEYBOARD LAYOUT
    const int numKeys = 4;
    Dictionary<string, KeyCode> keyMapping;
    string[] keyMaps = new string[numKeys]
    {
        "up",
        "down",
        "left",
        "right"
    };
    KeyCode[] qwerty = new KeyCode[numKeys]
    {
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D
    };
    KeyCode[] azerty = new KeyCode[numKeys]
    {
        KeyCode.Z,
        KeyCode.S,
        KeyCode.Q,
        KeyCode.D
    };
    KeyCode[] dvorak = new KeyCode[numKeys]
    {
        KeyCode.Comma,
        KeyCode.O,
        KeyCode.A,
        KeyCode.E
    };
    private void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for(int i=0;i<keyMaps.Length;++i)
        {
            keyMapping.Add(keyMaps[i], qwerty[i]);
        }
    }

    public bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    } 
      
    public void SetQwerty()
    {
        for(int i=0;i<keyMaps.Length;++i)
        {
            keyMapping[keyMaps[i]] = qwerty[i];
        }
    }
    public void SetAzerty()
    {
        for(int i=0;i<keyMaps.Length;++i)
        {
            keyMapping[keyMaps[i]] = azerty[i];
        }
    }
    public void SetDvorak()
    {
        for(int i=0;i<keyMaps.Length;++i)
        {
            keyMapping[keyMaps[i]] = dvorak[i];
        }
    }
}
