using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModel", menuName = "ScriptableObjects/GameModel")]
public class GameModel : ScriptableObject
{
    public struct GridData
    {
        public readonly float SCREEN_WIDTH;
        public readonly float SCREEN_HEIGHT;
        public readonly float SPRITE_DIMENSION;
        public readonly GameObject cell;

        public GridData(float screenWidth, float screenHeight, float spriteDimension, GameObject cell)
        {
            SCREEN_WIDTH = screenWidth;
            SCREEN_HEIGHT = screenHeight;
            SPRITE_DIMENSION = spriteDimension;
            this.cell = cell;
        }
    }

    [SerializeField] 
    private CellView _cell;

    public int inputWidth = 3;
    public int inputHeight = 3;
    
    [Range(0.1f, 2f)]
    public float speed = 0.1f;

    [Tooltip("Turning this on will based the grid dimension on screen resolution. Ignoring the input width and height")]
    public bool fillScreenCell;
    public bool spawnRandomAtStart;
    
    [Header("Game Stat Information")]
    [Space(50)]
    #if UNITY_EDITOR
    [ReadOnly]
    #endif
    public int CellCount, CellSpawned, CellDied, GridWidth, GridHeight, ScreenWidth, ScreenHeight, Steps;
    
    public Cell[,] _cellsArray;
    
    private GridData _gridData;
    public GridData GridModelData
    {
        get { return _gridData; }
    }
    
    public void Initialize()
    {
        _gridData = new GridData(
            screenWidth: Screen.width,
            screenHeight: Screen.height,
            spriteDimension: 16,
            cell: _cell.gameObject
        );

        speed = 0.1f;
    }
    
    
}
