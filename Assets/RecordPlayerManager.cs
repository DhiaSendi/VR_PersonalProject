using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecordPlayerManager : XRSocketInteractor
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Disc"))
        {

            other.GetComponent<AudioSource>().Play();
        }
    }
}

