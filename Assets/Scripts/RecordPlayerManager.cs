using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR.Interaction.Toolkit;

public class RecordPlayerManager : XRSocketInteractor
{
    [SerializeField] bool isSelected, can;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    IXRSelectInteractable selectInteractable;

    Coroutine co;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    protected new void OnTriggerEnter(Collider other)
     {
        if (other.CompareTag("PredefinedDisc"))
        {
            audioClip = other.GetComponent<AudioSource>().clip;

            if (can)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                print("Played once");
            }
        }
        else if (other.CompareTag("Disc"))
        {
            SetAudioSource(other.GetComponent<AlbumManager>().paths[0]);
            
        }
    }

    public void SetAudioSource(string _songPath)
    {
        co = StartCoroutine(GetAudioClip(_songPath));
    }

    IEnumerator GetAudioClip(string fileName)
    {
        string fullPath = "file://" + FilesManager.FilePath + "/" + fileName;
        //fullPath = "file:///" + fullPath;

        Debug.Log(fullPath);

        UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(
            fullPath, AudioType.MPEG);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error 1 " + webRequest.error);
        }
        else
        {

            AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);
            clip.name = fileName;
            if (can)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
                print("Played once");
            }
        }
        StopCoroutine(co);
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

