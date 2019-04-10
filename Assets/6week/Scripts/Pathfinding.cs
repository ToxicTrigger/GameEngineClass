using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour {

	public Transform seeker, target;
	Grid grid;

    float updateTick;

	void Start () 
	{
		grid = GetComponent<Grid> ();	
	}

	void Update()
	{
        if(updateTick >= 0.2f)
        {
            FindPath(seeker.position, target.position);
            updateTick = 0;
        }
        else
        {
            updateTick += Time.deltaTime;
        }
    }

	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
        Stopwatch sw = new Stopwatch();
        sw.Start();

		Node startNode = grid.NodeFromWorldPosition(startPos);
		Node targetNode = grid.NodeFromWorldPosition(targetPos);

		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add (startNode);
        int max = 0;

        while (openSet.Count > 0 && max <= 100) {

            // OpenSet에서 가장 낮은 fCost를 가지는 노드를 가져온다. 
            // 만일 fCost가 동일할 경우 gCost가 적은 쪽을 택함. 
            Node currentNode = openSet[0]; 
            Node min = currentNode;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].gCost > currentNode.gCost))
                {
                    currentNode = openSet[i];
                }
            }
                        /* 해당 코드를 작성할 것 */


            // 찾은 노드가 최종 노드면 루프 종료
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                sw.Stop();
                UnityEngine.Debug.Log("Elapsed Time : " + sw.ElapsedMilliseconds + "ms");
                return;
            }

            // 해당 노드를 ClosedSet으로 이동
            /* 해당 코드를 작성할 것 */
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // 현재 노드의 이웃들을 모두 가져와 비교
            foreach (Node n in grid.GetNeighbours(currentNode))
            {
                // 이웃 노드가 이미 ClosedSet에 있거나 걸어다니지 못하는 곳이면 제외한다.
                /* 해당 코드를 작성할 것 */
                if(closedSet.Contains(n) || !n.walkable) continue;

                // 현재 노드를 기점에서 파생된 이웃 노드의 fCost를 계산한다.
                /* 해당 코드를 작성할 것 */
                int g = currentNode.gCost + GetDistance(currentNode, n);
                int h = GetDistance(n, targetNode);
                int f = g+h;

                // 오픈셋에 현재 노드가 없으면 노드에 점수를 설정한 후 추가한다.
                if (!openSet.Contains(n))
                {
                    /* 해당 코드를 작성할 것 */
                    n.gCost = g;
                    n.hCost = h;
                    n.parent = currentNode;
                    openSet.Add(n);
                }
                else
                {
                    // 오픈셋에 현재 노드가 이미 있으면 수치를 비교한 후 경로를 교체한다. 동일하면 g수치가 큰 쪽으로 교체한다.
                    /* 해당 코드를 작성할 것 */
                    if(n.fCost > f || (n.fCost == f && n.gCost > g))
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
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if(dstX > dstY)
		{
			return 14*dstY + 10*(dstX - dstY);
		}

		return 14*dstX + 10*(dstY - dstX);
	}
}
