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

            OnEnable();
        }

        private void OnClick()
        {
            if (MarketController.Instance.PMone.Energy > 0)
            {
                LevelController.Instance.CurrentLevel = _level;
                LevelController.Instance.CurrentWorld = _world;

                SceneController.Instance.ChangeScene(_level.LevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                AndroidNativeFunctions.ShowToast(LocalController.Instance.L("market_level", "no_b"));
            }
        }

        private Sprite _default;
        private Sprite _default_active;

        private void OnEnable()
        {
            if (_world != "")
            {
                Level l;

                if ((l = LevelController.Instance.GetLevel(_world, _level.Name)) == null)
                {
                    _default = _image.sprite;
                    _default_active = _button.spriteState.pressedSprite;

                    _image.sprite = Block;

                    SpriteState ss = new SpriteState();
                    ss.disabledSprite = Block;
                    ss.pressedSprite = BlockOn;

                    IndexText.enabled = false;

                    _button.spriteState = ss;

                    int cost = LevelPackage.GetWorld(_world).Cost;

                    _button.onClick.AddListener(() =>
                    {
                        if (Application.isMobilePlatform)
                        {
                            AndroidNativeFunctions.ShowAlert(LocalController.Instance.L("market_level", "byu") + " " + cost + " " + LocalController.Instance.L("market_level", "b"), LocalController.Instance.L("market_level", "title"), LocalController.Instance.L("market_level", "yes"), LocalController.Instance.L("market_level", "no"), "", (DialogInterface d) =>
                            {
                                if (d == DialogInterface.Positive)
                                {
                                    if (MarketController.Instance.PMone.Money >= cost)
                                    {
                                        LevelController.Instance.OpenNew(_world, _level.LevelName);
                                        MarketController.Instance.MinusMoney(cost);

                                        _image.sprite = _default;

                                        SpriteState sps = new SpriteState();
                                        sps.pressedSprite = _default_active;
                                        _button.spriteState = sps;

                                        IndexText.enabled = true;

                                        AndroidNativeFunctions.ShowToast(LocalController.Instance.L("market_level", "success"));

                                        OnEnable();
                                    }
                                    else
                                    {
                                        AndroidNativeFunctions.ShowToast(LocalController.Instance.L("market_level", "no_b"));
                                    }
                                }
                            });
                        }
                        else
                        {
                            if (MarketController.Instance.PMone.Money >= cost)
                            {
                                LevelController.Instance.OpenNew(_world, _level.LevelName);
                                MarketController.Instance.MinusMoney(cost);

                                _image.sprite = _default;

                                SpriteState sps = new SpriteState();
                                sps.pressedSprite = _default_active;
                                _button.spriteState = sps;

                                IndexText.enabled = true;

                                OnEnable();
                            }
                            else
                            {
                                Debug.Log("No batteries!");
                            }
                        }
                    });
                }
                else
                {
                    _button.onClick.AddListener(() => OnClick());

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