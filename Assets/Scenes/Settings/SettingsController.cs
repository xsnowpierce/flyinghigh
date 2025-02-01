using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
public class SettingsController : MonoBehaviour
{
    private PlayerInput input;
    private SoundController sound;
    public int selected = 0;
    public bool inputAllowed = false;
    public bool inConfirmMenu = false;
    public bool isDialogue = false;
    public bool startedFrame = false;
    [Space(10)]
    public int sfxVolume;
    public int musicVolume;
    public int buttonIconID;
    public int quality;
    public bool useDayCycle;
    [Space(20)]
    public Slider bgmSlider;
    public Text bgmPercent;
    public Slider sfxSlider;
    public Text sfxPercent;
    public GameObject sfxPlayIcon;
    public Animator movingAnimator;
    public Animator[] sectionAnimators;
    public Animator[] graphicsButtonAnimators;
    public Animator[] dayNightCycleAnimators;
    public Animator[] buttonStyleAnimators;
    [Space(10)]
    public string[] descriptionText;
    public TMPro.TMP_Text description;
    [Space(10)]
    public Animator confirmBoxAnimator;
    public Text confirmBoxText;
    public GameObject confirmBoxCancelButton;
    public GameObject confirmBoxAcceptButton;
    private SaveDataLoaded data;
    private LoadingManager loading;
    [Space(10)]
    public float holdNavLength;
    public float holdNavActionTime;
    public bool holdingNav;
    public bool navHoldRegistered;
    public float holdNavDelay;

    void Awake(){
        sound = GameObject.FindObjectOfType<SoundController>();
        input = gameObject.GetComponent<PlayerInput>();
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        loading = GameObject.FindObjectOfType<LoadingManager>();

        sfxVolume = data.sfxVolume;
        musicVolume = data.musicVolume;
        buttonIconID = data.buttonIconID;
        quality = data.quality;
        useDayCycle = data.useDayCycle;
        sfxPlayIcon.SetActive(false);
        SetUIValues();
        if(sound.music.source.loop == false){
            sound.music.source.loop = true;
        }
    }

    void SetUIValues(){
        bgmSlider.value = (float) musicVolume / 100.0f;
        if(musicVolume > 0){
            bgmPercent.text = musicVolume + "%";
        }else{
            bgmPercent.text = "0%";
        }
        

        sfxSlider.value = (float) sfxVolume / 100.0f;
        if(sfxVolume > 0){
            sfxPercent.text = sfxVolume + "%";
        }else{
            sfxPercent.text = "0%";
        }

        graphicsButtonAnimators[quality].Play("select", 0, 0.9f);
        if(useDayCycle == true) dayNightCycleAnimators[0].Play("select", 0, 0.9f);
        else if(useDayCycle == false) dayNightCycleAnimators[1].Play("select", 0, 0.9f);
        buttonStyleAnimators[buttonIconID].Play("select", 0, 0.9f);
    }

    void Start(){
        StartCoroutine(frameStart());
    }

