using System.Collections.Generic;
using UnityEngine;

public class NewMazeGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] GameObject wallPrefab;

    // Representing wall directions (right, left, front, back)
    enum WallDirection { Right = 0, Left = 1, Front = 2, Back = 3 }

    private void Start()
    {
        int[,,] mazeData = GenerateMazeData(mazeSize);
        // You can use this mazeData for other purposes or pass it to another function to generate the maze visually.
        Debug.Log(mazeData[1, 1,1]);
        GenerateMazeFromData(mazeData);
    }

    int[,,] GenerateMazeData(Vector2Int size)
    {
        int[,,] mazeData = new int[size.x, size.y, 4]; // 4 represents the four directions (right, left, front, back)

        List<Vector2Int> currentPath = new List<Vector2Int>();
        List<Vector2Int> completedNodes = new List<Vector2Int>();

        Vector2Int startNode = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        currentPath.Add(startNode);
        completedNodes.Add(startNode);

        while (completedNodes.Count < size.x * size.y)
        {
            int currentNodeX = currentPath[currentPath.Count - 1].x;
            int currentNodeY = currentPath[currentPath.Count - 1].y;

            List<int> possibleNextDirections = new List<int>();

            if (currentNodeX < size.x - 1 && !completedNodes.Contains(new Vector2Int(currentNodeX + 1, currentNodeY)))
                possibleNextDirections.Add((int)WallDirection.Right);

            if (currentNodeX > 0 && !completedNodes.Contains(new Vector2Int(currentNodeX - 1, currentNodeY)))
                possibleNextDirections.Add((int)WallDirection.Left);

            if (currentNodeY < size.y - 1 && !completedNodes.Contains(new Vector2Int(currentNodeX, currentNodeY + 1)))
                possibleNextDirections.Add((int)WallDirection.Front);

            if (currentNodeY > 0 && !completedNodes.Contains(new Vector2Int(currentNodeX, currentNodeY - 1)))
                possibleNextDirections.Add((int)WallDirection.Back);

            if (possibleNextDirections.Count > 0)
            {
                int chosenDirectionIndex = Random.Range(0, possibleNextDirections.Count);
                int chosenDirection = possibleNextDirections[chosenDirectionIndex];

                mazeData[currentNodeX, currentNodeY, chosenDirection] = 1;

                Vector2Int nextNode = new Vector2Int(currentNodeX, currentNodeY);

                switch (chosenDirection)
                {
                    case (int)WallDirection.Right:
                        nextNode.x++;
                        break;
                    case (int)WallDirection.Left:
                        nextNode.x--;
                        break;
                    case (int)WallDirection.Front:
                        nextNode.y++;
                        break;
                    case (int)WallDirection.Back:
                        nextNode.y--;
                        break;
                }

                if (!completedNodes.Contains(nextNode))
                {
                    currentPath.Add(nextNode);
                    completedNodes.Add(nextNode);
                }
            }
            else
            {
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }

        return mazeData;
    }
    void GenerateMazeFromData(int[,,] mazeData)
    {
        // Assuming wallPrefab is a cube or another GameObject representing a wall

        for (int x = 0; x < mazeData.GetLength(0); x++)
        {
            for (int y = 0; y < mazeData.GetLength(1); y++)
            {
                for (int direction = 0; direction < 4; direction++)
                {
                    if (mazeData[x, y, direction] == 1)
                    {
                        Vector3 wallPosition = new Vector3(x, 0, y);

                        switch ((WallDirection)direction)
                        {
                            case WallDirection.Right:
                                wallPosition += new Vector3(0.5f, 0, 0);
                                break;
                            case WallDirection.Left:
                                wallPosition += new Vector3(-0.5f, 0, 0);
                                break;
                            case WallDirection.Front:
                                wallPosition += new Vector3(0, 0, 0.5f);
                                break;
                            case WallDirection.Back:
                                wallPosition += new Vector3(0, 0, -0.5f);
                                break;
                        }

                        Instantiate(wallPrefab, wallPosition, Quaternion.identity);
                    }
                }
            }
        }
    }
}
