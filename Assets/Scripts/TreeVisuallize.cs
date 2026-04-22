using UnityEngine;

public class TreeVisuallize : MonoBehaviour
{
    public GameObject Prefabs;

    public int createAmount;

    private BinarySearchTree<int, GameObject> tree = new();

    private void Start()
    {
        for(int i = 0; i < createAmount; i++)
        {
            tree[Random.Range(1, 11)] = Instantiate(Prefabs);
        }

        Pow();
    }

    private void Pow()
    {
        foreach(var tmep in tree.InOrderTraversal())
        {
            tree.root.Value.transform.position = new Vector3(1f, tree.root.Height, 0f);

            var left = tree.root.Left;

            while (left != null)
            {
                Debug.Log("왼쪽-----------------------");
                Debug.Log(left.Height);
                left.Value.transform.position = new Vector3(tree.root.Value.transform.position.x * -2f, left.Height, 0f);
                left = left.Left;
            }

            var right = tree.root.Right;
            while (right != null)
            {
                Debug.Log("오른쪽-----------------------");
                Debug.Log(right.Height);
                right.Value.transform.position = new Vector3(tree.root.Value.transform.position.x * 2f, right.Height, 0f);
                right = right.Right;
            }
        }
    }
}
