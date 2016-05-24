using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreloaderScript : MonoBehaviour
{
    private string expPath;
    private string logtxt;
    private bool alreadyLogged = false;
    private bool downloadStarted;

    public Text LogText;
    public Text AlarmText;

    private bool _requireDownload = false;
    private bool _active = false;

    private void log(string t)
    {
        return;
        //LogText.text += t + "\n";
    }

    private void Start()
    {
        //StartCoroutine(W());
        LoadGame();
    }

    private IEnumerator W()
    {
        log("Splash");
        yield return new WaitForSeconds(3f);
        log("End splash");
        _active = true;
        LoadGame();
    }

    private void LoadGame()
    {
        if (!GooglePlayDownloader.RunningOnAndroid())
        {
            AlarmText.text = "Use GooglePlayDownloader only on Android device!";
            return;
        }

        log("Loading started");

        expPath = GooglePlayDownloader.GetExpansionFilePath();
        if (expPath == null)
        {
            AlarmText.text = "External storage is not available!";
        }
        else
        {
            string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);

            log("expPath = " + expPath);
            log("Main = " + mainPath);
            log("Main = " + mainPath.Substring(expPath.Length));

            if (mainPath != null)
            {
                log("Main path != null");
                StartCoroutine(loadLevel());
            }
            else
            {
                AlarmText.text = "The game needs to download game content. It's recommanded to use WIFI connection.";
                _requireDownload = true;
            }
        }
    }

    private void Update()
    {
        if (!_active)
            return;

        if (_requireDownload)
        {
            GooglePlayDownloader.FetchOBB();
            StartCoroutine(loadLevel());
            _requireDownload = false;
        }
    }

    protected IEnumerator loadLevel()
    {
        log("loadLevel function");

        string mainPath;
        do
        {
            log("-- first do-while");
            yield return new WaitForSeconds(0.5f);
            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            log("waiting mainPath " + mainPath);
        }
        while (mainPath == null);

        log("end do-whle");

        if (downloadStarted == false)
        {
            log("downloadStarted == false");
            downloadStarted = true;

            string uri = "file://" + mainPath;
            log("downloading " + uri);
            WWW www = WWW.LoadFromCacheOrDownload(uri, 0);

            // Wait for download to complete

            log("downloading......");
            yield return www;
            log("downloaded");

            if (www.error != null)
            {
                log("wwww error " + www.error);
            }
            else
            {
                log("Success");
                SceneManager.LoadScene("Menu");
            }
        }
    }
}