using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BinarySearchTree<Tkey, TValue> : IDictionary<Tkey, TValue> where Tkey : IComparable<Tkey>
{
    public TreeNode<Tkey, TValue> root;

    public BinarySearchTree()
    {
        root = null;
    }

    public TValue this[Tkey key]
    {
        get 
        {
            if(TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"키 {key} 없음");
            }
        }

        set => root = AddOrUpdate(root, key, value);
    }

    public ICollection<Tkey> Keys => InOrderTraversal().Select(kvp => kvp.Key).ToList();

    public ICollection<TValue> Values => InOrderTraversal().Select(kvp => kvp.Value).ToList();

    public int Count => CountNodes(root);

    protected virtual int CountNodes(TreeNode<Tkey, TValue> node)
    {
        if(node == null)
        {
            return 0;
        }

        return 1 + CountNodes(node.Left) + CountNodes(node.Right);
    }

    public bool IsReadOnly => false;

    public void Add(Tkey key, TValue value)
    {
        root = Add(root, key, value);
    }

    public void Add(KeyValuePair<Tkey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    protected virtual TreeNode<Tkey, TValue> Add(TreeNode<Tkey, TValue> node, Tkey key, TValue value)
    {
        if(node == null)
        {
            return new TreeNode<Tkey, TValue>(key, value);
        }

        int compare = key.CompareTo(node.Key);
        if(compare < 0)
        {
            node.Left = Add(node.Left, key, value);
        }
        else if(compare > 0)
        {
            node.Right = Add(node.Right, key, value);
        }
        else
        {
            throw new ArgumentException($"키 {node.Key}가  존재합니다.");
        }

        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<Tkey, TValue> AddOrUpdate(TreeNode<Tkey, TValue> node, Tkey key, TValue value)
    {
        if (node == null)
        {
            return new TreeNode<Tkey, TValue>(key, value);
        }

        int compare = key.CompareTo(node.Key);
        if (compare < 0)
        {
            node.Left = AddOrUpdate(node.Left, key, value);
        }
        else if (compare > 0)
        {
            node.Right = AddOrUpdate(node.Right, key, value);
        }
        else
        {
            node.Value = value;
        }

        UpdateHeight(node);
        return node;
    }

    public void Clear()
    {
        root = null;
    }

    public bool Contains(KeyValuePair<Tkey, TValue> item)
    {
        return TryGetValue(item.Key, out _);
    }

    public bool ContainsKey(Tkey key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<Tkey, TValue>[] array, int arrayIndex)
    {
        foreach(var item in this)
        {
            array[arrayIndex++] = item;
        }
    }

    public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }

    public virtual IEnumerable<KeyValuePair<Tkey, TValue>> InOrderTraversal()
    {
        return InOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<Tkey, TValue>> InOrderTraversal(TreeNode<Tkey, TValue> node)
    {
        if(node != null)
        {
            foreach(var kvp in InOrderTraversal(node.Left))
            {
                yield return kvp;
            }

            yield return new KeyValuePair<Tkey, TValue>(node.Key, node.Value);

            foreach (var kvp in InOrderTraversal(node.Right))
            {
                yield return kvp;
            }
        }
    }

    public virtual IEnumerable<KeyValuePair<Tkey, TValue>> LevelOrderTraversal()
    {
        return LevelOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<Tkey, TValue>> LevelOrderTraversal(TreeNode<Tkey, TValue> node)
    {
        if(node == null)
        {
            yield break;
        }

        var queue = new Queue<TreeNode<Tkey, TValue>>();
        queue.Enqueue(node);

        while(queue.Count > 0)
        {
            var current = queue.Dequeue();

            yield return new KeyValuePair<Tkey, TValue>(current.Key, current.Value);
            if(current.Left != null)
            {
                queue.Enqueue(current.Left);
            }
            if (current.Right != null)
            {
                queue.Enqueue(current.Right);
            }
        }
    }

    public virtual IEnumerable<KeyValuePair<Tkey, TValue>> PreOrderTraversal()
    {
        return PreOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<Tkey, TValue>> PreOrderTraversal(TreeNode<Tkey, TValue> node)
    {
        if (node != null)
        {
            yield return new KeyValuePair<Tkey, TValue>(node.Key, node.Value);

            foreach (var kvp in PreOrderTraversal(node.Left))
            {
                yield return kvp;
            }

            foreach (var kvp in PreOrderTraversal(node.Right))
            {
                yield return kvp;
            }
        }
    }

    public virtual IEnumerable<KeyValuePair<Tkey, TValue>> PostOrderTraversal()
    {
        return PostOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<Tkey, TValue>> PostOrderTraversal(TreeNode<Tkey, TValue> node)
    {
        if (node != null)
        {
            foreach (var kvp in PostOrderTraversal(node.Left))
            {
                yield return kvp;
            }

            foreach (var kvp in PostOrderTraversal(node.Right))
            {
                yield return kvp;
            }

            yield return new KeyValuePair<Tkey, TValue>(node.Key, node.Value);
        }
    }

    public bool Remove(Tkey key)
    {
        int initialCount = Count;
        root = Remove(root, key);
        return Count < initialCount;
    }

    public bool Remove(KeyValuePair<Tkey, TValue> item)
    {
        return Remove(item.Key);
    }

    protected virtual TreeNode<Tkey, TValue> Remove(TreeNode<Tkey, TValue> node, Tkey key)
    {
        if(node == null)
        {
            return node;
        }

        int compare = key.CompareTo(node.Key);
        if(compare < 0)
        {
            node.Left = Remove(node.Left, key);
        }
        else if(compare > 0)
        {
            node.Right = Remove(node.Right, key);
        }
        else
        {
            if(node.Left == null)
            {
                return node.Right;
            }
            else if(node.Right == null)
            {
                return node.Left;
            }

            TreeNode<Tkey, TValue> minNode = FindMin(node.Right);

            node.Key = minNode.Key;
            node.Value = minNode.Value;

            node.Right = Remove(node.Right, minNode.Key);
        }

        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<Tkey, TValue> FindMin(TreeNode<Tkey,TValue> node)
    {
        while(node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    public bool TryGetValue(Tkey key, out TValue value)
    {
        return TryGetValue(root, key, out value);
    }

    protected bool TryGetValue(TreeNode<Tkey, TValue> node, Tkey key, out TValue value)
    {
        if(node == null)
        {
            value = default;
            return false;
        }

        int compare = key.CompareTo(node.Key);

        if(compare == 0)
        {
            value = node.Value;
            return true;
        }
        else if(compare < 0)
        {
            return TryGetValue(node.Left, key, out value);
        }
        else
        {
            return TryGetValue(node.Right, key, out value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected virtual void UpdateHeight(TreeNode<Tkey, TValue> node)
    {
        node.Height = Mathf.Max(Height(node.Left), Height(node.Right)) + 1;
    }

    protected virtual int Height(TreeNode<Tkey, TValue> node)
    {
        return node == null ? 0 : node.Height;
    }
}
