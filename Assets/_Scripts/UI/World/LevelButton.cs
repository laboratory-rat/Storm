using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class LevelButton : MonoBehaviour
    {
        public Text IndexText;
        //public Text RateText;
        //public Text CostText;

        public Sprite Block;
        public Sprite BlockOn;

        public Image FFlash;
        public Image SFlash;
        public Image TFlash;
        //public Image BackFlash;

        private Button _button;
        private string _world = "";
        private Level _level;
        private Image _image;

        private void Start()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        public void Init(string world, Level level, string index)
        {
            if (!_button)
                Start();

            FFlash.enabled = false;
            SFlash.enabled = false;
            TFlash.enabled = false;

            _world = world;

            var l = LevelController.Instance.GetLevel(world, level.Name);

            _level = l == null ? level : l;

            IndexText.text = index;
            //CostText.text = "Cost: " + _level.Cost;

            OnEnable();
        }

        private void OnClick()
        {
            if (MarketController.Instance.MinusEnergy(_level.Cost))
            {
                LevelController.Instance.CurrentLevel = _level;
                LevelController.Instance.CurrentWorld = _world;

                SceneController.Instance.ChangeScene(_level.LevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                AndroidNativeFunctions.ShowToast("Мало энергии");
            }
        }

        private void OnEnable()
        {
            if (_world != "")
            {
                Level l;

                if ((l = LevelController.Instance.GetLevel(_world, _level.Name)) == null)
                {
                    _image.sprite = Block;

                    SpriteState ss = new SpriteState();
                    ss.disabledSprite = Block;
                    ss.pressedSprite = BlockOn;
                    //BackFlash.enabled = false;

                    _button.spriteState = ss;
                }
                else
                {
                    _button.onClick.AddListener(() => OnClick());
                    //BackFlash.enabled = true;

                    switch (_level.Flash)
                    {
                        case FlashRate.One:
                            FFlash.enabled = true;
                            break;

                        case FlashRate.Two:
                            FFlash.enabled = true;
                            SFlash.enabled = true;
                            break;

                        case FlashRate.Three:
                            FFlash.enabled = true;
                            SFlash.enabled = true;
                            TFlash.enabled = true;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}