using AStar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderData : MonoBehaviour {
    public static PathFinderData Instance { get; private set; }

    public GraphData graphData = new GraphData();
    //List<Node> OuterNodes = new List<Node>();

    public void Awake() {
        Instance = this;
    }
    public void OnDestroy() {
        Instance = null;
    }


    //void Update() {
    //    if (Input.GetKeyDown(KeyCode.F1)) {
            
    //        System.Action<List<Node>, EnemyManager> PathOfNodes = delegate (List<Node> nodes, EnemyManager manager) {
    //            print("time.time：" + Time.time);
    //            print("委托开始");
    //            if (nodes == null || nodes.Count == 0) {
    //                print("Node为空");
    //                return;
    //            }
                
    //            foreach(var node in nodes) {
    //                OuterNodes.Add(node);
    //            }
    //        };
    //        FindShortestPathOfNodes(56,1,PathOfNodes);
    //        StartCoroutine(Check());
    //    }
    //}

    //IEnumerator Check() {
    //    while (OuterNodes.Count == 0) {
    //        yield return new WaitForSeconds(1);
    //    }
    //    print("outer nodes count:" + OuterNodes.Count);
    //    foreach (var node in OuterNodes) {
    //        print("node in outer nodes:" + node.ID);
    //    }
    //    OuterNodes.Clear();
    //}

    public int FindNearestNode(Vector3 point) {
        float minDistance = float.MaxValue;
        Node nearestNode = null;

        foreach (var node in graphData.nodes) {
            if (Vector3.Distance(node.Position, point) < minDistance) {
                minDistance = Vector3.Distance(node.Position, point);
                nearestNode = node;
            }
        }
        if(nearestNode != null) {
            if (Vector3.Distance(point, nearestNode.Position) < 10) {
                return nearestNode.ID;
            }
        }
        return -1;
    }

    public void EnableNode(int nodeID, bool enable) {
        if (graphData == null) {
            Debug.LogError("Graph Data not found");
            return;
        }

        Node node = graphData.GetNode(nodeID);
        if (node == null) {
            Debug.Log("Node not found");
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
            Debug.Log("Path not found");
            return;
        }
        path.isOpen = (enable);
    }

}

