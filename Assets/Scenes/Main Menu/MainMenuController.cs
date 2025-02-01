using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private LoadingManager loading;
    private SoundController sound;
    private PlayerInput input;
    public bool inputAllowed;
    public int menuSelected = 0;
    [Space(10)]
    public Animator titleAnimator;
    public Animator menuAnimator;
    public Animator cameraAnimator;
    public Animator selectAnimator;
    public Animator[] buttonAnimators;
    public GameObject pipesParent;
    public bool started;
    public bool inMenu;
    public bool inSelectMenu;
    [Space(10)]
    public AnimationClip cameraStartMove;
    public AnimationClip title_Appear;
    public AnimationClip title_Disappear;
    public AnimationClip cameraGameTurn;
    public AnimationClip cameraTitleTurn;
    public AnimationClip menu_Appear;
    public AnimationClip menu_Disappear;
    public AnimationClip button_Select;
    public AnimationClip button_DeSelect;
    public AnimationClip select_Appear;
    public AnimationClip select_Disappear;

    void Awake(){
        sound = GameObject.FindObjectOfType<SoundController>();
        input = GameObject.FindObjectOfType<PlayerInput>();
        loading = GameObject.FindObjectOfType<LoadingManager>();
        pipesParent.SetActive(false);
        inputAllowed = false;
        if(sound.music.source.loop == false){
            sound.music.source.loop = true;
        }
    }

    void Update(){
        if(!loading.isLoading && started == false){
            StartCoroutine(start());
            started = true;
        }
    }

    IEnumerator start(){
        if(sound.music.source.clip != sound.music.mainMenu){
            sound.music.CrossFadeSong(sound.music.mainMenu, false, true);
        }
        cameraAnimator.Play(cameraStartMove.name, 0, 0.0f);
        yield return new WaitForSeconds(cameraStartMove.length);

        pipesParent.SetActive(true);

        titleAnimator.Play(title_Appear.name, 0, 0.0f);
        yield return new WaitForSeconds(title_Appear.length);
        inputAllowed = true;

        yield return null;
    }

    void OnAnyKey(){
        if(!inSelectMenu && !inMenu && !loading.isLoading && inputAllowed){
            StartCoroutine(openMenu());
        }
    }
    void OnSelect(){
        if(!inSelectMenu && inMenu && !loading.isLoading && inputAllowed){
            // user clicked one of the buttons
            if(menuSelected == 0){
                // player pressed play
                sound.sfx.PlaySFX(sound.sfx.ui_confirm);
                loading.LoadSceneAsync("PlayMenu");
                inputAllowed = false;
            }
            else if(menuSelected == 1){
                // player clicked options
                sound.sfx.PlaySFX(sound.sfx.ui_confirm);
                loading.LoadSceneAsync("Settings");
                inputAllowed = false;
            }
            else if(menuSelected == 2){
                // player clicked quit game, prompt for confirmation
                StartCoroutine(openSelectMenu());
            }
        }
        if(inSelectMenu && inMenu && !loading.isLoading && inputAllowed){
            // player closed the game
            sound.sfx.PlaySFX(sound.sfx.ui_confirm);
            Application.Quit();
        }
    }

    IEnumerator openMenu(){
        
        inputAllowed = false;
        buttonAnimators[0].Play(button_Select.name, 0, 0.0f);
        
        sound.sfx.PlaySFX(sound.sfx.ui_confirm);
        titleAnimator.Play(title_Disappear.name, 0, 0.0f);
        yield return new WaitForSeconds(title_Disappear.length);

        cameraAnimator.Play(cameraGameTurn.name, 0, 0.0f);
        yield return new WaitForSeconds(cameraGameTurn.length);

        menuAnimator.Play(menu_Appear.name, 0, 0.0f);
        yield return new WaitForSeconds(menu_Appear.length);

        inMenu = true;
        inputAllowed = true;
        yield return null;
    }

    void OnCancel(){
        if(!inSelectMenu && inMenu && !loading.isLoading && inputAllowed){
            StartCoroutine(closeMenu());
            sound.sfx.PlaySFX(sound.sfx.ui_cancel);
            buttonAnimators[menuSelected].CrossFade(button_DeSelect.name, 0.1f, 0, 0.0f, 0.0f);
            menuSelected = 0;
        }
        if(inSelectMenu && inMenu && !loading.isLoading && inputAllowed){
            StartCoroutine(closeSelectMenu());
        }
    }
    IEnumerator closeMenu(){
        inputAllowed = false;
        inMenu = false;

        menuAnimator.Play(menu_Disappear.name, 0, 0.0f);
        yield return new WaitForSeconds(menu_Disappear.length);

        cameraAnimator.Play(cameraTitleTurn.name, 0, 0.0f);
        yield return new WaitForSeconds(cameraTitleTurn.length);

        titleAnimator.Play(title_Appear.name, 0, 0.0f);
        yield return new WaitForSeconds(title_Appear.length);

        inputAllowed = true;
        yield return null;
    }

    void OnNavigate(){
        if(!inSelectMenu && inMenu && !loading.isLoading && inputAllowed){
            float yval = input.actions["Navigate"].ReadValue<Vector2>().y;
            if(yval == 1){
                // player moved up
                if(menuSelected > 0){
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    buttonAnimators[menuSelected - 1].Play(button_Select.name, 0, 0.0f);
                    buttonAnimators[menuSelected].CrossFade(button_DeSelect.name, 0.1f, 0, 0.0f, 0.0f);
                    menuSelected --;
                }else{
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    buttonAnimators[buttonAnimators.Length - 1].Play(button_Select.name, 0, 0.0f);
                    buttonAnimators[menuSelected].CrossFade(button_DeSelect.name, 0.1f, 0, 0.0f, 0.0f);
                    menuSelected = buttonAnimators.Length - 1;
                }
            }else if(yval == -1){
                // player moved down
                if(menuSelected < buttonAnimators.Length - 1){
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    buttonAnimators[menuSelected + 1].Play(button_Select.name, 0, 0.0f);
                    buttonAnimators[menuSelected].CrossFade(button_DeSelect.name, 0.1f, 0, 0.0f, 0.0f);
                    menuSelected ++;
                }else{
                    sound.sfx.PlaySFX(sound.sfx.ui_select);
                    buttonAnimators[0].Play(button_Select.name, 0, 0.0f);
                    buttonAnimators[menuSelected].CrossFade(button_DeSelect.name, 0.1f, 0, 0.0f, 0.0f);
                    menuSelected = 0;
                }
            }
        }
    }

    IEnumerator openSelectMenu(){
        inputAllowed = false;
        inSelectMenu = true;

        sound.sfx.PlaySFX(sound.sfx.ui_confirm);
        selectAnimator.Play(select_Appear.name, 0, 0.0f);
        yield return new WaitForSeconds(select_Appear.length);

        inputAllowed = true;
        yield return null;
    }

    IEnumerator closeSelectMenu(){
        inputAllowed = false;
        inSelectMenu = true;

        sound.sfx.PlaySFX(sound.sfx.ui_cancel);
        selectAnimator.Play(select_Disappear.name, 0, 0.0f);
        yield return new WaitForSeconds(select_Disappear.length);

        inputAllowed = true;
        inSelectMenu = false;
        yield return null;
    }
}
