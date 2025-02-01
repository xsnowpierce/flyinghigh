using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerStats : MonoBehaviour
{
    public int goals;
    public int coins;
    public float distance;
    public float convertRate = 7;
    public BirdsEyeController eyeController;
    public TMP_Text goalText;
    public TMP_Text coinText;
    private DifficultyMode difficulty;
    public SoundController sound;
    public PlayerMovement movement;

    void Awake(){
        movement = GameObject.FindObjectOfType<PlayerMovement>();
        sound = GameObject.FindObjectOfType<SoundController>();
        difficulty = this.gameObject.GetComponent<DifficultyMode>();
    }

    void Start(){
        if(difficulty.inHardMode){
            sound.music.PlayIntroLoopSong(sound.music.hardGameIntro, sound.music.hardGameLoop);
        }else{
            sound.music.PlayIntroLoopSong(sound.music.easyGameIntro, sound.music.easyGameLoop);
        }
    }

    void Update(){
        distance = (float) Math.Round(this.gameObject.transform.parent.position.z / convertRate, 2);
    }

    public void IncreasePipes(){
        if(!movement.isDead){
            goals++;
            goalText.text = goals + "";
            sound.sfx.PlaySFX(sound.sfx.game_goalPass);
            if(difficulty.inHardMode && goals % difficulty.speedIncreaseAt == 0){
                difficulty.SpeedIncrease();
            }else{
                difficulty.goalsUntilSpeedup --;
                if(difficulty.goalsUntilSpeedup > 1){
                    difficulty.goalsUntilSpeedupText.text = difficulty.goalsUntilSpeedup + " GOALS UNTIL NEXT SPEEDUP";
                }else{
                    difficulty.goalsUntilSpeedupText.text = difficulty.goalsUntilSpeedup + " GOAL UNTIL NEXT SPEEDUP";
                }
            }
        }
    }

    public void IncreaseCoins(){
        sound.sfx.PlaySFX(sound.sfx.game_coinSound);
        eyeController.MoneyEffect();
        coins++;
        coinText.text = "" + coins;
    }
}
