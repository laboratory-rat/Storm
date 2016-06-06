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

        private void Start()
        {
            _button = GetComponent<Button>();
            _ls = FindObjectOfType<LevelSpacer>();

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
                    TextComponent.text = "0 / " + i;
                    _button.enabled = false;
                }
                else
                {
                    TextComponent.text = _world.Flash + " / " + _world.RequireFlash;
                    _button.onClick.AddListener(() => { Anim.SetTrigger(Trigger); _ls.PlaceLevels(World); });

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