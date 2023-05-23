using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TagLib;
using SimpleFileBrowser;
using UnityEngine.UI;
public class FilesManager : MonoBehaviour
{

    public static FilesManager Instance;
    public static string FilePath;
    public List<Transform> columns = new List<Transform>();

    [SerializeField] GameObject albumPf;
    [SerializeField] List<AlbumManager> albums = new List<AlbumManager>();
    [SerializeField] int fileCount = 0;
    [SerializeField] int columnsSize;
    DirectoryInfo dir;
    FileInfo[] info;

    Dictionary<string, List<string>> albumnDict = new Dictionary<string, List<string>>();


    /*
    1. Read all files (audio) in filepath folder (COMPLETE)
    2. Create Dictionary <albumnName, <list>> for each album (COMPLETE)
    3. Count albumns and only display covers per albumn (COMPLETE)

    4. Assign images to cover 
    - If filename has space the (image) metadata cannot be read
    - Need to write read file function to account for spacing in filename (COMPLETE WORKS WITHOUT SPACES)
    
    ****
    5. When select albulm, load songlist on front panel (audio metadata)
        - When  albumn name is selected, list song names from selected albumn (stored in dictionary - albumnDict)
        - When song is selected, load MP3 files based on name, add to AudioSource, set clip from loaded AudioSource to AudioSource in manager
        - Switch state to 3.
    6. When select song, transition into visualizer
    7. Fix player gravity
    ****
    *
    *. Load song data and place into audiosource.

    */


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        OpenDialog();
        //FilePath = "E:/songs3";
        //dir = new DirectoryInfo(FilePath);
        //info = dir.GetFiles("*.*");
        //CountFilesInDir();

    }
    void OpenDialog()
    {
        //FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        //FileBrowser.SetDefaultFilter(".jpg");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        // FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        StartCoroutine(ShowLoadDialogCoroutine());

    }

    public void CountFilesInDir()
    {
        int i = 0;
        int index = 0;
        //Fetch all the file
        foreach (FileInfo f in info)
        {
            fileCount++;
            //Create a metadata for each song
            TagLib.File _file = TagLib.File.Create(@FilePath + "/" + f.Name);
            if (albumnDict.ContainsKey(_file.Tag.Album))
            {
                List<string> _list = albumnDict[_file.Tag.Album];
                _list.Add(f.Name);
                albumnDict[_file.Tag.Album] = _list;
                // print(_i+"  " + f.Name);

            }
            else
            {
                List<string> _list = new List<string>();
                _list.Add(f.Name);
                albumnDict.Add(_file.Tag.Album, _list);

                var _byte = (byte[])_file.Tag.Pictures[0].Data.Data;
                Texture2D material = new Texture2D(2, 2);

                material.LoadImage(_byte);

                GameObject album = Instantiate(albumPf);
                album.transform.position = columns[index].position;
                album.GetComponentInChildren<Renderer>().material.mainTexture = material;
                album.GetComponentInChildren<Renderer>().material.mainTextureScale = new Vector2(2,2);


                AlbumManager albumManager = album.GetComponent<AlbumManager>();
                albumManager.albumName = _file.Tag.Album;

                //albumManager.albumNameTxt.SetText(_file.Tag.Album);
                albumManager.cover = material;

                albums.Add(albumManager);


                index++;
                //image.texture = material;
            }




        }
        Debug.Log(albumnDict.Count);
        //LoadCovers();
        foreach (KeyValuePair<string, List<string>> item in albumnDict)
        {
            // print("Key " + item.Key + " Value nbr " + item.Value.Count);
            // x += item.Value.Count;
            foreach (AlbumManager album in albums)
            {
                if (album.albumName == item.Key)
                {
                    foreach (var song in item.Value)
                    {
                        album.paths.Add(song);
                    }

                }

            }
        }

    }


    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                Debug.Log("1 " + FileBrowser.Result[i]);
                FilePath = FileBrowser.Result[i];

            }

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            // byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            //Debug.Log("2 "+bytes.ToString());
            //FilePath = bytes.ToString();


            //
            // // Or, copy the first file to persistentDataPath
            //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            // print(destinationPath);
            // FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
            //FilePath = destinationPath;
            dir = new DirectoryInfo(FilePath);
            info = dir.GetFiles("*.*");
            CountFilesInDir();
        }
    }

}
