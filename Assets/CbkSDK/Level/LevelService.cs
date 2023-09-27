using System.Collections;
using System.Collections.Generic;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using CbkSDK.PlayerPrefs;
using CbkSDK.State;
using UnityEngine;

namespace CbkSDK.Level
{
    public interface ILevelService : IService
    {
        public bool IsLevelStart { get; }
        public bool IsLevelFail { get; }
        public bool IsLevelSuccess { get; }
        public int Level { get; set; }
        public float LevelMultiplier { get; }
        public void RestartLevel();
        public void Fail();
        public void Success();
    }

    [Service(false)]
    public class LevelService : BaseMonoService, ILevelService
    {
        public static ABType LEVEL_SET = ABType.A;
    
        [SerializeField] private LevelData _levelData;
        [SerializeField] private GameObject _levelInstance;
        [SerializeField] private int _level = 1;
        [SerializeField] private bool _isLevelStart = false;
        [SerializeField] private bool _isLevelSuccess = false;
        [SerializeField] private bool isLevelFail = false;
        private IPlayerPrefsService _playerPrefsService;
        private List<GameObject> LevelPrefabs => _levelData.levelSets[(int)LEVEL_SET].levels;
        public bool IsLevelStart => _isLevelStart;
        public bool IsLevelFail => isLevelFail;
        public bool IsLevelSuccess => _isLevelSuccess;

        public int Level
        {
            get => _level;
            set
            {
                var val = Mathf.Clamp(value, 1, int.MaxValue);
                _level = val;
                _playerPrefsService.Save("level", val);
            }
        }

        public float LevelMultiplier
        {
            get
            {
                var levelMultipliers = _levelData.levelSets[(int)LEVEL_SET].levelMultipliers;
                var index = Level - 1;
                index = Mathf.Clamp(index, 0, levelMultipliers.Count - 1);
                var levelMultiplier = levelMultipliers[index];
                return levelMultiplier;
            }
        }

        public void Initialize()
        {
            _levelData = Resources.LoadAll<LevelData>("")[0];
        }
    
        private void Awake()
        {
            Subscribe(GameStateEvents.ON_GAMEPLAY_ENTER, OnGamePlayEnter);
        }

        private void OnDestroy()
        {
            Unsubscribe(GameStateEvents.ON_GAMEPLAY_ENTER, OnGamePlayEnter);
        }
        private void Start()
        {
            _playerPrefsService = GetService<IPlayerPrefsService>();
            if (!_playerPrefsService.Load("level", out _level))
                _level = 1;
            var levelIndex = LevelIndex(_level);
            InitLevel(levelIndex);
        }

        private void OnGamePlayEnter(IEvent e)
        {
            if (_isLevelStart) return;
            _isLevelStart = true;
            StartLevel();
        }

        private void StartLevel()
        {
            Fire(LevelEvents.ON_LEVEL_START);
        }

        private int LevelIndex(int level)
        {
            if (level <= 0) level = 1;
            var index = level - 1;
            var levels = LevelPrefabs;
            if (index < levels.Count) return index;
            var levelCount = levels.Count;
            var newIndex = index - levelCount;
            var possibleLevelCount = _levelData.playFirstLevelAgain ? levelCount : levelCount - 1;
            var mod = newIndex % possibleLevelCount;
            var inc = _levelData.shuffle ? Mathf.PI * level : 0;
            var result = (mod + (int)inc) % possibleLevelCount;
            return _levelData.playFirstLevelAgain ? result : result + 1;
        }

        private void DestroyCurrentLevelObject()
        {
            if (_levelInstance)
            {
                Fire(LevelEvents.ON_BEFORE_LEVEL_DESTROY);
                Destroy(_levelInstance);
                Fire(LevelEvents.ON_LEVEL_DESTROY);
                _levelInstance.SetActive(false);
            }
        }

        private void InitLevel(int levelIndex)
        {
            StartCoroutine(InitLevelCoroutine(levelIndex));
        }

        private IEnumerator InitLevelCoroutine(int levelIndex)
        {
            yield return new WaitForEndOfFrame();
            var currentLevel = LevelPrefabs[levelIndex];
            _isLevelStart = false;
            _isLevelSuccess = false;
            isLevelFail = false;
            Fire(LevelEvents.ON_BEFORE_LEVEL_INIT);
            _levelInstance = Instantiate(currentLevel);
            Fire(LevelEvents.ON_LEVEL_INIT);
            if (!_levelInstance.activeSelf) _levelInstance.SetActive(true);
        }


        public void RestartLevel()
        {
            DestroyCurrentLevelObject();
            var levelId = Level;
            var levelIndex = LevelIndex(levelId);
            InitLevel(levelIndex);
        }

        public void Fail()
        {
            if (isLevelFail) return;
            isLevelFail = true;
            Fire(LevelEvents.ON_LEVEL_FAIL);
        }

        public void Success()
        {
            if (_isLevelSuccess) return;
            _isLevelSuccess = true;
            var level = Level;
            Fire(LevelEvents.ON_LEVEL_SUCCESS);
            Level = level + 1;
        }
    


    }
}