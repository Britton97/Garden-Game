using System;
using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using Sirenix.OdinInspector;
//using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap_Manager : MonoBehaviour
{
    public static TileMap_Manager Instance { get; private set; }

    public GameAction buildNavMeshAction;
    [SerializeField] private Vector2 tileMapCenter;
    [SerializeField] private Vector2Int tileMapSize;
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Tilemap waterTileMap;
    //[SerializeField] private Tile_SO testTile;
    [SerializeField] private List<Tile_SO> tileList;
    [SerializeField][ReadOnly] int tileIndex = 0;
    [SerializeField] private DataInt_SO money;
    [SerializeField] private GameAction moneySpentAction;
    [SerializeField] private GameAction swingAction;

    [SerializeReference] public Dictionary<String, int> tileMapDict = new Dictionary<String, int>();

    //Tile Selector Variables
    [SerializeField] Transform tileSelector;
    [SerializeField] SpriteRenderer currentTileSpriteRenderer;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Color validColor;
    [SerializeField] Color invalidColor;
    private SpriteRenderer tileSelectorSpriteRenderer;
    public NavMeshSurface navMeshSurface;
    public TextMesh tileAmountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        tileMapCenter = groundTileMap.transform.position;
        tileSelectorSpriteRenderer = tileSelector.GetComponent<SpriteRenderer>();
        currentTileSpriteRenderer.sprite = tileList[tileIndex].tileSprite;
        CountTiles();
    }

    void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = groundTileMap.WorldToCell(mousePosition);
        Vector3 cellCenterPosition = groundTileMap.GetCellCenterWorld(cellPosition);

        MoveTileSelector(cellCenterPosition);
        UpdateTileSelectorDetails(cellPosition);
    }

    public void RebuildNavmesh()
    {
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }

    private void UpdateTileSelectorDetails(Vector3Int cellPosition)
    {
        TileBase tileAtPosition = groundTileMap.GetTile(cellPosition);


        if (tileAtPosition == null || tileAtPosition.name != tileList[tileIndex].tileName)
        { // No tile at position or tile is different make green
            tileSelectorSpriteRenderer.color = validColor;
            tileAmountText.text = $"${tileList[tileIndex].cost.ToString()}";
        }
        else if (tileAtPosition.name == tileList[tileIndex].tileName)
        { // Tile is the same make red
            tileSelectorSpriteRenderer.color = invalidColor;
            tileAmountText.text = "";
        }
    }

    private void MoveTileSelector(Vector3 position)
    {
        tileSelector.position = Vector3.MoveTowards(tileSelector.position, position, moveSpeed * Time.deltaTime);
    }

    #region Parent Selector Functions
    public void ParentSelector()
    {
        tileSelector.transform.parent = transform;
    }

    public void UnparentSelector()
    {
        tileSelector.transform.parent = null;
    }
    #endregion

    public void CountTiles()
    {
        int totalTiles = 0;
        int filledTiles = 0;
        tileMapDict.Clear();

        for (int x = -tileMapSize.x; x < tileMapSize.x; x++)
        {
            for (int y = -tileMapSize.y; y < tileMapSize.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 worldPosition = groundTileMap.GetCellCenterWorld(tilePosition);
                totalTiles++;
                if (groundTileMap.HasTile(tilePosition))
                {
                    filledTiles++;
                    if (tileMapDict.ContainsKey(groundTileMap.GetTile(tilePosition).name))
                    {
                        tileMapDict[groundTileMap.GetTile(tilePosition).name]++;
                    }
                    else
                    {
                        tileMapDict.Add(groundTileMap.GetTile(tilePosition).name, 1);
                    }
                    //debug the tile position
                    //Debug.Log($"Tile Position: {tilePosition}");
                }
            }
        }
    }

    public void PlaceTile()
    {
        // Check if the player has enough money
        if (money.data < tileList[tileIndex].cost) return;

        // Get the current position of the player or cursor
        Vector3 position = GetCurrentPosition();
        Vector3Int tilePosition = groundTileMap.WorldToCell(position);

        // Check if there is a water tile at the position and remove it
        if (waterTileMap.HasTile(tilePosition))
        {
            waterTileMap.SetTile(tilePosition, null);
        }

        // Check if the tile is already taken
        if (groundTileMap.HasTile(tilePosition))
        {
            // Get the existing tile at the position
            TileBase existingTile = groundTileMap.GetTile(tilePosition);

            // Check if the existing tile is the same as the one being placed
            if (existingTile == tileList[tileIndex].tileBase)
            {
                return; // Tile is the same, do not allow placement
            }
        }

        // Place the new tile
        groundTileMap.SetTile(tilePosition, tileList[tileIndex].tileBase);
        money.data -= tileList[tileIndex].cost;
        moneySpentAction.InvokeAction();
        swingAction.InvokeAction();
        RebuildNavmesh();
    }

    // This method should return the current position where the tile should be placed
    private Vector3 GetCurrentPosition()
    {
        // Implement logic to get the current position
        // For example, you might get the player's position or the cursor position
        // This is a placeholder implementation
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void NextTile()
    {
        tileIndex++;
        if (tileIndex >= tileList.Count)
        {
            tileIndex = 0;
        }
        currentTileSpriteRenderer.sprite = tileList[tileIndex].tileSprite;
    }

    public void PreviousTile()
    {
        tileIndex--;
        if (tileIndex < 0)
        {
            tileIndex = tileList.Count - 1;
        }
        currentTileSpriteRenderer.sprite = tileList[tileIndex].tileSprite;
    }

    public int GetTileAmount(string tileName)
    {
        if (tileMapDict.ContainsKey(tileName))
        {
            return tileMapDict[tileName];
        }
        else
        {
            return 0;
        }
    }

    public Vector3 GetRandomTileGroundTile()
    {
        // make a list of all the tiles and then pick a random one
        List<Vector3> tilePositions = new List<Vector3>();
        for (int x = -tileMapSize.x; x < tileMapSize.x; x++)
        {
            for (int y = -tileMapSize.y; y < tileMapSize.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 worldPosition = groundTileMap.GetCellCenterWorld(tilePosition);
                if (groundTileMap.HasTile(tilePosition))
                {
                    tilePositions.Add(worldPosition);
                }
            }
        }
        //choose a random one and return its vector3 pos
        return tilePositions[UnityEngine.Random.Range(0, tilePositions.Count)];
    }
}
