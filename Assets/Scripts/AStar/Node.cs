using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStar {

    public enum PathLineType {
        Straight,
        CatmullRomCurve,
    }

    [System.Serializable]
    public class Node {
        [SerializeField] private Vector3 position;
        public Vector3 Position {
            get { return position; }
        }
        [SerializeField] private bool isOpen;
        public bool IsOpen {
            get { return isOpen; }
        }

        [SerializeField] public Node prevNode;
        [SerializeField] public int ID;
        [SerializeField] public float F {
            get { return H + G; }
        }
        [SerializeField] public float H;
        [SerializeField] public float G;

        public Node(Vector3 pos) {
            position = pos;
        }
        public void SetPosition(Vector3 pos) {
            position = pos;
        }
        public void SetOpen(bool value) {
            isOpen = value;
        }
    }

    [System.Serializable]
    public class Path {
        public Path(int a, int b) {
            NodeAId = a;
            NodeBId = b;
        }

        [SerializeField] public int cost;
        [SerializeField] public int ID;
        [SerializeField] public int NodeAId = -1;
        [SerializeField] public int NodeBId = -1;
        [SerializeField] public bool isOneWay = false;
        [SerializeField] public bool isOpen = true;
    }

    [System.Serializable]
    public class GraphData {
        //[SerializeField] public PathLineType lineType;
        [SerializeField] public Color lineColor = Color.yellow;
        [SerializeField] public float nodeSize = 0.5f;
        [SerializeField] public float heightFromTheGround = 0;
        [SerializeField] public string groundColliderLayerName = "Default";

        [SerializeField] public List<Node> nodes;
        [SerializeField] public List<Path> paths;

        [HideInInspector] public Dictionary<int, Node> nodesSorted;
        [HideInInspector] public Dictionary<int, Path> pathsSorted;

        public GraphData() {
            nodes = new List<Node>();
            paths = new List<Path>();
            nodesSorted = new Dictionary<int, Node>();
            pathsSorted = new Dictionary<int, Path>();
        }

        public Node GetNode(int ID) {
            if (nodesSorted.ContainsKey(ID))
                return nodesSorted[ID];
            return null;
        }
        public Path GetPath(int ID) {
            if (pathsSorted.ContainsKey(ID))
                return pathsSorted[ID];
            return null;
        }
        public Path GetPathBetweenNodeId(int from, int to) {
            foreach (Path path in paths) {
                if (
                    (path.NodeAId == from && path.NodeBId == to)
                    || (path.NodeBId == from && path.NodeAId == to)
                ) {
                    return path;
                }
            }
            return null;
        }

        public Path GetPathBetweenNode(Node from, Node to) {
            if (from == null | to == null)
                return null;

            return GetPathBetweenNodeId(from.ID, to.ID);
        }

        public void GenerateIDs() {
            if (nodes == null)
                return;

            // 创建结点ID
            {
                int maxID = 0;

                for (int i = 0; i < nodes.Count; i++) {
                    if (nodes[i].ID > maxID)
                        maxID = nodes[i].ID;
                }

                maxID = maxID + 1;

                for (int i = 0; i < nodes.Count; i++) {
                    if (nodes[i].ID <= 0)
                        nodes[i].ID = maxID++;
                }
            }

            // 创建路径ID
            {
                int maxID = 0;
                for (int i = 0; i < paths.Count; i++) {
                    if (paths[i].ID > maxID)
                        maxID = paths[i].ID;
                }

                maxID = maxID + 1;

                for (int i = 0; i < paths.Count; i++) {
                    if (paths[i].ID <= 0)
                        paths[i].ID = maxID++;
                }
            }

            // 排序结点和路径
            {
                pathsSorted.Clear();
                nodesSorted.Clear();

                for (int i = 0; i < nodes.Count; i++) {
                    nodesSorted[nodes[i].ID] = nodes[i];
                }

                for (int i = 0; i < paths.Count; i++) {
                    pathsSorted[paths[i].ID] = paths[i];
                }
            }
        }

    }
}
