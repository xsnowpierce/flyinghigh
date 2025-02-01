using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SplashController : MonoBehaviour
{
    private SaveDataLoaded data;
    public GameObject saveDataScreen;
    public SoundController sound;
    public LoadingManager loading;
    [Space(10)]
    public Animator logoAnimator;
    public AnimationClip logoAnimationClip;
    [Space(10)]
    public Animator saveDataAnimator;
    public AnimationClip saveDataClip;
    public AnimationClip fadeSaveDataClip;
    [Space(10)]
    public bool playSounds = false;
    public bool allowInputs;

    void Awake(){
        loading = GameObject.FindObjectOfType<LoadingManager>();
        sound = GameObject.FindObjectOfType<SoundController>();
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
    }

    void Start(){
        StartCoroutine(startFrame());
    }

    IEnumerator startFrame(){
        yield return new WaitForSecondsRealtime(1.0f);
        logoAnimator.Play("logo", 0, 0.0f);
        yield return new WaitForSecondsRealtime(logoAnimationClip.length + 1.0f);
        if(!data.SaveGameExists()){
            StartCoroutine(dataCheck());
        }else{
            data.LoadGame();
            StartCoroutine(Proceed(false));
        }
        
        yield return null;
    }

    IEnumerator dataCheck(){
        saveDataScreen.SetActive(true);
        saveDataAnimator.Play("savedata", 0, 0.0f);
        yield return new WaitForSecondsRealtime(saveDataClip.length);
        allowInputs = true;
        playSounds = true;
        yield return null;
    }

    void OnSelect(){
        // user accepted save
        if(allowInputs && !loading.isLoading){
            sound.sfx.PlaySFX(sound.sfx.ui_confirm);
            AcceptSave();
        }
    }
    void OnCancel(){
        // player declined
        if(allowInputs && !loading.isLoading){
            sound.sfx.PlaySFX(sound.sfx.ui_cancel);
            DeclineSave();
        }
    }
    public void AcceptSave(){
        data.AcceptedSave();
        StartCoroutine(Proceed(true));
    }

    public void DeclineSave(){
        data.useSaveFile = false;
        StartCoroutine(Proceed(true));
    }

    IEnumerator Proceed(bool saveMenuOpened){
        allowInputs = false;
        if(saveMenuOpened){
            saveDataAnimator.Play("fadesavedata", 0, 0.0f);
            yield return new WaitForSecondsRealtime(fadeSaveDataClip.length + 0.5f);
        }
        loading.LoadSceneAsync("Main Menu");
    }

}
