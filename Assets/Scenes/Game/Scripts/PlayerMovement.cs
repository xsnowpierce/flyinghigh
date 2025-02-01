using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [HideInInspector]
    public Rigidbody body;
    public float forceMultiplier;
    public float jumpCooldownTime = 0.3f;
    public float jumpCooldown = 0.0f;
    public float jumpCooldownDeadzone = 0.001f;
    public float maxJumpHeight = 33;
    public Vector3 gravity;
    public GameObject birdObject;
    public Vector3 birdRotationAmount;
    public Quaternion birdRotationDefault;
    private CameraAutoMove move;
    private PipeSpawner spawner;
    public GameObject UIDisable;
    public GameObject splashCanvas;
    public LoadingManager loading;
    public bool isDead;
    private SunController sun;
    public SoundController sound;

    void Awake(){
        loading = GameObject.FindObjectOfType<LoadingManager>();
        sound = GameObject.FindObjectOfType<SoundController>();
        sun = GameObject.FindObjectOfType<SunController>();
        move = GameObject.FindObjectOfType<CameraAutoMove>();
        spawner = GameObject.FindObjectOfType<PipeSpawner>();
        body = gameObject.GetComponent<Rigidbody>();
        Physics.gravity = Vector3.zero;
    }

    void Update(){
        if(move.isMoving){
            if(jumpCooldown > 0.0f){
                jumpCooldown -= Time.deltaTime;
            }
            if(jumpCooldown >= -jumpCooldownDeadzone && jumpCooldown <= jumpCooldownDeadzone){
                jumpCooldown = 0.0f;
            }
            if(jumpCooldown < 0.0f){
                jumpCooldown = 0.0f;
            }
            if(birdObject.transform.rotation.x < 90){
                birdObject.transform.Rotate(birdRotationAmount * Time.deltaTime, Space.Self);
            }
            Physics.gravity = gravity;
        }
    }

    public void StartGame(){
        UIDisable.SetActive(true);
        Physics.gravity = gravity;
        spawner.SpawnPipe();
        move.isMoving = true;
        sun.move = true;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        
    }

    public void PauseGame(){
        UIDisable.SetActive(false);
        Physics.gravity = Vector3.zero;
        move.isMoving = false;
        body.velocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnJump(){
        if(jumpCooldown == 0.0f && gameObject.transform.localPosition.y < maxJumpHeight && !loading.isLoading){

            if(!isDead){
                
                if(!move.isMoving){
                    // player just started the game
                    splashCanvas.SetActive(false);
                    StartGame();
                }else{
                    sound.sfx.PlaySFX(sound.sfx.game_jumpSound);
                }

                body.velocity = Vector3.zero;
                //body.AddForce(Vector3.up * forceMultiplier);
                body.velocity = new Vector3(0,1 * forceMultiplier,0);
                jumpCooldown = jumpCooldownTime;

                // reset the rotation of the bird
                birdObject.transform.rotation = birdRotationDefault;

                // play the jump animation
                birdObject.GetComponent<Animator>().CrossFade("Flap", 0.03f, 0, 0.0f, 0.0f);
            }
            
        }
    }
}
