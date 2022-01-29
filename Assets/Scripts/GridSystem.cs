using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public Square[,] grid = new Square[16,10];
    public List<Square> path = new List<Square>();
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
                grid[x,y] = ScriptableObject.CreateInstance<Square>();
                grid[x,y].Initialise(x, y, gameObject);
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
        return path[n].squareGameObject.transform.position;
    }
}
