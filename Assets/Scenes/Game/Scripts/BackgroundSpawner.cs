using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    private Camera mainCamera;
    public float maxZDistance;
    private PipeSpawner spawner;
    public float spawnAtZDistance;
    public bool spawned;
    void Awake(){
        mainCamera = Camera.main;
        spawner = GameObject.FindObjectOfType<PipeSpawner>();
    }
    void Update(){
        if(this.gameObject.transform.position.z  <= maxZDistance){
            spawner.backgroundList.Remove(this.gameObject);
            GameObject.Destroy(this.gameObject);
        }
        if(mainCamera.transform.position.z - this.gameObject.transform.position.z  >= spawnAtZDistance && spawned == false){
            spawner.SpawnBackground(); 
            spawned = true;
        }
    }
}
