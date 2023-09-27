using System.Collections.Generic;
using UnityEngine;

namespace _GAME.Scripts.Util
{
    public class MultipleLineRenderer : MonoBehaviour
    {
        [SerializeField] private int count = 1;
        [SerializeField] private List<LineRenderer> _lines = new List<LineRenderer>();
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private List<LineRenderer> _passives = new List<LineRenderer>();
        [SerializeField] private bool removeLast = true;

        public int Count
        {
            get => count;
            set
            {
                count = value;
                UpdateLineCount();
            }
        }
    
        private void UpdateLineCount()
        {
            if (count < 0) count = 0;
            while (_lines.Count < count)
            {
                CreateLine();
            }

            while (_lines.Count > count)
            {
                RemoveLine();
            }
        }
    
        private void CreateLine()
        {
            LineRenderer line;
            if (_passives.Count>0)
            {
                var index = _passives.Count - 1;
                line = _passives[index];
                _passives.RemoveAt(index);
                line.gameObject.SetActive(true);
            }
            else
                line = Instantiate(_linePrefab, transform).GetComponent<LineRenderer>();
            _lines.Add(line);
        }

        private void RemoveLine()
        {
            var linesCount = _lines.Count;
            var index = removeLast?linesCount - 1:0;
            var line = _lines[index];
            _lines.RemoveAt(index);
            line.gameObject.SetActive(false);
            _passives.Add(line);
        }
        public void DrawLine(params Vector3[] positions)
        {
            Count++;
            var last = Last();
            last.positionCount = positions.Length;
            last.SetPositions(positions);
        }



        public LineRenderer Get(int index)
        {
            return _lines[index];
        }

        public LineRenderer Last() => _lines[_lines.Count - 1];
    }
}
