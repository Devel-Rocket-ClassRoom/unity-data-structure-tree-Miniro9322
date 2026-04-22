using System.Collections.Generic;
using UnityEngine;

public class TreeVisuallize : MonoBehaviour
{
    public GameObject Prefabs;

    public int createAmount;

    private readonly Dictionary<object, Vector3> nodePositions = new();
    private BinarySearchTree<int, GameObject> tree = new();
    public float horizontalSpacing = 2.0f;
    public float verticalSpacing = 2.0f;

    private void Awake()
    {
        tree = new BinarySearchTree<int, GameObject>();
        for (int i = 0; i < createAmount; i++)
        {
            tree[Random.Range(0, 100)] = Instantiate(Prefabs);
        }
        //AssignPositionsPow(tree.root, Vector3.zero, tree.root.Height);
        //AssignPositionsLevelOrder(tree.root);
        int xIndex = 0;
        AssignPositionsInOrder(tree.root, 0, ref xIndex);
    }

    private void Start()
    {
        foreach(var node in tree.InOrderTraversal())
        {
            node.Value.transform.position = nodePositions[node.Value];
        }

        DrawConnections(tree.root);
    }



    private void AssignPositionsPow<TKey, TValue>(TreeNode<TKey, TValue> node, Vector3 position, int height)
    {
        if (node == null) return;

        nodePositions[node.Value] = position;

        // TODO: 이번 레벨에서 자식을 좌우로 얼마나 벌릴지 계산
        //   힌트: horizontalSpacing * 0.5f * Mathf.Pow(2, ???)
        float offset = horizontalSpacing * 0.5f * Mathf.Pow(2, height);

        Vector3 childBase = position + Vector3.down * verticalSpacing;

        AssignPositionsPow(node.Left, childBase + Vector3.left * offset, height - 1);
        AssignPositionsPow(node.Right, childBase + Vector3.right * offset, height - 1);
    }

    private void AssignPositionsLevelOrder<TKey, TValue>(TreeNode<TKey, TValue> root)
    {
        var levels = new List<List<TreeNode<TKey, TValue>>>();
        var queue = new Queue<(TreeNode<TKey, TValue> node, int depth)>();

        queue.Enqueue((root, 0));

        while (queue.Count > 0)
        {
            var (node, depth) = queue.Dequeue();

            // TODO: levels 리스트 크기가 depth보다 작으면 빈 List를 추가해 늘려준다
            while (levels.Count <= depth) { levels.Add(new List<TreeNode<TKey, TValue>>()); }

            levels[depth].Add(node);

            // TODO: 좌/우 자식을 depth + 1로 큐에 넣기
            if (node.Left != null)
                queue.Enqueue((node.Left, depth + 1));
            if (node.Right != null)
                /* ??? */
                queue.Enqueue((node.Right, depth + 1));
        }

        for (int depth = 0; depth < levels.Count; depth++)
        {
            float y = -depth * verticalSpacing;
            var row = levels[depth];

            for (int i = 0; i < row.Count; i++)
            {
                // TODO: i번째 노드의 x좌표는?
                nodePositions[row[i].Value] = new Vector3((i - (row.Count - 1) * 0.5f) * horizontalSpacing, y, 0f);
            }
        }
    }

    private void AssignPositionsInOrder<TKey, TValue>(TreeNode<TKey, TValue> node, int depth, ref int xIndex)
    {
        if (node == null) return;

        // TODO: 왼쪽 서브트리 먼저 방문 (depth + 1)
        /* ??? */
        AssignPositionsInOrder(node.Left, depth + 1, ref xIndex);

        // TODO: 자신의 좌표 기록 — x는 xIndex 기반, y는 depth 기반
        nodePositions[node.Value] = new Vector3(xIndex * horizontalSpacing, -depth * verticalSpacing, 0f);
        xIndex++;

        // TODO: 오른쪽 서브트리 방문 (depth + 1)
        /* ??? */
        AssignPositionsInOrder(node.Right, depth + 1, ref xIndex);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.parent = transform;

        var lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        lr.useWorldSpace = true;
    }

    private void DrawConnections(TreeNode<int, GameObject> node)
    {
        if (node == null) return;

        Vector3 parentPos = node.Value.transform.position;

        if (node.Left != null)
        {
            Vector3 leftPos = node.Left.Value.transform.position;
            DrawLine(parentPos, leftPos);
            DrawConnections(node.Left);
        }

        if (node.Right != null)
        {
            Vector3 rightPos = node.Right.Value.transform.position;
            DrawLine(parentPos, rightPos);
            DrawConnections(node.Right);
        }
    }
}
