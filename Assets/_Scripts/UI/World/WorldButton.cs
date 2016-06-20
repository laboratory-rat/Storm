using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class WorldButton : MonoBehaviour
    {
        public Text TextComponent;
        public Sprite ClosedImage;
        public string World;
        public string NextWorld = "";
        public bool LastWorld = true;
        public Animator Anim;
        public string Trigger = "ToLevels";

        private Button _button;
        private LevelSpacer _ls;
        private World _world;
        private Sprite _baseImage;

        private void Start()
        {
            _button = GetComponent<Button>();
            _ls = FindObjectOfType<LevelSpacer>();
            _baseImage = GetComponent<Image>().sprite;

            UpdateWorlds();
            LevelController.Instance.OnLevelsChanged += UpdateWorlds;
        }

        public void UpdateWorlds()
        {
            if (_button)
            {
                if ((_world = LevelController.Instance.GetWorld(World)) == null)
                {
                    GetComponent<Image>().sprite = ClosedImage;
                    int i = LevelPackage.GetWorld(World).RequireFlash;
                    TextComponent.enabled = false;
                    TextComponent.text = "0 / " + i;
                    _button.enabled = false;
                }
                else
                {
                    _button.enabled = true;
                    TextComponent.enabled = true;
                    TextComponent.text = _world.Flash + " / " + _world.RequireFlash;
                    _button.onClick.AddListener(() => { Anim.SetTrigger(Trigger); _ls.PlaceLevels(World); });

                    GetComponent<Image>().sprite = _baseImage;

                    if (!string.IsNullOrEmpty(NextWorld) && !LastWorld)
                    {
                        if (_world.Flash >= _world.RequireFlash)
                        {
                            LevelController.Instance.OpenWorld(NextWorld);
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            LevelController.Instance.OnLevelsChanged -= UpdateWorlds;
        }
    }
}