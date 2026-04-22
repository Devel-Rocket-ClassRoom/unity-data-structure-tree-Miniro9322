using UnityEngine;

public class TreeTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var bst = new BinarySearchTree<string, string>();

        bst["1"] = "1";
        bst["2"] = "2";
        bst["3"] = "3";
        bst["4"] = "4";
        bst["5"] = "5";
        bst["6"] = "6";
        bst["7"] = "7";

        foreach (var pair in bst.InOrderTraversal())
        {
            Debug.Log(pair);
        }

        foreach (var pair in bst.LevelOrderTraversal())
        {
            Debug.Log(pair);
        }
    }
}
