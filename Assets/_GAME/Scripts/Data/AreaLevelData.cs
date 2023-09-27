using System.Collections.Generic;
using _GAME.Scripts.Services;
using UnityEngine;

namespace _GAME.Scripts.Data
{
    [System.Serializable]
    public class AreaInfo
    {
        public int index = 0;
        public List<ColorType> crowds = new List<ColorType>();
    
    }

    [CreateAssetMenu(fileName = "AreaLevelData", menuName = "_GAME/AreaLevelData")]
    public class AreaLevelData : ScriptableObject
    {
        public int level;
        public List<AreaInfo> areas = new List<AreaInfo>();
    }
}