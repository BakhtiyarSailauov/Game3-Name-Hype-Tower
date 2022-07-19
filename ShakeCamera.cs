using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Transform camTransform;
    private float shakeCamDur = 1f, shakeCamStrong = 0.04f, stopShakeCam = 1.5f;
    private Vector3 originalPos;

    private void Start()
    {
        camTransform = GetComponent<Transform>();
        originalPos = camTransform.localPosition;
    }
    private void Update()
    {
        if (shakeCamDur > 0) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * 
                                                                  shakeCamStrong;
            shakeCamDur -= Time.deltaTime * stopShakeCam;
            
        }
        else{
            shakeCamDur = 0;
            camTransform.localPosition = originalPos;
        }
            
    }
}
