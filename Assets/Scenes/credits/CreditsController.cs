using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    private LoadingManager loading;
    public Animator creditsAnimator;
    public AnimationClip creditsClip;
    public bool inputAllowed;
    void Awake(){
        if(GameObject.FindObjectOfType<SoundController>() != null){
            SoundController sound = GameObject.FindObjectOfType<SoundController>();
            sound.music.CrossFadeSong(sound.music.creditsSong, false, true);
        }
        loading = GameObject.FindObjectOfType<LoadingManager>();
    }

    void Start(){
        StartCoroutine(credits());
    }

    IEnumerator credits(){
        creditsAnimator.Play("move", 0, 0.0f);
        inputAllowed = true;
        yield return new WaitForSeconds(creditsClip.length);
        yield return new WaitForSeconds(0.5f);
        
        loading.LoadSceneAsync("Main Menu");
        yield return null;
    }

    void OnSelect(){
        // exit credits
        if(inputAllowed && !loading.isLoading){
            StartCoroutine(exitCredits());
        }
    }

    void OnCancel(){
        // exit credits
        if(inputAllowed && !loading.isLoading){
            StartCoroutine(exitCredits());
        }
    }

    IEnumerator exitCredits(){
        inputAllowed = false;
        loading.LoadSceneAsync("Main Menu");
        yield return null;
    }
}
