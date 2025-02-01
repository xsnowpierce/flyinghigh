using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public AudioClip game_coinSound;
    public AudioClip game_goalPass;
    public AudioClip game_hitSound;
    public AudioClip game_jumpSound;
    public AudioClip game_speedUpSound;
    public AudioClip ui_select;
    public AudioClip ui_cancel;
    public AudioClip ui_confirm;
    private AudioSource source;
    public float volume;
    void Awake(){
        source = this.gameObject.GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip){
        source.PlayOneShot(clip, volume);
    }

    public void StopSFX(){
        source.Stop();
    }
}
