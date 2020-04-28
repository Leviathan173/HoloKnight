﻿using AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    private static PathFinder _instance;
    public static PathFinder instance { get { return _instance; } }

    public GraphData graphData = new GraphData();

    public void Awake() {
        _instance = this;
    }
    public void OnDestroy() {
        _instance = null;
    }


    public void FindShortestPathOfNodes(int fromNodeID, int toNodeID) {
        if (QPathFinder.Logger.CanLogInfo) QPathFinder.Logger.LogInfo(" FindShortestPathAsynchronous triggered from " + fromNodeID + " to " + toNodeID, true);
        StartCoroutine(FindShortestPathAsynchonousInternal(fromNodeID, toNodeID));
    }

    public int FindNearestNode(Vector3 point) {
        float minDistance = float.MaxValue;
        Node nearestNode = null;

        foreach (var node in graphData.nodes) {
            if (Vector3.Distance(node.Position, point) < minDistance) {
                minDistance = Vector3.Distance(node.Position, point);
                nearestNode = node;
            }
        }

        return nearestNode != null ? nearestNode.ID : -1;
    }

    public void EnableNode(int nodeID, bool enable) {
        if (graphData == null) {
            Debug.LogError("Graph Data not found");
            return;
        }

        Node node = graphData.GetNode(nodeID);
        if (node == null) {
            Debug.LogError("Node not found");
            return;
        }
        node.SetOpen(enable);
    }
    public void EnablePath(int pathID, bool enable) {
        if (graphData == null) {
            Debug.LogError("Graph Data not found");
            return;
        }

        Path path = graphData.GetPath(pathID);
        if (path == null) {
            Debug.LogError("Path not found");
            return;
        }
        path.isOpen = (enable);
    }

    private IEnumerator FindShortestPathAsynchonousInternal(int fromNodeID, int toNodeID) {

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

            float minCost = 99999;
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
                }
                StartCoroutine(Follow(finalPath));
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
            Debug.LogError("Path not found between " + fromNodeID + " and " + toNodeID);
            yield break;
        }

        Debug.LogError("Unknown error while finding the path!");

        yield break;
    }

    private IEnumerator Follow(List<Node> finalPath) {
        yield return null;
    }
}
