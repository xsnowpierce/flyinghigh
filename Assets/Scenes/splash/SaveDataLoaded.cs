using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class SaveDataLoaded : MonoBehaviour
{
    public int bestEasyRun = 0;
    public int bestHardRun = 0;
    public int totalCoins = 0;
    public float totalEasyDistance = 0.0f;
    public float totalHardDistance = 0.0f;
    public int thisEasyRun;
    public int thisHardRun;
    public int thisRunCoins;
    public float thisRunEasyDistance = 0.0f;
    public float thisRunHardDistance = 0.0f;
    public int sfxVolume = 50;
    public int musicVolume = 50;
    public bool hasSaveFile;
    public bool useSaveFile;
    public GameObject saveScreen;
    public bool hardMode;
    public int buttonIconID;
    public int quality;
    public bool useDayCycle;
    private QualityController qualityController;

    void Awake(){
        qualityController = GameObject.FindObjectOfType<QualityController>();
        DontDestroyOnLoad(this);
    }

    public bool SaveGameExists(){
        if(File.Exists(Application.persistentDataPath + "/" + Application.productName)){
            return true;
        }else{
            return false;
        }
    }

    public void SaveGame(){
        if(useSaveFile){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + Application.productName);
            SaveData data = new SaveData();
            data.bestEasyRun = bestEasyRun;
            data.bestHardRun = bestHardRun;
            data.totalCoins = totalCoins;
            data.totalEasyDistance = totalEasyDistance;
            data.totalHardDistance = totalHardDistance;
            data.sfxVolume = sfxVolume;
            data.musicVolume = musicVolume;
            data.buttonIconID = buttonIconID;
            data.quality = quality;
            data.useDayCycle = useDayCycle;
            qualityController.UpdateQuality(quality);
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("The game has been saved.");
        }
    }
    public void LoadGame(){
        if(File.Exists(Application.persistentDataPath + "/" + Application.productName)){
            hasSaveFile = true;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + Application.productName, FileMode.Open);
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();
            bestEasyRun = data.bestEasyRun;
            bestHardRun = data.bestHardRun;
            totalCoins = data.totalCoins;
            totalEasyDistance = data.totalEasyDistance;
            totalHardDistance = data.totalHardDistance;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
            buttonIconID = data.buttonIconID;
            quality = data.quality;
            useDayCycle = data.useDayCycle;
            qualityController.UpdateQuality(quality);
            Debug.Log("Save data loaded!");
        }
    }
    public void DeleteSave(){
        if(File.Exists(Application.persistentDataPath + "/" + Application.productName)){
            bestEasyRun = 0;
            bestHardRun = 0;
            totalCoins = 0;
            totalEasyDistance = 0;
            totalHardDistance = 0;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + Application.productName);
            SaveData data = new SaveData();
            data.bestEasyRun = bestEasyRun;
            data.bestHardRun = bestHardRun;
            data.totalCoins = totalCoins;
            data.totalEasyDistance = totalEasyDistance;
            data.totalHardDistance = totalHardDistance;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Data has been deleted.");
        }
    }
    public void SaveSettings(){
        if(useSaveFile){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + Application.productName);
            SaveData data = new SaveData();
            data.sfxVolume = sfxVolume;
            data.musicVolume = musicVolume;
            data.buttonIconID = buttonIconID;
            data.quality = quality;
            data.useDayCycle = useDayCycle;
            qualityController.UpdateQuality(quality);
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("The settings have been saved.");
        }
    }
    public void AcceptedSave(){
        SaveGame();
    }
}

[System.Serializable]
class SaveData {
    public int bestEasyRun = 0;
    public int bestHardRun = 0;
    public int totalCoins = 0;
    public float totalEasyDistance = 0.0f;
    public float totalHardDistance = 0.0f;
    public int sfxVolume = 50;
    public int musicVolume = 50;
    public int buttonIconID = 0;
    public int quality = 2;
    public bool useDayCycle = true;
}
