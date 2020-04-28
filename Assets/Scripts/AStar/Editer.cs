﻿using AStar;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(PathFinder))]
public class PathFinderEditor : Editor {
    enum SceneMode {
        AddNode,
        EditNode,
        ConnectPath,
        None
    }

    [MenuItem("GameObject/Create a 2D PathFinder in scene with a collider")]
    public static void Create2DPathFinderObjectInScene() {
        if (FindObjectOfType<PathFinder>() == null) {
            var managerGo = new GameObject("PathFinder");
            var colliderGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderGo.name = "Ground";
            colliderGo.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.black);

            colliderGo.transform.localScale = new Vector3(100f, 100f, 1f); ;
            var boxCollider = colliderGo.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            managerGo.AddComponent<PathFinder>();
        } else {
            //if (QPathFinder.Logger.CanLogError) QPathFinder.Logger.LogError("PathFollower Script already exists!");
            Debug.LogError("PathFollower Script already exists!");
        }
    }

    #region OnInspectorGUI

    public override void OnInspectorGUI() {
        showDefaultInspector = EditorGUILayout.Toggle("Show Default inspector", showDefaultInspector);
        if (showDefaultInspector) {
            DrawDefaultInspector();
        } else {
            CustomGUI.DrawSeparator(Color.gray);
            ShowNodesAndPathInInspector();
        }
    }

    private void ShowNodesAndPathInInspector() {
        script.graphData.nodeSize = EditorGUILayout.Slider("Node gizmo Size", script.graphData.nodeSize, 0.1f, 3f);
        script.graphData.lineColor = EditorGUILayout.ColorField("Path Color", script.graphData.lineColor);
        //script.graphData.lineType = (PathLineType)EditorGUILayout.EnumPopup("Path Type", script.graphData.lineType);
        //script.graphData.heightFromTheGround = EditorGUILayout.FloatField("Offset from ground( Height )", script.graphData.heightFromTheGround);
        script.graphData.groundColliderLayerName = EditorGUILayout.TextField("Ground collider layer name", script.graphData.groundColliderLayerName);
        EditorGUILayout.Space();
        GUILayout.Label("<size=12><b>Nodes</b></size>", CustomGUI.GetStyleWithRichText());

        if (script.graphData.nodes.Count > 0) {
            showNodeIDsInTheScene = EditorGUILayout.Toggle("Show Node IDs in scene", showNodeIDsInTheScene);

            List<Node> nodeList = script.graphData.nodes;
            for (int j = 0; j < nodeList.Count; j++) {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("\t" + "Node <Color=" + nodeGUITextColor + ">" + nodeList[j].ID + "</Color>", CustomGUI.GetStyleWithRichText(), GUILayout.Width(120f));

                    nodeList[j].SetPosition(EditorGUILayout.Vector3Field("", nodeList[j].Position));
                    EditorGUILayout.LabelField("Enable", EditorStyles.miniLabel, GUILayout.Width(50f)); nodeList[j].SetOpen(EditorGUILayout.Toggle(nodeList[j].IsOpen));

                    if (GUILayout.Button("+", GUILayout.Width(25f)))
                        AddNode(nodeList[j].Position + Vector3.right + Vector3.up, j + 1);
                    if (GUILayout.Button("-", GUILayout.Width(25f)))
                        DeleteNode(j);
                }
                GUILayout.EndHorizontal();
            }
        } else {
            EditorGUILayout.LabelField("<Color=green> Nodes are empty. Use <b>Add Node</b> in scene view to create Nodes!</Color>", CustomGUI.GetStyleWithRichText(CustomGUI.SetAlignmentForText(TextAnchor.MiddleCenter)));
        }
        EditorGUILayout.Space();
        GUILayout.Label("<size=12><b>Paths</b></size>", CustomGUI.GetStyleWithRichText());

        if (script.graphData.paths.Count > 0) {
            showPathIDsInTheScene = EditorGUILayout.Toggle("Show Path IDs in scene", showPathIDsInTheScene);
            drawPathsInTheScene = EditorGUILayout.Toggle("Draw Paths", drawPathsInTheScene);
            showCostsInTheScene = EditorGUILayout.Toggle("Show Path Costs in scene", showCostsInTheScene);

            List<Path> paths = script.graphData.paths;
            for (int j = 0; j < paths.Count; j++) {
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("\t" + "Path <Color=" + pathGUITextColor + ">" + paths[j].ID + "</Color>", CustomGUI.GetStyleWithRichText(), GUILayout.Width(120f));

                    EditorGUILayout.LabelField("From", EditorStyles.miniLabel, GUILayout.Width(30f)); paths[j].NodeAId = EditorGUILayout.IntField(paths[j].NodeAId, GUILayout.Width(50f));
                    EditorGUILayout.LabelField("To", EditorStyles.miniLabel, GUILayout.Width(25f)); paths[j].NodeBId = EditorGUILayout.IntField(paths[j].NodeBId, GUILayout.Width(50f));
                    EditorGUILayout.LabelField("<Color=" + costGUITextColor + ">" + "Cost" + "</Color>", CustomGUI.GetStyleWithRichText(EditorStyles.miniLabel), GUILayout.Width(30f)); paths[j].cost = EditorGUILayout.IntField(paths[j].cost, GUILayout.Width(50f));

                    EditorGUILayout.LabelField("One Way", EditorStyles.miniLabel, GUILayout.Width(50f)); paths[j].isOneWay = EditorGUILayout.Toggle(paths[j].isOneWay);
                    EditorGUILayout.LabelField("Enable", EditorStyles.miniLabel, GUILayout.Width(50f)); paths[j].isOpen = EditorGUILayout.Toggle(paths[j].isOpen);

                    if (GUILayout.Button("+", GUILayout.Width(25f)))
                        AddPath(j + 1);
                    if (GUILayout.Button("-", GUILayout.Width(25f)))
                        DeletePath(j);
                }
                GUILayout.EndHorizontal();
            }
        } else {
            EditorGUILayout.LabelField("<Color=green> Paths are empty. Use <b>Connect Nodes</b> in scene view to create Paths!</Color>", CustomGUI.GetStyleWithRichText(CustomGUI.SetAlignmentForText(TextAnchor.MiddleCenter)));
        }

        if (GUI.changed)
            MarkThisDirty();
    }

    #endregion

    #region On Scene Rendering and Scene GUI

    private void OnSceneGUI() {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlID);
        DrawGUIWindowOnScene();
        UpdateMouseInput();

        if (sceneMode == SceneMode.AddNode) {
            DrawNodes(Color.green);
        } else if (sceneMode == SceneMode.EditNode) {
            DrawNodes(Color.magenta, true);
        } else if (sceneMode == SceneMode.ConnectPath) {
            DrawNodes(Color.green, false, script.graphData.GetNode(selectedNodeForConnectNodesMode), Color.red);
        } else
            DrawNodes(Color.gray);
        DrawPathLine();
        CheckGUIChanged();
    }

    private void CheckGUIChanged() {
        if (GUI.changed) {
            SceneView.RepaintAll();
        }
    }
    private void DrawGUIWindowOnScene() {
        GUILayout.Window(1, new Rect(0f, 25f, 70f, 80f),
                                                        delegate (int windowID) {
                                                            EditorGUILayout.BeginHorizontal();

                                                            sceneMode = (SceneMode)GUILayout.SelectionGrid((int)sceneMode, new string[] { "Add Node", "Move Node", "Connect Nodes", "None" }, 1);

                                                            GUI.color = Color.white;

                                                            EditorGUILayout.EndHorizontal();
                                                        }
                        , "Mode");
        GUILayout.Window(2, new Rect(0, 155f, 70f, 80f),
                                                        delegate (int windowID) {
                                                            EditorGUILayout.BeginVertical();

                                                            if (GUILayout.Button("Delete Node"))
                                                                DeleteNode();

                                                            if (GUILayout.Button("Delete Path"))
                                                                DeletePath();

                                                            if (GUILayout.Button("Clear All")) {
                                                                ClearNodes();
                                                                ClearPaths();
                                                            }

                                                            if (GUILayout.Button("Refresh Data")) {
                                                                script.graphData.GenerateIDs();
                                                            }
                                                            GUI.color = Color.white;

                                                            EditorGUILayout.EndVertical();
                                                        }
                            , "");
    }


    private void DrawNodes(Color color, bool canMove = false, Node selectedNode = null, Color colorForSelected = default(Color)) {
        Handles.color = color;
        foreach (var node in script.graphData.nodes) {
            if (selectedNode != null && node == selectedNode)
                Handles.color = colorForSelected;
            else
                Handles.color = color;

            if (canMove)
                node.SetPosition(Handles.FreeMoveHandle(node.Position, Quaternion.identity, script.graphData.nodeSize, Vector3.zero, Handles.SphereHandleCap));
            else
                Handles.SphereHandleCap(0, node.Position, Quaternion.identity, script.graphData.nodeSize, EventType.Repaint);
        }
        Handles.color = Color.white;
        DrawGUIDisplayForNodes();
        Handles.color = Color.white;
    }

    private void DrawPathLine() {
        List<Path> paths = script.graphData.paths;
        List<Node> nodes = script.graphData.nodes;
        Vector3 currNode;
        Vector2 guiPosition;

        if (paths == null || nodes == null)
            return;

        Handles.color = script.graphData.lineColor;
        Node a, b;

        for (int i = 0; i < paths.Count; i++) {
            if (!paths[i].isOpen)
                continue;

            a = b = null;
            if (script.graphData.nodesSorted.ContainsKey(paths[i].NodeAId))
                a = script.graphData.nodesSorted[paths[i].NodeBId];

            if (script.graphData.nodesSorted.ContainsKey(paths[i].NodeBId))
                b = script.graphData.nodesSorted[paths[i].NodeBId];

            if (a != null && b != null && a != b && a.IsOpen && b.IsOpen) {
                if (drawPathsInTheScene)
                    Handles.DrawLine(a.Position, b.Position);

                Handles.BeginGUI();
                {
                    currNode = (a.Position + b.Position) / 2;
                    guiPosition = HandleUtility.WorldToGUIPoint(currNode);
                    string str = "";
                    if (showPathIDsInTheScene)
                        str += "<Color=" + pathGUITextColor + ">" + paths[i].ID.ToString() + "</Color>";
                    if (showCostsInTheScene) {
                        if (!string.IsNullOrEmpty(str))
                            str += "<Color=" + "#ffffff" + ">" + "  Cost: " + "</Color>";
                        str += "<Color=" + costGUITextColor + ">" + paths[i].cost.ToString() + "</Color>";
                    }

                    if (!string.IsNullOrEmpty(str))
                        GUI.Label(new Rect(guiPosition.x - 10, guiPosition.y - 30, 40, 20), str, CustomGUI.GetStyleWithRichText());
                }
                Handles.EndGUI();
            }
        }
        Handles.color = Color.white;
    }

    private void DrawGUIDisplayForNodes() {
        if (!showNodeIDsInTheScene)
            return;

        Node currNode;
        Vector2 guiPosition;
        Handles.BeginGUI();

        for (int i = 0; i < script.graphData.nodes.Count; i++) {
            currNode = script.graphData.nodes[i];
            guiPosition = HandleUtility.WorldToGUIPoint(currNode.Position);
            GUI.Label(new Rect(guiPosition.x - 10, guiPosition.y - 30, 20, 20), "<Color=" + nodeGUITextColor + ">" + currNode.ID.ToString() + "</Color>", CustomGUI.GetStyleWithRichText());
        }
        Handles.EndGUI();
    }

    #endregion

    #region Input Method

    void UpdateMouseInput() {
        Event e = Event.current;
        if (e.type == EventType.MouseDown) {
            if (e.button == 0)
                OnMouseClick(e.mousePosition);
        } else if (e.type == EventType.MouseUp) {
            MarkThisDirty();
            SceneView.RepaintAll();
        }
    }

    void OnMouseClick(Vector2 mousePos) {
        if (sceneMode == SceneMode.AddNode) {
            LayerMask backgroundLayerMask = 1 << LayerMask.NameToLayer(script.graphData.groundColliderLayerName);
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100000f, backgroundLayerMask)) {
                Vector3 hitPos = hit.point;
                hitPos += (-ray.direction.normalized)/* * script.graphData.heightFromTheGround*/;
                AddNode(hitPos);
            } else
                QPathFinder.Logger.LogError("No collider detected with layer " + script.graphData.groundColliderLayerName + "! Could not add node! ");
        } else if (sceneMode == SceneMode.ConnectPath) {
            LayerMask backgroundLayerMask = 1 << LayerMask.NameToLayer(script.graphData.groundColliderLayerName);
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, backgroundLayerMask)) {
                Vector3 hitPos = hit.point;
                TryAddPath(hitPos);
            } else
                QPathFinder.Logger.LogError("No collider detected with layer " + script.graphData.groundColliderLayerName + "! Could not add node! ");
        }
    }

    #endregion


    #region Node and Path methods

    void AddNode(Vector3 position, int addIndex = -1) {
        Node nodeAdded = new Node(position);
        if (addIndex == -1)
            script.graphData.nodes.Add(nodeAdded);
        else
            script.graphData.nodes.Insert(addIndex, nodeAdded);

        script.graphData.GenerateIDs();

        QPathFinder.Logger.LogInfo("Node with ID:" + nodeAdded.ID + " Added!");
    }

    void DeleteNode(int removeIndex = -1) {
        List<Node> nodeList = script.graphData.nodes;
        if (nodeList == null || nodeList.Count == 0)
            return;

        if (removeIndex == -1)
            removeIndex = nodeList.Count - 1;

        Node nodeRemoved = nodeList[removeIndex];
        nodeList.RemoveAt(removeIndex);
        script.graphData.GenerateIDs();

        QPathFinder.Logger.LogInfo("Node with ID:" + nodeRemoved.ID + " Removed!");
    }

    void ClearNodes() {
        script.graphData.nodes.Clear();
        QPathFinder.Logger.LogWarning("All Nodes are cleared!");
    }

    void AddPath(int addIndex = -1, int from = -1, int to = -1) {
        if (from != -1 && to != -1) {
            if (from == to) {
                QPathFinder.Logger.LogError("Preventing from adding Path to the same node.");
                return;
            }
            Path pd = script.graphData.GetPathBetweenNodeId(from, to);
            if (pd != null) {
                QPathFinder.Logger.LogError("We already have a path between these nodes. New Path not added!");
                return;
            }
        }
        Path newPath = new Path(from, to);
        if (addIndex == -1)
            script.graphData.paths.Add(newPath);
        else
            script.graphData.paths.Insert(addIndex, newPath);
        script.graphData.GenerateIDs();

        QPathFinder.Logger.LogInfo("Path with ID:" + newPath.ID + " Added");
    }

    void DeletePath(int removeIndex = -1) {
        List<Path> pathList = script.graphData.paths;
        if (pathList == null || pathList.Count == 0)
            return;

        if (removeIndex == -1)
            removeIndex = pathList.Count - 1;

        Path removedPath = pathList[removeIndex];
        pathList.RemoveAt(removeIndex);
        script.graphData.GenerateIDs();

        QPathFinder.Logger.LogInfo("Path with ID:" + removedPath.ID + " Removed");
    }

    void ClearPaths() {
        script.graphData.paths.Clear();
        QPathFinder.Logger.LogWarning("All Paths are cleared!");
    }

    void TryAddPath(Vector3 position) {
        Node selectedNode = script.graphData.GetNode(script.FindNearestNode(position));
        if (selectedNode == null) {
            QPathFinder.Logger.LogError("Could not find any nearest Node to connect to!");
            return;
        }
        if (selectedNodeForConnectNodesMode != -1) {
            AddPath(-1, selectedNodeForConnectNodesMode, selectedNode.ID);
            QPathFinder.Logger.LogInfo("Connected " + selectedNodeForConnectNodesMode.ToString() + " and " + selectedNode.ID);
            selectedNodeForConnectNodesMode = -1;
        } else {
            selectedNodeForConnectNodesMode = selectedNode.ID;
            QPathFinder.Logger.LogInfo("Selected : " + selectedNodeForConnectNodesMode.ToString() + ". Now click another node to join these two");
        }
    }

    #endregion

    #region PRIVATE

    private void OnEnable() {
        sceneMode = SceneMode.None;
        script = target as PathFinder;
        script.graphData.GenerateIDs();
    }

    // When anything in inspector is changed, this will mark the scene or the prefab dirty
    private void MarkThisDirty() {
        if (Application.isPlaying)
            return;

        if (PrefabUtility.GetCorrespondingObjectFromSource(script.gameObject) != null) {
            //QPathFinder.Logger.LogInfo ( "Prefab for PathFinder found! Marked it Dirty ( Modified )");
            EditorUtility.SetDirty(script);
        } else {
            //QPathFinder.Logger.LogInfo ( "Prefab for PathFinder Not found! Marked the scene as Dirty ( Modified )");
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }


    private SceneMode sceneMode;
    private PathFinder script;

    const string nodeGUITextColor = "#ff00ffff";
    const string pathGUITextColor = "#00ffffff";
    const string costGUITextColor = "#0000ffff";

    private int selectedNodeForConnectNodesMode = -1;
    private bool showNodeIDsInTheScene = true;
    private bool showPathIDsInTheScene = true;
    private bool drawPathsInTheScene = true;
    private bool showCostsInTheScene = false;
    private bool showDefaultInspector = false;

    #endregion
}

