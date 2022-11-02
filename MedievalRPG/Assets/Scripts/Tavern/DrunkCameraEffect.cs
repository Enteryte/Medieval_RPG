using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DrunkCameraEffect : MonoBehaviour
{
    public VolumeProfile volumeP;
    public MotionBlur mBlur;

    // Start is called before the first frame update
    void Start()
    {
        volumeP = Camera.main.GetComponent<Volume>().sharedProfile;

        if (!volumeP.TryGet<MotionBlur>(out var mB))
        {
            mB = volumeP.Add<MotionBlur>();

            mBlur = mB;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
