using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [HideInInspector]
    public MusicController music;
    [HideInInspector]
    public SFXController sfx;
    private SaveDataLoaded data;
    void Awake(){
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        music = GetComponentInChildren<MusicController>();
        sfx = GetComponentInChildren<SFXController>();
        DontDestroyOnLoad(this.gameObject);

        music.defaultVolume = (float) data.musicVolume / 100.0f;
        music.ChangeVolume((float) data.musicVolume / 100.0f);
        sfx.volume = (float) data.sfxVolume / 100.0f;
    }
    
}
