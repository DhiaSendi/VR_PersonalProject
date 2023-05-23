using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlbumManager : MonoBehaviour
{
    public GameObject songPf;
    public string albumName;
    public TextMeshProUGUI albumNameTxt;
    public int songsNbr;
   // public Texture2D coverTexture;
    public Texture2D cover;
    public List<string> paths = new List<string>();
    public List<string> names = new List<string>();

    Transform holderTransform;

    public void OnClickAlbum()
    {
        print("clicked");
        int index = 1;
        //holderTransform = MainManager.Instance.songsHolder; 
        foreach (string song in paths)
        {
            GameObject songGO = Instantiate(songPf, holderTransform);
           // SongsManager songsManager = songGO.GetComponent<SongsManager>();

            songGO.GetComponentInChildren<Renderer>().material.mainTexture = cover;
           // songsManager.songPath = song;
           // songsManager.songName.SetText(song);
           // songsManager.songIndex.SetText(index.ToString());

            index++;
        }
       // MainManager.Instance.musicHolder.gameObject.SetActive(true);
        //MainManager.Instance.coversHolder.gameObject.SetActive(false);
        //Junior
    }
}
