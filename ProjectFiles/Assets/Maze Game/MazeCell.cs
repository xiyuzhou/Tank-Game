using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] public GameObject[] walls;
    [SerializeField] MeshRenderer floor;
    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                //floor.material.color = Color.white;
                break;
            case NodeState.Current:
                //floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                //floor.material.color = Color.blue;
                break;
        }
    }
}
public enum NodeState
{
    Available,
    Current,
    Completed
}