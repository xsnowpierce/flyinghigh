using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip mainMenu;
    public AudioClip difficultySelect;
    public AudioClip easyGameIntro;
    public AudioClip easyGameLoop;
    public AudioClip hardGameIntro;
    public AudioClip hardGameLoop;
    public AudioClip resultsLost;
    public AudioClip resultsWinIntro;
    public AudioClip resultsWinLoop;
    public AudioClip creditsSong;
    [Space(10)]
    public float crossFadeDecrementValue;
    private Coroutine currentCoroutine;
    public float defaultVolume;

    void OnEnable(){
        source = GetComponent<AudioSource>();
    }

    public void CrossFadeSong(AudioClip destionationSong, bool loop, bool onlyFadeOut){
        if(currentCoroutine != null){
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(CrossFade(destionationSong, loop, onlyFadeOut));
    }
    public void InterruptSong(AudioClip destionationSong, bool loop){
        if(currentCoroutine != null){
            StopCoroutine(currentCoroutine);
        }
        source.clip = destionationSong;
        source.Play();
    }

    public void PlayIntroLoopSong(AudioClip intro, AudioClip loop){
        if(currentCoroutine != null){
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(PlayLoopIntro(intro, loop));
    }

    public void ContinueLoopSong(AudioClip intro, AudioClip loop){
        if(currentCoroutine != null){
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ContinueLoop(intro, loop));
    }

    IEnumerator PlayLoopIntro(AudioClip intro, AudioClip loop){
        source.clip = intro;
        source.loop = false;
        source.Play();
        yield return new WaitForSecondsRealtime(intro.length);
        source.clip = loop;
        source.loop = true;
        source.Play();
        yield return null;
    }

    IEnumerator ContinueLoop(AudioClip intro, AudioClip loop){
        if(source.clip == intro){
            yield return new WaitForSecondsRealtime(intro.length - source.time);
            source.clip = loop;
            source.loop = true;
            source.Play();
            
        }
        yield return null;
    }

    IEnumerator CrossFade(AudioClip destionationSong, bool loop, bool onlyFadeOut){
        while(source.volume > 0.0f){
            source.volume -= (crossFadeDecrementValue * Time.unscaledDeltaTime);
            yield return null;
        }
        
        source.clip = destionationSong;
        source.Play();
        if(!onlyFadeOut){
            while(source.volume < defaultVolume){
                source.volume += (crossFadeDecrementValue * Time.unscaledDeltaTime);
                yield return null;
            }
        }else{
            source.volume = defaultVolume;
        }
        
        
        if(loop){
            source.loop = true;
        }
    }

    public void ChangeVolume(float volume){
        source.volume = volume * 1.0f;
        defaultVolume = volume * 1.0f;
    }
}
