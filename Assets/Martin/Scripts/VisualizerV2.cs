using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerV2 : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    void Start()
    {
        
    }

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, (AudioDataV2._freqBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
    }
}
