using UnityEngine.Rendering;
using UnityEngine;

public class QualityController : MonoBehaviour
{
    public RenderPipelineAsset lowQuality;
    public RenderPipelineAsset mediumQuality;
    public RenderPipelineAsset highQuality;
    public RenderPipelineAsset ultraQuality;
    [Space(10)]
    public int currentQuality;
    [Space(10)]
    public int maxFPS = 60;
    public bool lockFPS;

    void Awake(){
        DontDestroyOnLoad(this);
    }
    public void UpdateQuality(int quality){
        if(quality != currentQuality){
            currentQuality = quality;
            QualitySettings.SetQualityLevel(quality);
        }
        if(lockFPS) Application.targetFrameRate = maxFPS;
    }
}
