using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public Animator loadingAnimator;
    public Animator logoAnimator;
    public AnimationClip loadingAppear;
    public AnimationClip loadingProgress;
    public AnimationClip loadingDisappear;
    public float loadingDelay;
    public float crossFadeDuration;
    public bool isLoading;
    public bool beganFadeout;
    private SoundController sound;
    private SettingsController settings;
    private SaveDataLoaded data;
    void Awake(){
        DontDestroyOnLoad(this);
        sound = GameObject.FindObjectOfType<SoundController>();
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
    }

    public void LoadSceneAsync(string scene){
        StartCoroutine(loadSceneAsyncronous(scene));
    }

    IEnumerator loadSceneAsyncronous(string scene){
        isLoading = true;
        logoAnimator.Play(loadingProgress.name, 0, 0.0f);
        loadingAnimator.Play(loadingAppear.name, 0, 0.0f);
        yield return new WaitForSecondsRealtime(loadingAppear.length);
        AsyncOperation operation =  SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while(!operation.isDone){
            yield return null;
        }
        yield return new WaitForSecondsRealtime(loadingDelay);
        beganFadeout = true;
        loadingAnimator.CrossFade(loadingDisappear.name, crossFadeDuration, 0, 0.0f, 0.0f);
        yield return new WaitForSecondsRealtime(loadingDisappear.length);
        logoAnimator.StopPlayback();
        isLoading = false;
        yield return null;
    }

    void Update(){
        if(SceneManager.GetActiveScene().name == "Settings"){
            if(!isLoading){
                // lets make the volume of both music and sfx be linked to the settings values,
                // so the player can preview.
                SettingsController settings = GameObject.FindObjectOfType<SettingsController>();
                sound.sfx.volume = (float) settings.sfxVolume / 100.0f;
                sound.music.ChangeVolume((float) settings.musicVolume / 100.0f);
                sound.music.defaultVolume = (float) settings.musicVolume / 100.0f;
            }
        }else{
            if(sound.music.defaultVolume != (float) data.musicVolume / 100.0f){
                sound.music.ChangeVolume((float) data.musicVolume / 100.0f);
                sound.sfx.volume = (float) data.sfxVolume / 100.0f;
            }
        }
    }
}
