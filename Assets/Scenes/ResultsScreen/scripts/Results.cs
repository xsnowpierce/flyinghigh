using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class Results : MonoBehaviour
{
    private LoadingManager loading;
    private InputActionMap asset;
    public TMP_Text modeText;
    public TMP_Text bestRunText;
    public TMP_Text thisRunText;
    public TMP_Text coinText;
    public TMP_Text highscoreText;
    public string congratsText;
    public string tiedText;
    public string misplacedText;
    private SaveDataLoaded data;
    public bool canInteract = true;
    public Texture2D easyTexture;
    public Texture2D hardTexture;
    public RawImage background;
    public Button[] buttons;
    public ResultsEyeController controller;

    void Awake(){
        loading = GameObject.FindObjectOfType<LoadingManager>();
        asset = gameObject.GetComponent<PlayerInput>().currentActionMap;
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        LoadData();
    }

    void Start(){
        
    }

    void OnSelect(){
        // player pressed retry
        Progress("Game");
    }

    void OnCancel(){
        // player pressed menu
        Progress("Main Menu");
    }

    void LoadData(){

        MusicController music = GameObject.FindObjectOfType<MusicController>();
        
        if(data.hardMode){
            modeText.text = "HARD";
            bestRunText.text = data.bestHardRun + "";
            thisRunText.text = data.thisHardRun + "";
            if(data.thisHardRun > data.bestHardRun){
                highscoreText.text = congratsText;
                controller.position = ResultsEyeController.EyePosition.Happy;
                music.PlayIntroLoopSong(music.resultsWinIntro, music.resultsWinLoop);
            }else{
                if(data.thisHardRun == data.bestHardRun){
                    highscoreText.text = tiedText;
                    controller.position = ResultsEyeController.EyePosition.Normal;
                    music.InterruptSong(music.resultsLost, false);
                }else if (data.thisHardRun < data.bestHardRun){
                    highscoreText.text = misplacedText;
                    controller.position = ResultsEyeController.EyePosition.Dead;
                    music.InterruptSong(music.resultsLost, false);
                }
            }
            background.texture = hardTexture;
        }
        if(!data.hardMode){
            modeText.text = "EASY";
            bestRunText.text = data.bestEasyRun + "";
            thisRunText.text = data.thisEasyRun + "";
            if(data.thisEasyRun > data.bestEasyRun){
                highscoreText.text = congratsText;
                controller.position = ResultsEyeController.EyePosition.Happy;
                music.PlayIntroLoopSong(music.resultsWinIntro, music.resultsWinLoop);
            }else{
                if(data.thisEasyRun == data.bestEasyRun){
                    highscoreText.text = tiedText;
                    controller.position = ResultsEyeController.EyePosition.Normal;
                    music.InterruptSong(music.resultsLost, false);
                }else if (data.thisEasyRun < data.bestEasyRun){
                    highscoreText.text = misplacedText;
                    controller.position = ResultsEyeController.EyePosition.Dead;
                    music.InterruptSong(music.resultsLost, false);
                }
            }
            background.texture = easyTexture;
        }
        coinText.text = data.thisRunCoins + "";
    }

    public void Progress(string scene){
        if(canInteract){
            WriteToTempData();
            canInteract = false;
            if(scene == "Main Menu"){
                if(data.useSaveFile){
                    data.SaveGame();
                }
            }
            loading.LoadSceneAsync(scene);
        }
        
    }

    void WriteToTempData(){
        if(data.hardMode){
            if(data.thisHardRun > data.bestHardRun){
                data.bestHardRun = data.thisHardRun;
                data.totalHardDistance += data.thisRunHardDistance;
            }
        }else{
            if(data.thisEasyRun > data.bestEasyRun){
                data.bestEasyRun = data.thisEasyRun;
                data.totalEasyDistance += data.thisRunEasyDistance;
            }
        }
        data.totalCoins += data.thisRunCoins;
    }

}