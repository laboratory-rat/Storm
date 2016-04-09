using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        public Text Title;

        public void LevelOnClick(string level)
        {
            GetComponent<Button>().onClick.AddListener(() => SceneController.Instance.ChangeScene(level, UnityEngine.SceneManagement.LoadSceneMode.Single));
        }
    }
}