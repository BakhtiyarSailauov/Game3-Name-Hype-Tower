using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Ads : MonoBehaviour
{
    private Coroutine _ShowAds;
    private static int countLoses;
    private string gameId = "4118267", type = "video";
    private bool testMode = true;

    public void Start() 
    {
        Advertisement.Initialize(gameId, testMode);

        StartCoroutine(ShowAds());
    }

    IEnumerator ShowAds() 
    {
        while (true)
        {
            if (Advertisement.IsReady(type))
            {
                Debug.Log("Ready");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
