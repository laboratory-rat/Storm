using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PathTester : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        string s = string.Format("dataPath: {0}\n persistentDataPath: {1}\n is movbile platform: {2}", Application.dataPath, Application.persistentDataPath, Application.isMobilePlatform);
        text.text = s;
    }
}