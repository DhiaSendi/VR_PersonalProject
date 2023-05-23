using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RecordPlayerManager : XRSocketInteractor
{
    [SerializeField] bool isSelected, can;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    IXRSelectInteractable selectInteractable;
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    protected new void OnTriggerEnter(Collider other)
     {
        if (other.CompareTag("Disc"))
        {
            audioClip = other.GetComponent<AudioSource>().clip;

            if (can)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                print("Played once");
            }
        }
     }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        isSelected = interactable.isSelected;
        can = base.CanSelect(interactable);
        selectInteractable = interactable;
        return base.CanSelect(interactable);
    }
    private void Update()
    {
       // print("Is " + isSelected);
       // print("Can " + can);
       //print(selectInteractable);
       //print(selectInteractable.isSelected);
       //print(selectInteractable.transform.gameObject.name);

    }
}

