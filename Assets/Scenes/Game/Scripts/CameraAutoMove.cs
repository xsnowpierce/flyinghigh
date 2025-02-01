using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public bool isMoving;
    public float moveSpeed;
    public GameObject pipes;
    public GameObject coins;
    public GameObject background;

    void Update(){
        if(isMoving){
            foreach (Transform child in pipes.transform){
                child.transform.Translate((Vector3.back * moveSpeed) * Time.deltaTime, Space.World);
            }
            foreach (Transform child in coins.transform){
                child.transform.Translate((Vector3.back * moveSpeed) * Time.deltaTime, Space.World);
            }
            foreach (Transform child in background.transform){
                child.transform.Translate((Vector3.back * moveSpeed) * Time.deltaTime, Space.World);
            }
        }
    }
}
