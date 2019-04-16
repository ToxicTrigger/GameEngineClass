using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{

    public Transform seeker, target;
    Grid grid;

    float updateTick;

    void Start()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node startNode = grid.NodeFromWorldPosition(startPos);
        Node targetNode = grid.NodeFromWorldPosition(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        int max = 0;

        while (openSet.Count > 0 && max <= 100)
        {
            Node currentNode = openSet[0];
            Node min = currentNode;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].gCost > currentNode.gCost))
                {
                    currentNode = openSet[i];
                }
            }

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                sw.Stop();
                UnityEngine.Debug.Log("Elapsed Time : " + sw.ElapsedTicks);
                return;
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            foreach (Node n in grid.GetNeighbours(currentNode))
            {
                if (closedSet.Contains(n) || !n.walkable) continue;
                int g = currentNode.gCost + GetDistance(currentNode, n);
                int h = GetDistance(n, targetNode);
                int f = g + h;

                if (!openSet.Contains(n))
                {
                    n.gCost = g;
                    n.hCost = h;
                    n.parent = currentNode;
                    openSet.Add(n);
                }
                else
                {
                    if (n.fCost > f || (n.fCost == f && n.gCost > g))
                    {
                        n.gCost = g;
                        n.parent = currentNode;
                    }
                }
            }
            max++;
        }

    }

    // 경로를 보여준다.
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
