using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shaders.WobblyGrid
{
    public class WobblyGrid : MonoBehaviour
    {
        [Header("Grid Properties")]
        [SerializeField] private int _height;
        [SerializeField] private int _width;
        [SerializeField] private float _gridElevation;
        [SerializeField] private float _blockWidth;
        [SerializeField] private float _blockHeight;
        [SerializeField] private float _spaceInBetween;
        [SerializeField] private float _gridDivision;
        
        [Header("Animation")]
        [SerializeField] private bool _wobble;
        [SerializeField] private float _scale;
        [SerializeField] private float _amplitude;
        [SerializeField] private float _frequency;
        
        [SerializeField] private GameObject _cell;


        private GameObject[] _grid;
        private int _cellIndex;
        
        public int GridSize => _height * _width;

        private void Awake()
        {
            _grid = new GameObject[GridSize];
        }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    float xTemp;
                    float zTemp;
                    if (i > _width / _gridDivision)
                    {
                        xTemp = _width / _gridDivision - (i - (_width / _gridDivision));
                    }
                    else
                    {
                        xTemp = i;
                    }
                    if (j > _height / _gridDivision)
                    {
                        zTemp = _height / _gridDivision - (j - (_height / _gridDivision));
                    }
                    else
                    {
                        zTemp = j;
                    }
                    
                    var position = new Vector3(i * _spaceInBetween, (xTemp * zTemp) * _gridElevation, j * _spaceInBetween);
                    var cell = Instantiate(_cell, position, Quaternion.identity, transform);
                    cell.transform.localScale = new Vector3(_blockWidth, _blockHeight, _blockWidth);
                    cell.name = $"Cell{_cellIndex}";
                    _grid[_cellIndex] = cell;
                    _cellIndex++;
                }
            }
        }


        private void Update()
        {
            if(!_wobble) return;

            for (int i = 0; i < GridSize; i++)
            {
                var wave = _scale * Mathf.Sin(Time.fixedTime * _amplitude + (i * _frequency));
                var wave2 = _scale * Mathf.PerlinNoise(Time.fixedTime * _amplitude + (i * _frequency), 
                    Time.fixedTime * _amplitude + (i * _frequency));
                
                _grid[i].transform.position = new Vector3(_grid[i].transform.position.x,
                    _grid[i].transform.position.y + (wave * wave2) * Random.Range(1,3),
                    _grid[i].transform.position.z);
            }
        }
    }
}
