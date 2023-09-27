using System.Collections.Generic;
using UnityEngine;

namespace CbkSDK.Level
{
    public enum ABType
    {
        A,
        B
    }

    [System.Serializable]
    public class LevelSet
    {
        public List<GameObject> levels = new List<GameObject>();
        [Range(0f, 100f)]
        public List<float> levelMultipliers = new List<float>() { 1f };
    }
    
    
    
    [CreateAssetMenu(fileName = "LevelData", menuName = "_GAME/LevelData")]
    public class LevelData:ScriptableObject
    {
        public bool playFirstLevelAgain = true;
        public bool shuffle = true;
        public List<LevelSet> levelSets = new List<LevelSet>();
    }
}