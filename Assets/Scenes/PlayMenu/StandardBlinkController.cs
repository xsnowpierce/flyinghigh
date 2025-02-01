using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBlinkController : MonoBehaviour
{
    public GameObject[] blinkParent;
    public GameObject[] normalParent;
    public enum EyePosition{
        Blink,
        Normal
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
            StartCoroutine(Blink());
        }
        if(blinkCooldown > 0.0f){
            blinkCooldown -= Time.deltaTime;
        }
        if((blinkCooldown <= blinkDeadzone && blinkCooldown >= -blinkDeadzone) || blinkCooldown < 0.0f){
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
        for(int i = 0; i < normalParent.Length; i++){
            switch(position){
                case EyePosition.Blink:
                    normalParent[i].SetActive(false);
                    blinkParent[i].SetActive(true);
                    break;
                case EyePosition.Normal:
                    blinkParent[i].SetActive(false);
                    normalParent[i].SetActive(true);
                    break;
            }
        }
    }
}
