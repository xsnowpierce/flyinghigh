using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGrabber : MonoBehaviour
{
    public PlayerStats stats;
    public AnimationClip collectClip;
    public float transitionDuration = 0.3f;
    public float timeOffset = 0.0f;
    public float transitionTime = 0.0f;

    void OnEnable(){
        stats = GameObject.FindObjectOfType<PlayerStats>();
    }
    
    void OnTriggerEnter(){
        stats.IncreaseCoins();
        GameObject.Destroy(this.gameObject, collectClip.length);
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        anim.CrossFadeInFixedTime("collect", transitionDuration, 0, timeOffset, transitionTime);
    }
}
