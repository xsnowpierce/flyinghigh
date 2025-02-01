using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PipeSpawner : MonoBehaviour
{
    
    public float lowY;
    public float highY;
    public GameObject cam;
    [Space(10)]
    public GameObject easyPipePrefab;
    public GameObject hardPipePrefab;
    public GameObject pipePrefab;
    public ArrayList pipesList = new ArrayList();
    public int startingPipeDistance;
    public int pipeDistance;
    public GameObject pipeParent;
    public int maxPipes;
    public int spawnPipesAtTime = 2;
    public int spawnPipes = 2;
    [Space(20)]
    public GameObject coinPrefab;
    public GameObject coinParent;
    public float coinChance = 0.6f;
    public float coinDistance = 10;
    [Space(20)]
    public GameObject backgroundPrefab;
    public ArrayList backgroundList = new ArrayList();
    public GameObject backgroundParent;
    public float backgroundDistance;
    public int spawnBackgroundsAtTime = 2;
    public int spawnBackgrounds = 2;

    void Awake(){

        if(GameObject.FindObjectOfType<SaveDataLoaded>() != null){
            SaveDataLoaded data = GameObject.FindObjectOfType<SaveDataLoaded>();
            if(data.hardMode){
                pipePrefab = hardPipePrefab;
            }else {
                pipePrefab = easyPipePrefab;
            }
        }else{
            pipePrefab = easyPipePrefab;
        }
        SpawnBackground();
    }
    public void SpawnPipe(){
        if(pipesList.Count == 0){
            // beginning of the game, spawn a pipe at the fixed distance
            // Lets get a random Y, using a random number between lowY and highY
            int yPos = (int) Random.Range(lowY, highY);
            GameObject newPipe = GameObject.Instantiate(pipePrefab, new Vector3(0,yPos,cam.transform.position.z + startingPipeDistance), Quaternion.identity, pipeParent.transform);
            pipesList.Add(newPipe);
            TrySpawningCoin(newPipe);
            // make i = 1 here so we are including the one we start off with
            for(int i = 1; i <= spawnPipes; i++){
                GameObject lastPipe = (GameObject) pipesList[pipesList.Count - 1];
                GameObject morePipe = GameObject.Instantiate(pipePrefab, new Vector3(0, yPos, lastPipe.transform.position.z + pipeDistance), Quaternion.identity, pipeParent.transform);
                pipesList.Add(morePipe);
                TrySpawningCoin(morePipe);
            }
        }else if(pipesList.Count > 0){
            int yPos = (int) Random.Range(lowY, highY);
            for(int i = 1; i <= spawnPipesAtTime; i++){
                GameObject lastPipe = (GameObject) pipesList[pipesList.Count - 1];
                GameObject newPipe = GameObject.Instantiate(pipePrefab, new Vector3(0, yPos, lastPipe.transform.position.z + pipeDistance), Quaternion.identity, pipeParent.transform);
                pipesList.Add(newPipe);
                TrySpawningCoin(newPipe);
            }
        }
    }

    public void SpawnBackground(){
        if(backgroundList.Count == 0){
            // beginning of the game, spawn a pipe at the fixed distance
            // Lets get a random Y, using a random number between lowY and highY
            GameObject newBackground = GameObject.Instantiate(backgroundPrefab, new Vector3(124.7f,49.72f,0), Quaternion.identity, backgroundParent.transform);
            backgroundList.Add(newBackground);
            // make i = 1 here so we are including the one we start off with
            for(int i = 1; i <= spawnBackgrounds; i++){
                GameObject lastBackground = (GameObject) backgroundList[backgroundList.Count - 1];
                GameObject moreBackground = GameObject.Instantiate(backgroundPrefab, new Vector3(124.7f,49.72f, lastBackground.transform.position.z + backgroundDistance), Quaternion.identity, backgroundParent.transform);
                backgroundList.Add(moreBackground);
            }
        }else if(backgroundList.Count > 0){
            for(int i = 1; i <= spawnBackgroundsAtTime; i++){
                GameObject lastBackground = (GameObject) backgroundList[backgroundList.Count - 1];
                GameObject newBackground = GameObject.Instantiate(backgroundPrefab, new Vector3(124.7f,49.72f, lastBackground.transform.position.z + backgroundDistance), Quaternion.identity, backgroundParent.transform);
                backgroundList.Add(newBackground);
            }
        }
    }

    public void TrySpawningCoin(GameObject relativeTo){
        float random = Random.Range(0.0f, 1.0f);
        if(random <= coinChance){
            // spawn a coin!
            int yPos = (int) Random.Range(lowY, highY);
            GameObject coin = GameObject.Instantiate(coinPrefab, new Vector3(0, yPos, relativeTo.transform.position.z + coinDistance), Quaternion.identity, coinParent.transform);
        }
    }
}
