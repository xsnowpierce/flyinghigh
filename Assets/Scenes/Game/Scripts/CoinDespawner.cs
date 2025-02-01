using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDespawner : MonoBehaviour
{
    public Camera mainCamera;
    public int maxZDistance;
    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(mainCamera.transform.position.z - this.gameObject.transform.position.z  >= maxZDistance || mainCamera.transform.position.z - this.gameObject.transform.position.z  <= -maxZDistance){
            GameObject.Destroy(this.gameObject);
        }
    }
}
