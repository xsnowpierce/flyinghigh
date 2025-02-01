using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTextureModifier : MonoBehaviour
{
    public Material[] bird1Materials;
    public Material[] bird2Materials;
    [Space(10)]
    public float bird1Fade;
    public float bird2Fade;

    void Update(){
        for(int i = 0; i < bird1Materials.Length; i++){
            bird1Materials[i].SetFloat("_BlackValue", bird1Fade);
        }
        for(int i = 0; i < bird2Materials.Length; i++){
            bird2Materials[i].SetFloat("_BlackValue", bird2Fade);
        }
    }
    public void FadeBird1(float targetValue, float speed){
        StartCoroutine(fade1Bird(targetValue, speed));
    }

    IEnumerator fade1Bird(float targetValue, float speed){
        while(bird1Fade != targetValue){
            if(bird1Fade > targetValue){
                bird1Fade =  Mathf.Clamp(bird1Fade - (speed * Time.deltaTime), 0.0f, 1.0f);
            }else if(bird1Fade < targetValue){
                bird1Fade =  Mathf.Clamp(bird1Fade + (speed * Time.deltaTime), 0.0f, 1.0f);
            }
            yield return null;
        }
        yield return null;
    }
    public void FadeBird2(float targetValue, float speed){
        StartCoroutine(fade2Bird(targetValue, speed));
    }

    IEnumerator fade2Bird(float targetValue, float speed){
        while(bird2Fade != targetValue){
            if(bird2Fade > targetValue){
                bird2Fade =  Mathf.Clamp(bird2Fade - (speed * Time.deltaTime), 0.0f, 1.0f);
            }else if(bird2Fade < targetValue){
                bird2Fade =  Mathf.Clamp(bird2Fade + (speed * Time.deltaTime), 0.0f, 1.0f);
            }
            yield return null;
        }
        yield return null;
    }

}
