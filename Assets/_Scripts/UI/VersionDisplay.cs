using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().text = "Version: " + Application.version;
    }
}