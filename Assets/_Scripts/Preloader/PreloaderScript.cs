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

    private void log(string t)
    {
        LogText.text += t + "\n";
    }

    private void Start()
    {
        if (!GooglePlayDownloader.RunningOnAndroid())
        {
            AlarmText.text = "Use GooglePlayDownloader only on Android device!";
            return;
        }

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
                StartCoroutine(loadLevel());

            if (mainPath == null)
            {
                AlarmText.text = "The game needs to download game content. It's recommanded to use WIFI connection.";
                _requireDownload = true;
            }
        }
    }

    private void Update()
    {
        if (_requireDownload)
        {
            GooglePlayDownloader.FetchOBB();
            StartCoroutine(loadLevel());
            _requireDownload = false;
        }
    }

    protected IEnumerator loadLevel()
    {
        string mainPath;
        do
        {
            yield return new WaitForSeconds(0.5f);
            mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            log("waiting mainPath " + mainPath);
        }
        while (mainPath == null);

        if (downloadStarted == false)
        {
            downloadStarted = true;

            string uri = "file://" + mainPath;
            log("downloading " + uri);
            WWW www = WWW.LoadFromCacheOrDownload(uri, 0);

            // Wait for download to complete
            yield return www;

            if (www.error != null)
            {
                log("wwww error " + www.error);
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}