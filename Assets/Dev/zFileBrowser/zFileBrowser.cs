using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class zFileBrowser : zNodeController
{
    public StringEvent fileOpenEvent;
    public StringEvent fileSaveEvent;

    public Text pathdisplay;
    public List<string> extensionsToAccept;
    string currentPath;
    [Header("other templates")]
    public GameObject dirButtonTemplate;
    Transform dirbuttonTarget;
    public GameObject favButtonTemplate;
    Transform favButtonTarget;
    public string startFolder;
    protected virtual void openFile(string s)
    { //  s = s.Replace('\\', Path.DirectorySeparatorChar);
      //  s = s.Replace('/', Path.DirectorySeparatorChar);

        if (fileOpenEvent != null)
        {
            fileOpenEvent.Invoke(s);
        }
        else Debug.Log("no opening even " + s);
        scanDir(currentPath);
    }
    const int MAX_FAVOURITES = 16;
    public FileNode createNode(FileNode parentitem, string templateName, string path)
    {
        FileNode thisItem = (FileNode)AddNode(path, templateName);

        return thisItem;
    }
    public void scanRoot()

    {
        Clear();
        for (int i = 0; i < 16; i++)
        {
            char driveLetter = (char)(('C') + i);
            string path = driveLetter + ":" + Path.DirectorySeparatorChar;
            bool driveExists = System.IO.Directory.Exists(path);
            if (driveExists)
            {
                FileNode thisNode = createNode(null, "Volume", path);
                thisNode.onClickAction.AddListener(() => scanDir(path));
            }
        }
        if (pathdisplay != null)
            pathdisplay.text = "ROOT";
        GetDirButtons(currentPath);
    }
    public void rescancurrent()
    {
        scanDir(currentPath);
    }

    public void scanDir(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            // Debug.Log("empty path"); 
            return;
        }
        if (s.Equals("Drives"))
        {
            scanRoot();
            return;
        }
        //   Debug.Log("scning " + s + " is rooted =" + Path.IsPathRooted(s));

        if (s.Equals(".."))
        {
            s = Path.GetDirectoryName(currentPath);
            //  Debug.Log("root path " + s);
        }
        if (!System.IO.Directory.Exists(s))
        {
            scanRoot();
            return;
        }

        Clear();
        string[] dirs = System.IO.Directory.GetDirectories(s, "*.*", System.IO.SearchOption.TopDirectoryOnly);
        string[] files = System.IO.Directory.GetFiles(s, "*.*", System.IO.SearchOption.TopDirectoryOnly); //System.IO.SearchOption.AllDirectories)
                                                                                                          //  FileNode n = 
        FileNode parent = createNode(null, "Dir", "..");
        parent.onClickAction.AddListener(() => scanDir(".."));

        for (int i = 0; i < dirs.Length; i++)
        {
            string thisDirName = dirs[i];
            FileNode thisDirNode = createNode(null, "Dir", thisDirName);
            thisDirNode.onClickAction.AddListener(() => scanDir(thisDirName));
            thisDirNode.fileName = thisDirName;

        }

        List<string> sortfiles = new List<string>();
        for (int i = 0; i < files.Length; i++)
            sortfiles.Add(files[i]);
        sortfiles.Sort();
        for (int i = 0; i < sortfiles.Count; i++)
        {
            string thisFileName = Path.GetFileName(sortfiles[i]);
            string thisFilePath = sortfiles[i];
            if (isSupportedExtensoin(sortfiles[i]))
            {
                FileNode thisNode = createNode(null, "File", thisFileName);
                nodes.Add(thisNode);
                thisNode.onClickAction.AddListener(() => openFile(thisFilePath));
                thisNode.fileName = thisFileName;
            }
            else
            {
                FileNode thisNode = createNode(null, "FileInactive", thisFileName);
                thisNode.fileName = thisFileName;
            }
        }
        currentPath = s;
        GetDirButtons(currentPath);
        if (pathdisplay != null)
            pathdisplay.text = s;
    }

    public void saveClicked(string saveFilename)
    {
        if (Path.GetExtension(saveFilename).Equals(""))saveFilename+=".xml";
              
        string fullPath = Path.Combine(currentPath, saveFilename);
        Debug.Log("save filename " + saveFilename + " fullpath " + fullPath);
        Clear();
        scanDir(currentPath);
        fileSaveEvent.Invoke(fullPath);

    }

    void GetDirButtons(string path)
    {
        if (dirButtonTemplate == null) return;
        if (dirbuttonTarget == null)
        {
            dirbuttonTarget = dirButtonTemplate.transform.parent;
            dirButtonTemplate.SetActive(false);
        }
        dirbuttonTarget.removeChildren(0);

        while (System.IO.Directory.Exists(path))
        {

            GameObject newButton = Instantiate(dirButtonTemplate, dirbuttonTarget);
            newButton.SetActive(true);
            Button b = newButton.GetComponent<Button>();
            string p = path;
            b.onClick.AddListener(() => scanDir(p));
            Text t = newButton.GetComponentInChildren<Text>();
            t.text = path;
            newButton.transform.SetSiblingIndex(1);
            path = Path.GetDirectoryName(path);

        }


    }

    protected override void OnValidate()
    {
        base.OnValidate();
        for (int i = 0; i < extensionsToAccept.Count; i++)
        {

            string thisExt = extensionsToAccept[i].ToLower();
            if (thisExt != null)
                if (thisExt[0] != '.') thisExt = "." + thisExt;
            extensionsToAccept[i] = thisExt;
        }
    }
    bool isSupportedExtensoin(string s)
    {
        if (extensionsToAccept.Count == 0) return true;
        string extension = Path.GetExtension(s);
        if (extensionsToAccept.Contains(extension)) return true;
        return false;
    }

    string getFavId(int id)
    {
        return "FileFavourite_" + id;
    }
    public void addFavourite()
    {
        for (int i = 0; i < MAX_FAVOURITES; i++)
        {
            string thisFavouriteName = getFavId(i);
            if (!PlayerPrefs.HasKey(thisFavouriteName))
            {
                Debug.Log("saved " + currentPath + " as " + getFavId(i));
                PlayerPrefs.SetString(getFavId(i), currentPath);
                getFavourites();
                return;
            }
        }
        Debug.Log("NO FREE SAVE SLOTS");
    }
    void removeFavouriste(int id)
    {
        Debug.Log(" has " + id + " " + PlayerPrefs.HasKey(getFavId(id)));
        PlayerPrefs.DeleteKey(getFavId(id));
        Debug.Log("now has " + id + " " + PlayerPrefs.HasKey(getFavId(id)));

        getFavourites();
    }
    void getFavourites()
    {
        if (favButtonTarget == null)
        {
            favButtonTarget = favButtonTemplate.transform.parent;
            favButtonTemplate.SetActive(false);
        }
        favButtonTarget.removeChildren(1);

        for (int i = 0; i < 16; i++)
        {
            string thisFavouriteName = getFavId(i);
            if (PlayerPrefs.HasKey(thisFavouriteName))
            {
                GameObject thisFav = Instantiate(favButtonTemplate, favButtonTarget);
                thisFav.SetActive(true);
                Text t = thisFav.GetComponentInChildren<Text>();
                string thisPath = PlayerPrefs.GetString(thisFavouriteName);
                t.text = thisPath;
                Button[] buttons = thisFav.GetComponentsInChildren<Button>();
                buttons[0].onClick.AddListener(() => scanDir(thisPath));
                int k = i;
                buttons[1].onClick.AddListener(() => removeFavouriste(k));
                thisFav.name = thisFavouriteName;
            }
        }

    }
    protected override void Start()
    {
        base.Start();

        getFavourites();

        if (!string.IsNullOrEmpty(startFolder))
            scanDir(startFolder);
        else
            scanDir(Path.GetDirectoryName(Path.GetDirectoryName(Application.streamingAssetsPath))); //Path.GetDirectoryName*/
        /*  if (useKeyboardInput)
          {
              zKeyMap.map(this, OnUp, KeyCode.UpArrow);
              zKeyMap.map(this, OnDown, KeyCode.DownArrow);
              zKeyMap.map(this, OnEnter, KeyCode.Return);
              zKeyMap.map(this, OnEscape, KeyCode.Escape);
          }*/

    }


}
