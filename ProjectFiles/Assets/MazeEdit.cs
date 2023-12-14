using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEdit : MonoBehaviour
{
    public MazeCell[] cells;
    public Vector2Int mazeSize;
    public MazeCell[,] Mazecells;
    public void CheckForSameWalls()
    {
        Mazecells = new MazeCell[mazeSize.x, mazeSize.y];
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                Mazecells[i,j] = cells[i* mazeSize.y + j];
                Debug.Log(i * mazeSize.y + j);
            }
        }

        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                if(j < mazeSize.y-1)
                {
                    if (Mazecells[i, j].walls[2].activeSelf && Mazecells[i, j + 1].walls[3].activeSelf)
                    {
                        Mazecells[i, j].walls[2].SetActive(false);
                    }
                }
                if (i < mazeSize.x-1)
                {
                    if (Mazecells[i, j].walls[0].activeSelf  && Mazecells[i + 1, j].walls[1].activeSelf)
                    {
                        Mazecells[i, j].walls[0].SetActive(false);
                    }
                }
            }
        }
    }
    public void AddTag()
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                for(int k = 0; k < 4; k++)
                {
                    Mazecells[i, j].walls[k].tag = "wall";
                }
            }
        }
    }
    public void AddWallComponent()
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    // Check if the current wall is not an outer wall
                    if (!(i == 0 && k == 1) &&  // Check left wall
                        !(i == mazeSize.x - 1 && k == 0) &&  // Check right wall
                        !(j == 0 && k == 3) &&  // Check bottom wall
                        !(j == mazeSize.y - 1 && k == 2))  // Check top wall
                    {
                        // Add WallHp component to the current wall

                        Mazecells[i, j].walls[k].gameObject.AddComponent<WallHp>();
                        Mazecells[i, j].walls[k].gameObject.AddComponent<PhotonView>();
                    }
                }
            }
        }
    }
    public void DestoryNotActive()
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (!Mazecells[i, j].walls[k].activeSelf)
                    {
                        DestroyImmediate(Mazecells[i, j].walls[k].gameObject);
                    }
                }
            }
        }
    }
}
