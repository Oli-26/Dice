using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject[,] grid = new GameObject[16,10];
    public List<GameObject> path = new List<GameObject>();

    public Sprite HighlightedSquareSprite;
    void Start()
    {
        InitialiseGrid();
        Level1();
    }

    
    void Update()
    {
        
    }

    void InitialiseGrid(){
        for(int x = 0; x < 16; x++){
            for(int y= 0; y < 10; y++){
                grid[x,y] = new GameObject();
                grid[x,y].AddComponent<Square>();
                grid[x,y].GetComponent<Square>().Initialise(x, y, gameObject, HighlightedSquareSprite);
            }
        }
    }

    public void Level1(){
        path.Add(grid[0,3]);
        path.Add(grid[7,3]);
        path.Add(grid[7,1]);
        path.Add(grid[12,1]);
        path.Add(grid[12,4]);
        path.Add(grid[14,4]);
        path.Add(grid[14,6]);
        path.Add(grid[12,6]);
        path.Add(grid[12,8]);
        path.Add(grid[0,8]);
    }

    public int GetAmountOfPoints(){
        return path.Count;
    }

    public Vector3 GetNthSquareOnPath(int n){
        return path[n].transform.position;
    }

    public Vector3 SnapToGrid(Vector3 position){
        return new Vector3(Mathf.Ceil(position.x) - 0.5f, Mathf.Ceil(position.y) - 0.5f, 0f);
    }
}
