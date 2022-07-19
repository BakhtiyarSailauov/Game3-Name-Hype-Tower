using UnityEngine;

public class UnlockCubs : MonoBehaviour
{
    public Material blackMaterial;
    public int needPointforCubs;

    private void Start() 
    {
        if (PlayerPrefs.GetInt("score") < needPointforCubs)
            GetComponent<MeshRenderer>().material = blackMaterial;
    }
}