    IEnumerator frameStart(){

        while(!loading.beganFadeout){
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        movingAnimator.Play("appear", 0, 0.0f);
        yield return new WaitForSeconds(movingAnimator.GetCurrentAnimatorClipInfo(0).Length);

        inputAllowed = true;

        yield return null;
    }

    void Update(){

        description.text = descriptionText[selected];

        float xval = input.actions["Navigate"].ReadValue<Vector2>().x;
        if(xval != 0){
            if(holdNavLength < holdNavActionTime){
                holdNavLength += Time.deltaTime;
            }else{
                holdingNav = true;
                if(navHoldRegistered == false){
                    StartCoroutine(holdInput());
                }
            }
        }else{
            holdNavLength = 0.0f;
            holdingNav = false;
            navHoldRegistered = false;
        }


    }

    IEnumerator holdInput(){
        navHoldRegistered = true;
        while(holdingNav){

            if(inputAllowed && !inConfirmMenu){
                float xval = input.actions["Navigate"].ReadValue<Vector2>().x;

                if(selected == 1){
                    // music
                        
                    if(xval == -1){
                        // left
                        if(musicVolume > 0){
                            musicVolume -= 1;
                            bgmSlider.value = (float) musicVolume / 100.0f;
                            if(musicVolume > 0){
                                bgmPercent.text = musicVolume + "%";
                            }else{
                                bgmPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                    }else if(xval == 1){
                        // right
                        if(musicVolume < 100){
                            musicVolume += 1;
                            bgmSlider.value = (float) musicVolume / 100.0f;
                            if(musicVolume > 0){
                                bgmPercent.text = musicVolume + "%";
                            }else{
                                bgmPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                    }
                }else if(selected == 2){
                    // sfx
                    if(xval == -1){
                        if(sfxVolume > 0){
                            // left
                            sfxVolume -= 1;
                            sfxSlider.value = (float) sfxVolume / 100.0f;
                            if(sfxVolume > 0){
                                sfxPercent.text = sfxVolume + "%";
                            }else{
                                sfxPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                        
                    }else if(xval == 1){
                        if(sfxVolume < 100){
                            // right
                            sfxVolume += 1;
                            sfxSlider.value = (float) sfxVolume / 100.0f;
                            if(sfxVolume > 0){
                                sfxPercent.text = sfxVolume + "%";
                            }else{
                                sfxPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                    }
                }

            }
            yield return new WaitForSeconds(holdNavDelay);
            yield return null;
        }
        yield return null;
    }

    void OnNavigate(){
        float yval = input.actions["Navigate"].ReadValue<Vector2>().y;
        float xval = input.actions["Navigate"].ReadValue<Vector2>().x;

        if(inputAllowed && !inConfirmMenu){
            if(yval == 1){
                // player moved up
                if(selected > 0){
                    if((selected - 1) == 2){
                        sfxPlayIcon.SetActive(true);
                    }else{
                        sfxPlayIcon.SetActive(false);
                    }
                    // successfully move up
                    sectionAnimators[selected].Play("deSelect", 0, 0.0f);
                    sectionAnimators[selected - 1].Play("select", 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    selected --;
                }else{
                    // wrap input to bottom
                    sectionAnimators[selected].Play("deSelect", 0, 0.0f);
                    sectionAnimators[sectionAnimators.Length - 1].Play("select", 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    selected = sectionAnimators.Length - 1;
                }
            }else if(yval == -1){
                // player moved down
                if(selected < sectionAnimators.Length - 1){
                    if((selected + 1) == 2){
                        sfxPlayIcon.SetActive(true);
                    }else{
                        sfxPlayIcon.SetActive(false);
                    }
                    // successfully move down
                    sectionAnimators[selected].Play("deSelect", 0, 0.0f);
                    sectionAnimators[selected + 1].Play("select", 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    selected ++;
                }else{
                    // wrap input to top?
                    sectionAnimators[selected].Play("deSelect", 0, 0.0f);
                    sectionAnimators[0].Play("select", 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    selected = 0;
                }
            }
            if(xval == -1){
                // player moved left
                switch(selected){
                    case 1:
                        // user pressed left on bgm volume
                        if(musicVolume > 0){
                            musicVolume -= 1;
                            bgmSlider.value = (float) musicVolume / 100.0f;
                            if(musicVolume > 0){
                                bgmPercent.text = musicVolume + "%";
                            }else{
                                bgmPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                        break;
                    case 2:
                        // user pressed left on sfx volume
                        if(sfxVolume > 0){
                            sfxVolume -= 1;
                            sfxSlider.value = (float) sfxVolume / 100.0f;
                            if(sfxVolume > 0){
                                sfxPercent.text = sfxVolume + "%";
                            }else{
                                sfxPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                        break;
                    case 3:
                        // user pressed left on graphics quality
                        if(quality > 0){
                            graphicsButtonAnimators[quality].Play("deSelect", 0, 0.0f);
                            graphicsButtonAnimators[quality - 1].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            quality --;
                        }
                        break;
                    case 4:
                        // user pressed left on daynightcycle
                        if(useDayCycle == false){
                            dayNightCycleAnimators[1].Play("deSelect", 0, 0.0f);
                            dayNightCycleAnimators[0].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            useDayCycle = true;
                        }
                        break;
                    case 5:
                        // user pressed left on button style
                        if(buttonIconID > 0){
                            buttonStyleAnimators[buttonIconID].Play("deSelect", 0, 0.0f);
                            buttonStyleAnimators[buttonIconID - 1].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            buttonIconID --;
                        }
                        break;
                }
            }else if(xval == 1){
                // player moved right
                switch(selected){
                    case 1:
                        // user pressed right on bgm volume
                        if(musicVolume < 100){
                            musicVolume += 1;
                            bgmSlider.value = (float) musicVolume / 100.0f;
                            if(musicVolume > 0){
                                bgmPercent.text = musicVolume + "%";
                            }else{
                                bgmPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                        break;
                    case 2:
                        // user pressed right on sfx volume
                        if(sfxVolume < 100){
                            sfxVolume += 1;
                            sfxSlider.value = (float) sfxVolume / 100.0f;
                            if(sfxVolume > 0){
                                sfxPercent.text = sfxVolume + "%";
                            }else{
                                sfxPercent.text = "0%";
                            }
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                        }
                        break;
                    case 3:
                        // user pressed right on graphics quality
                        if(quality < 3){
                            graphicsButtonAnimators[quality].Play("deSelect", 0, 0.0f);
                            graphicsButtonAnimators[quality + 1].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            quality ++;
                        }
                        break;
                    case 4:
                        // user pressed right on daynightcycle
                        if(useDayCycle == true){
                            dayNightCycleAnimators[0].Play("deSelect", 0, 0.0f);
                            dayNightCycleAnimators[1].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            useDayCycle = false;
                        }
                        break;
                    case 5:
                        // user pressed right on button style
                        if(buttonIconID < 2){
                            buttonStyleAnimators[buttonIconID].Play("deSelect", 0, 0.0f);
                            buttonStyleAnimators[buttonIconID + 1].Play("select", 0, 0.0f);
                            sound.sfx.PlaySFX(sound.sfx.ui_select);
                            buttonIconID ++;
                        }
                        break;
                }
            }
        }
    }

    void OnSelect(){
        if(inputAllowed){
            if(!inConfirmMenu){
                switch(selected){
                    case 0:
                        confirmBoxText.text = "Apply changes and return to menu?";
                        StartCoroutine(openConfirmMenu(false));
                        break;
                    case 2:
                        // player wants to sample sfx volume
                        sound.sfx.PlaySFX(sound.sfx.ui_confirm);
                        break;
                    case 6:
                        // player pressed delete save
                        confirmBoxText.text = "Are you sure you want to delete all data?";
                        StartCoroutine(openConfirmMenu(false));
                        break;
                    case 7:
                        // player pressed view credits
                        confirmBoxText.text = "Are you sure you want to view the credits?";
                        StartCoroutine(openConfirmMenu(false));
                        break;
                }
            }else{
                if(selected == 0){
                    // player pressed confirm changes
                    inputAllowed = false;
                    sound.sfx.PlaySFX(sound.sfx.ui_confirm);

                    // save data
                    data.sfxVolume = sfxVolume;
                    data.musicVolume = musicVolume;
                    data.quality = quality;
                    data.useDayCycle = useDayCycle;
                    data.buttonIconID = buttonIconID;

                    // save to config file
                    data.SaveSettings();

                    StartCoroutine(closeConfirmMenu());
                    loading.LoadSceneAsync("Main Menu");
                    GameObject.FindObjectOfType<IconController>().UpdateIcon();
                }
                // player accepted inside the confirm window
                if(selected == 6){
                    // player deleted data
                    data.DeleteSave();
                    confirmBoxText.text = "Save data deleted.";
                    StartCoroutine(openConfirmMenu(true));
                    
                }else if(selected == 7){
                    // player clicked credits
                    inputAllowed = false;
                    loading.LoadSceneAsync("Credits");
                    StartCoroutine(closeConfirmMenu());
                }
            }
        }
    }

    IEnumerator openConfirmMenu(bool dialoguePrompt){
        inputAllowed = false;
        sound.sfx.PlaySFX(sound.sfx.ui_confirm);
        if(dialoguePrompt){
            confirmBoxCancelButton.SetActive(false);
            isDialogue = true;
            confirmBoxAcceptButton.transform.localPosition = new Vector3(0, confirmBoxAcceptButton.transform.localPosition.y, confirmBoxAcceptButton.transform.localPosition.z);
        }else{
            confirmBoxCancelButton.SetActive(true);
            isDialogue = false;
            confirmBoxAcceptButton.transform.localPosition = new Vector3(-407, confirmBoxAcceptButton.transform.localPosition.y, confirmBoxAcceptButton.transform.localPosition.z);
        }
        confirmBoxAnimator.Play("appear", 0, 0.0f);
        yield return new WaitForSeconds(confirmBoxAnimator.GetCurrentAnimatorClipInfo(0).Length);
        
        inConfirmMenu = true;
        inputAllowed = true;

        yield return null;
    }
    IEnumerator closeConfirmMenu(){
        sound.sfx.PlaySFX(sound.sfx.ui_cancel);
        inputAllowed = false;
        
        confirmBoxAnimator.Play("fade", 0, 0.0f);
        yield return new WaitForSeconds(confirmBoxAnimator.GetCurrentAnimatorClipInfo(0).Length);
        
        inConfirmMenu = false;
        inputAllowed = true;

        yield return null;
    }

    void OnCancel(){
        if(inputAllowed){
            if(!inConfirmMenu){
                // lets check to see if they are on confirm. if not, lets bring them there
                if(selected != 0){
                    sectionAnimators[selected].Play("deSelect", 0, 0.0f);
                    sectionAnimators[0].Play("select", 0, 0.0f);
                    sound.sfx.PlaySFX(sound.sfx.ui_cancel);
                    selected = 0;
                }else{
                    // user pressed back on confirm. Lets ask them if they are wanting to cancel changes
                    if(checkForChanges() == false){
                        // user is leaving without making changes
                        confirmBoxText.text = "Exit without making changes?";
                        StartCoroutine(openConfirmMenu(false));
                    }else{
                        // user is cancelling their changes
                        confirmBoxText.text = "Revert your changes and return to menu?";
                        StartCoroutine(openConfirmMenu(false));
                    }
                }
            }else{
                if(!isDialogue){
                    // player declined in the confirm window
                    StartCoroutine(closeConfirmMenu());
                }
                
            }
        }
    }

    bool checkForChanges(){
        if(sfxVolume == data.sfxVolume && musicVolume == data.musicVolume 
        && quality == data.quality && useDayCycle == data.useDayCycle 
        && buttonIconID == data.buttonIconID){
            return false;
        }else{
            return true;
        }
    }
}