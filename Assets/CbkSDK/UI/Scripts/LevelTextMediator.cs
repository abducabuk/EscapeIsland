using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using CbkSDK.Level;
using TMPro;
using UnityEngine;

namespace CbkSDK.UI.Scripts
{
    public class LevelTextMediator : BaseMediator
    {
        [SerializeField] private TextMeshProUGUI _text;

        private ILevelService _levelService;
        private void OnEnable()
        {
            _levelService = GetService<ILevelService>();
            Subscribe(LevelEvents.ON_LEVEL_INIT,OnLevelInit);
        }

        private void OnDisable()
        {
            Subscribe(LevelEvents.ON_LEVEL_INIT,OnLevelInit);
        }

        private void OnLevelInit(IEvent e)
        {
            _text.text = "Level " + _levelService.Level;
        }
    }
}
