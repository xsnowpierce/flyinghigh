using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    public bool hardSelected;
    public bool inputAllowed = true;
    public bool hasSelected = false;
    public float inputCooldown;
    public bool inMenu;

    [Space(10)]
    public LoadingManager loading;
    public BirdTextureModifier textureModifier;
    public float birdFadeSpeed;
    public float fadeDelay;
    private SoundController sound;
    private SaveDataLoaded data;
    public Text selectText;

    [Space(10)]
    public Animator cameraAnim;
    public Animator EasyMode;
    public Animator HardMode;
    public Animator Spinner;
    public Animator SelectMenu;
    public Animator FadeLayer;
    
    [Space(20)]
    public AnimationClip moveToEasyClip;
    public AnimationClip moveToHardClip;
    public AnimationClip chooseEasyClip;
    public AnimationClip chooseHardClip;
    [Space(10)]
    public AnimationClip easyMode_Select;
    public AnimationClip easyMode_Deselect;
    [Space(10)]
    public AnimationClip hardMode_Select;
    public AnimationClip hardMode_Deselect;
    [Space(10)]
    public AnimationClip spinner_selectEasy;
    public AnimationClip spinner_deSelectEasy;
    public AnimationClip spinner_selectHard;
    public AnimationClip spinner_deSelectHard;
    [Space(10)]
    public AnimationClip select_Appear;
    public AnimationClip select_Fade;
    [Space(10)]
    public AnimationClip fade_Appear;
    public AnimationClip fade_Fade;

    void Awake(){
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        sound = GameObject.FindObjectOfType<SoundController>();
        loading = GameObject.FindObjectOfType<LoadingManager>();
        sound.music.CrossFadeSong(sound.music.difficultySelect, true, true);
    }

    void OnNavigate(){
        float nav = GetComponent<PlayerInput>().actions["Navigate"].ReadValue<Vector2>().x;
        if(inputAllowed && !loading.isLoading){
            if(!hasSelected){
                if(nav == -1){
                    StartCoroutine(moveTo(chooseEasyClip, fadeDelay, 0));
                    StartCoroutine(ChangeSpinner(0, true));
                    hardSelected = false;
                }else if(nav == 1){
                    StartCoroutine(moveTo(chooseHardClip, fadeDelay, 1));
                    StartCoroutine(ChangeSpinner(1, true));
                    hardSelected = true;
                }
                hasSelected = true;
            }else{
                // play animation
                if(!hardSelected) {
                     StartCoroutine(moveTo(moveToHardClip, fadeDelay, 1));
                     StartCoroutine(ChangeSpinner(1, false));
                     hardSelected = true;
                }else if(hardSelected) { 
                    StartCoroutine(moveTo(moveToEasyClip, fadeDelay, 0));
                    StartCoroutine(ChangeSpinner(0, false));
                    hardSelected = false;
                }
            }
            sound.sfx.PlaySFX(sound.sfx.ui_select);
        }
    }

    void OnSelect(){
        if(inputAllowed && !loading.isLoading){
            StartCoroutine(Select(true));
        }
        
    }
    IEnumerator Select(bool selecting){
        if(selecting){

            // player pressed accept
            if(inputAllowed && !loading.isLoading){
                if(!inMenu){
                    inputAllowed = false;
                    if(hardSelected){
                        selectText.text = "Are you sure you want to play in Hard Mode?";
                    } else {
                        selectText.text = "Are you sure you want to play in Easy Mode?";
                    }
                    SelectMenu.Play(select_Appear.name, 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_confirm);
                    inMenu = true;
                    yield return new WaitForSeconds(select_Appear.length + 0.1f);
                    inputAllowed = true;
                }else{
                    inputAllowed = false;
                    FadeLayer.Play(fade_Appear.name, 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_confirm);
                    yield return new WaitForSeconds(fade_Appear.length + 0.1f);
                    
                    // load new scene
                    if(hardSelected) data.hardMode = true;
                    if(!hardSelected) data.hardMode = false;
                    loading.LoadSceneAsync("Game");
                }
            }
        }else{
            // player pressed cancel
            inputAllowed = false;
            SelectMenu.Play(select_Fade.name, 0, 0.0f);
            sound.sfx.PlaySFX(sound.sfx.ui_cancel);
            yield return new WaitForSeconds(select_Fade.length + 0.1f);
            inMenu = false;
            inputAllowed = true;
        }
        yield return null;
    }

    void OnCancel(){
        if(inputAllowed && !loading.isLoading){
            if(inMenu){
                StartCoroutine(Select(false));
            }else{
                inputAllowed = false;
                sound.sfx.PlaySFX(sound.sfx.ui_cancel);
                loading.LoadSceneAsync("Main Menu");
            }
        }
    }

    IEnumerator moveTo(AnimationClip moveClip, float fadeDelay, int fadeBird){

        inputAllowed = false;
        cameraAnim.Play(moveClip.name, 0, 0.0f);

        // start fading out the current bird right away
        if(fadeBird == 0) textureModifier.FadeBird2(1, birdFadeSpeed);
        if(fadeBird == 1) textureModifier.FadeBird1(1, birdFadeSpeed);

        yield return new WaitForSeconds(moveClip.length * fadeDelay);

        // start raising bird fade
        if(fadeBird == 0){
            textureModifier.FadeBird1(0, birdFadeSpeed);
        }
        if(fadeBird == 1){
            textureModifier.FadeBird2(0, birdFadeSpeed);
        }

        // apply text change
        ChangeText(fadeBird);

        yield return new WaitForSeconds(moveClip.length - (moveClip.length * fadeDelay));
        inputAllowed = true;
        yield return null;
    }

    IEnumerator ChangeSpinner(int birdSelect, bool firstSelect){
        if(firstSelect){
            if(birdSelect == 0){
                Spinner.Play(spinner_selectEasy.name, 0, 0.0f);
            }else if(birdSelect == 1){
                Spinner.Play(spinner_selectHard.name, 0, 0.0f);
            }
        }else{
            if(birdSelect == 0){
                Spinner.Play(spinner_deSelectHard.name, 0, 0.0f);
                yield return new WaitForSeconds(spinner_deSelectHard.length);
                Spinner.Play(spinner_selectEasy.name, 0, 0.0f);
            }else if(birdSelect == 1){
                Spinner.Play(spinner_deSelectEasy.name, 0, 0.0f);
                yield return new WaitForSeconds(spinner_deSelectEasy.length);
                Spinner.Play(spinner_selectHard.name, 0, 0.0f);
            }
        }
        yield return null;
    }

    void ChangeText(int birdSelect){
        if(birdSelect == 0){
            EasyMode.Play(easyMode_Select.name, 0, 0.0f);
            HardMode.Play(hardMode_Deselect.name, 0, 0.0f);
        }else if(birdSelect == 1){
            EasyMode.Play(easyMode_Deselect.name, 0, 0.0f);
            HardMode.Play(hardMode_Select.name, 0, 0.0f);
        }
    }
}
