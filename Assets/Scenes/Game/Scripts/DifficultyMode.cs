using System.Collections;
using TMPro;
using UnityEngine;

public class DifficultyMode : MonoBehaviour
{
    private SaveDataLoaded data;
    public bool inHardMode;
    public Animator speedIncreaseAnimator;
    public int speedIncreaseAt = 10;
    public float speedIncreaseModifier = 0.25f;
    public float currentSpeed = 1.0f;
    public int speedIncreases;
    public GameObject speedupParent;
    public int goalsUntilSpeedup = 5;
    public TMP_Text goalsUntilSpeedupText;
    private PlayerMovement movement;
    private CameraAutoMove autoMove;
    public float speedupTextSpeed;
    public float maxSpeedup;
    public SoundController sound;
    void Awake(){
        sound = GameObject.FindObjectOfType<SoundController>();
        movement = this.gameObject.GetComponent<PlayerMovement>();
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        autoMove = GameObject.FindObjectOfType<CameraAutoMove>();
        if(data != null){
            if(data.hardMode){
                speedupParent.SetActive(true);
                inHardMode = true;
            } else {
                speedupParent.SetActive(false);
                inHardMode = false;
            }
        }else{
            if(inHardMode == true){
                speedupParent.SetActive(true);
            }else speedupParent.SetActive(false);
        }
        goalsUntilSpeedup = speedIncreaseAt;
        goalsUntilSpeedupText.text = goalsUntilSpeedup + " GOALS UNTIL NEXT SPEEDUP";
    }

    public void SpeedIncrease(){
        StartCoroutine(speedUp());
    }

    IEnumerator speedUp(){
        if(!movement.isDead){
            sound.sfx.PlaySFX(sound.sfx.game_speedUpSound);
            goalsUntilSpeedup = speedIncreaseAt;
            goalsUntilSpeedupText.text = speedIncreaseAt + " GOALS UNTIL NEXT SPEEDUP";
            speedIncreases ++;
            float tempint;

            if(currentSpeed * speedIncreaseModifier <= maxSpeedup){
                Time.timeScale = Time.timeScale * speedIncreaseModifier;
                tempint = currentSpeed * speedIncreaseModifier;
            }else{
                Time.timeScale = maxSpeedup;
                tempint = maxSpeedup;
            }

            speedIncreaseAnimator.PlayInFixedTime("speedup", 0, 0.0f);

            yield return new WaitForSecondsRealtime(0.05f);

            // subtract current speed and target speed, and then divide difference?
            float incrementSize = (tempint - currentSpeed) / speedupTextSpeed;

            // increase timer
            while(currentSpeed < tempint){
                currentSpeed += incrementSize * Time.unscaledDeltaTime;
                speedIncreaseAnimator.GetComponent<TMP_Text>().text = "" + currentSpeed.ToString("#.00") + "x";
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.5f);
            speedIncreaseAnimator.PlayInFixedTime("winddown", 0, 0.0f);
        }
        yield return null;
    }

}
