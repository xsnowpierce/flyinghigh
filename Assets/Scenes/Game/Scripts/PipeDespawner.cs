using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDespawner : MonoBehaviour
{
    private Camera mainCamera;
    public float maxZDistance;
    private PipeSpawner spawner;
    public PlayerStats stats;
    void Awake(){
        mainCamera = Camera.main;
        spawner = GameObject.FindObjectOfType<PipeSpawner>();
        stats = GameObject.FindObjectOfType<PlayerStats>();
    }
    void Update(){
        if(mainCamera.transform.position.z - this.gameObject.transform.position.z  >= maxZDistance || mainCamera.transform.position.z - this.gameObject.transform.position.z  <= -maxZDistance){
            spawner.pipesList.Remove(transform.parent.gameObject);
            GameObject.Destroy(transform.parent.gameObject);           
        }
    }
    void OnTriggerEnter(){
        stats.IncreasePipes();
        spawner.SpawnPipe();
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
