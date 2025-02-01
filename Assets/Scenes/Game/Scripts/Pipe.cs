using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pipe : MonoBehaviour
{
    public GameObject birdObject;
    public AnimationClip deathClip;
    public BirdsEyeController eyeController;
    public Animator transitionLayer;
    public AnimationClip fadeLayerClip;
    public GameObject UIDisable;
    public GameObject deathImage;
    public GameObject flashImage;
    public AnimationClip flashClip;
    private SaveDataLoaded data;
    private PlayerStats stats;
    public SoundController sound;

    void Awake(){
        sound = GameObject.FindObjectOfType<SoundController>();
        data = GameObject.FindObjectOfType<SaveDataLoaded>();
        stats = GameObject.FindObjectOfType<PlayerStats>();
    }
    void OnCollisionEnter(Collision other){
        StoreData();
        StartCoroutine(birdDie());
    }
    void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void StoreData(){
        if(data.hardMode){
            data.thisHardRun = stats.goals;
            data.thisRunHardDistance = stats.distance;
        }else{
            data.thisEasyRun = stats.goals;
            data.thisRunEasyDistance = stats.distance;
        }
        data.thisRunCoins = stats.coins;
    }

    IEnumerator birdDie(){
        
        MusicController music = GameObject.FindObjectOfType<MusicController>();
        music.GetComponent<AudioSource>().Stop();
        
        Time.timeScale = 1.0f;

        sound.sfx.StopSFX();
        sound.sfx.PlaySFX(sound.sfx.game_hitSound);
        PlayerMovement movement = GameObject.FindObjectOfType<PlayerMovement>();
        movement.isDead = true;
        movement.PauseGame();

        eyeController.position = BirdsEyeController.EyePosition.Dead;
        birdObject.transform.LookAt(Camera.main.transform);
        birdObject.GetComponent<Animator>().Play(deathClip.name, 0, 0.0f);

        SetLayerRecursively(birdObject, 6);
        deathImage.SetActive(true);

        yield return new WaitForSeconds(deathClip.length);
        transitionLayer.Play("fade", 0, 0.0f);
        yield return new WaitForSeconds(fadeLayerClip.length);
        flashImage.GetComponent<Animator>().Play("appear", 0, 0.0f);
        yield return new WaitForSeconds(flashClip.length);
        SceneManager.LoadSceneAsync("ResultsScreen", LoadSceneMode.Single);
    }
}
