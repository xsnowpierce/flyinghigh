using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public bool move;
    public Light sun;
    public float rotateSpeed;
    [Space(10)]
    public float intensity;
    public bool fadeTransitionStarted;
    public bool raiseTransitionStarted;
    public float transitionValue;
    public Material cloudMaterial;

    [Space(20)]
    public Vector3 fadeIntensityStart;
    public Vector3 fadeIntensityEnd;
    [Space(10)]
    public Vector3 raiseIntensityStart;
    public Vector3 raiseIntensityEnd;
    [Space(10)]
    public float raiseIntensityDifference;
    public float fadeIntensityDifference;
    public float fadeValue;
    public float raiseValue;
    [Space(10)]
    public Vector3 disabledRotation;
    [Space(20)]
    public Vector3 xrot;
    private SaveDataLoaded data;
    public float fadeIncrementBy;
    public float raiseIncrementBy;

    void Awake(){
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        fadeIntensityDifference = (360 - fadeIntensityStart.x) + fadeIntensityEnd.x;
        raiseIntensityDifference = (360 - raiseIntensityEnd.x) + raiseIntensityStart.x;
    }
    void Update(){

        if(data.useDayCycle){
            xrot = gameObject.transform.rotation.eulerAngles;

            intensity = Mathf.Clamp(transitionValue + 0.4f, 0.0f, 1.0f);

            sun.intensity = intensity;
            sun.bounceIntensity = intensity;
            sun.shadowStrength = intensity;
            RenderSettings.ambientIntensity = intensity;
            cloudMaterial.SetColor("_MainColor", Color.white * intensity);
            // transform rotation of sun

            fadeIncrementBy = fadeValue / 50;
            raiseIncrementBy = raiseValue / 50;

            if(xrot.x > 180){
                fadeValue = xrot.x - fadeIntensityStart.x;
            }else{
                fadeValue = xrot.x + (360 - fadeIntensityStart.x);
            }

            if(xrot.x < 180){
                raiseValue = raiseIntensityStart.x - xrot.x;
            }else{
                raiseValue = 360 - xrot.x + raiseIntensityStart.x;
            }

            if(move){
                this.gameObject.transform.Rotate(new Vector3(-rotateSpeed * Time.unscaledDeltaTime,0,0), Space.World);
            }

            if(xrot.y == 180 && xrot.z == 180){

                // sun is on the left side of the screen
                if(xrot.x >= fadeIntensityStart.x){
                    // start fade
                    fadeTransitionStarted = true;
                }
                if(xrot.x >= fadeIntensityEnd.x && xrot.x < 180){
                    // end fade
                    fadeTransitionStarted = false;
                }

            }else if(xrot.y == 0 && xrot.z == 0){
                if(xrot.x <= raiseIntensityStart.x){
                    // start raise
                    raiseTransitionStarted = true;
                }
                if(xrot.x <= raiseIntensityEnd.x && xrot.x > 180){
                    // end raise
                    raiseTransitionStarted = false;
                }
                // sun is on the right side of the screen
            }

            if(fadeTransitionStarted){

                Debug.Log(fadeIncrementBy);

                transitionValue = Mathf.Clamp(1 - (fadeIncrementBy), 0.0f, 1.0f);
            }
            if(raiseTransitionStarted){
                
                Debug.Log(raiseIncrementBy);

                transitionValue = Mathf.Clamp(raiseIncrementBy, 0.0f, 1.0f);

            }
        }else{
            // don't use day night cycle
            gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(disabledRotation.x, disabledRotation.y, disabledRotation.z));
        }
    }
}
