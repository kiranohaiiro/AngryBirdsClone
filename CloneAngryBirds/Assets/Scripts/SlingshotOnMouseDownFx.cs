using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotOnMouseDownFx : MonoBehaviour
{
    public AudioSource slingshotFx;
    public AudioClip onMouseDownFx;
    public AudioClip onMouseUpFx;

    public void MouseDownFx()
    {
        slingshotFx.PlayOneShot(onMouseDownFx);
    }

    public void MouseUpFx()
    {
        slingshotFx.PlayOneShot(onMouseUpFx);
    }


}
