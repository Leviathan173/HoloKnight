using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private GraphData graphData;
    void Start() {
        graphData = PathFinderData.Instance.graphData;
    }
    /// <summary>
    /// 异步寻找对应路径
    /// </summary>
    /// <param name="fromNodeID">起始结点</param>
    /// <param name="toNodeID">终点</param>
    /// <param name="manager">敌人管理器</param>
    /// <param name="callback">返回委托</param>
    public void FindShortestPathOfNodes(int fromNodeID, int toNodeID, EnemyManager manager, System.Action<List<Node>, EnemyManager> callback) {
        print(" FindShortestPathAsynchronous triggered from " + fromNodeID + " to " + toNodeID);
        StartCoroutine(FindShortestPathAsynchonous(fromNodeID, toNodeID, manager, callback));
    }
    /// <summary>
    /// 寻路协程
    /// </summary>
    /// <param name="fromNodeID">起始结点</param>
    /// <param name="toNodeID">终点</param>
    /// <param name="manager">敌人管理器</param>
    /// <param name="callback">返回委托</param>
    /// <returns></returns>
    private IEnumerator FindShortestPathAsynchonous(int fromNodeID, int toNodeID, EnemyManager manager, System.Action<List<Node>, EnemyManager> callback) {
        float start = Time.time;
        if (callback == null || manager == null || fromNodeID < 0 || toNodeID < 0) {
            //callback(null, null);
            yield break;
        }


        int startPointID = fromNodeID;
        int endPointID = toNodeID;
        bool found = false;

        graphData.GenerateIDs();

        Node startPoint = graphData.nodesSorted[startPointID];
        Node endPoint = graphData.nodesSorted[endPointID];
            

        foreach (var point in graphData.nodes) {
            point.H = -1;
            point.prevNode = null;
        }

        List<Node> completedPoints = new List<Node>();
        List<Node> nextPoints = new List<Node>();
        List<Node> finalPath = new List<Node>();

        startPoint.G = 0;
        startPoint.H = Vector3.Distance(startPoint.Position, endPoint.Position);
        nextPoints.Add(startPoint);

        while (true) {
            Node leastCostPoint = null;

            float minCost = float.MaxValue;
            foreach (var point in nextPoints) {
                if (point.H <= 0)
                    point.H = Vector3.Distance(point.Position, endPoint.Position) + Vector3.Distance(point.Position, startPoint.Position);

                if (minCost > point.F) {
                    leastCostPoint = point;
                    minCost = point.F;
                }
            }

            if (leastCostPoint == null)
                break;

            // 如果结束
            if (leastCostPoint == endPoint) {
                found = true;
                Node prevPoint = leastCostPoint;
                while (prevPoint != null) {
                    finalPath.Insert(0, prevPoint);
                    prevPoint = prevPoint.prevNode;
                }

                if (finalPath != null) {
                    string str = "";
                    foreach (var a in finalPath) {
                        str += "=>" + a.ID.ToString();
                    }
                    print("Path found between " + fromNodeID + " and " + toNodeID + ":" + str);
                    foreach (var node in finalPath) {
                        print("node:" + node.ID);
                    }
                }
                callback(finalPath, manager);
                float end = Time.time;
                print("Time:" + (end - start));
                yield break;
            }

            foreach (var path in graphData.paths) {
                if (path.NodeAId == leastCostPoint.ID
                || path.NodeBId == leastCostPoint.ID) {
                    if (path.isOneWay) {
                        if (leastCostPoint.ID == path.NodeBId)
                            continue;
                    }

                    if (!path.isOpen)
                        continue;

                    Node otherPoint = path.NodeAId == leastCostPoint.ID ?
                                            graphData.nodesSorted[path.NodeBId] : graphData.nodesSorted[path.NodeAId];

                    if (!otherPoint.IsOpen)
                        continue;

                    if (otherPoint.H <= 0)
                        otherPoint.H = Vector3.Distance(otherPoint.Position, endPoint.Position) + Vector3.Distance(otherPoint.Position, startPoint.Position);

                    if (completedPoints.Contains(otherPoint))
                        continue;

                    if (nextPoints.Contains(otherPoint)) {
                        if (otherPoint.G >
                            (leastCostPoint.G + path.cost)) {
                            otherPoint.G = leastCostPoint.G + path.cost;
                            otherPoint.prevNode = leastCostPoint;
                        }
                    } else {
                        otherPoint.G = leastCostPoint.G + path.cost;
                        otherPoint.prevNode = leastCostPoint;
                        nextPoints.Add(otherPoint);
                    }
                }
            }

            nextPoints.Remove(leastCostPoint);
            completedPoints.Add(leastCostPoint);

            yield return null;
        }

        if (!found) {
            print("Path not found between " + fromNodeID + " and " + toNodeID);
            callback(null, null);
            yield break;
        }

        Debug.LogError("Unknown error while finding the path!");
        callback(null, null);
        yield break;
    }
}
