using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsEyeController : MonoBehaviour
{
    public GameObject blinkParent;
    public GameObject deadParent;
    public GameObject happyParent;
    public GameObject normalParent;
    public GameObject moneyParent;
    public enum EyePosition{
        Blink,
        Dead,
        Happy,
        Normal,
        Money
    }
    public EyePosition position;
    [Space(20)]
    public float blinkCooldownMax = 3.0f;
    public float blinkCooldownMin = 0.3f;
    private float blinkCooldownRandom;
    public float blinkCooldown;
    public float blinkDuration;
    public float blinkDeadzone;
    void Awake(){
        UpdateEyes();
    }
    void Update(){
        if(blinkCooldown == 0.0f && position != EyePosition.Blink){
            if(position != EyePosition.Dead){
                StartCoroutine(Blink());
            }
        }
        if(blinkCooldown > 0.0f){
            blinkCooldown -= Time.deltaTime;
        }
        if(blinkCooldown <= blinkDeadzone && blinkCooldown >= -blinkDeadzone){
            blinkCooldown = 0.0f;
        }
    }

    IEnumerator Blink(){
        EyePosition previousPosition = position;
        position = EyePosition.Blink;
        UpdateEyes();
        yield return new WaitForSeconds(blinkDuration);
        position = previousPosition;
        UpdateEyes();
        float random = Random.Range(blinkCooldownMin, blinkCooldownMax);
        blinkCooldown = random;
    }

    void UpdateEyes(){
        switch(position){
            case EyePosition.Blink:
                deadParent.SetActive(false);
                happyParent.SetActive(false);
                normalParent.SetActive(false);
                moneyParent.SetActive(false);
                blinkParent.SetActive(true);
                break;
            case EyePosition.Dead:
                blinkParent.SetActive(false);
                happyParent.SetActive(false);
                normalParent.SetActive(false);
                moneyParent.SetActive(false);
                deadParent.SetActive(true);
                break;
            case EyePosition.Happy:
                blinkParent.SetActive(false);
                deadParent.SetActive(false);
                normalParent.SetActive(false);
                moneyParent.SetActive(false);
                happyParent.SetActive(true);
                break;
            case EyePosition.Money:
                blinkParent.SetActive(false);
                deadParent.SetActive(false);
                normalParent.SetActive(false);
                happyParent.SetActive(false);
                moneyParent.SetActive(true);
                break;
            case EyePosition.Normal:
                blinkParent.SetActive(false);
                deadParent.SetActive(false);
                happyParent.SetActive(false);
                moneyParent.SetActive(false);
                normalParent.SetActive(true);
                break;
        }
    }
}
