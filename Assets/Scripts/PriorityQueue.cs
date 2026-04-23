using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PriorityQueue<TElement, TPriority>
{
    public int Count { get; private set; }

    private List<(TElement Element, TPriority Priority)> queue;

    public void Enqueue(TElement element, TPriority priority)
    {
        if(queue == null)
        {
            queue = new();
        }

        queue.Add((element, priority));
        
        Count = queue.Count;

        HeapifyUp((element, priority));

        foreach (var value in queue)
        {
            Debug.Log(value.Element);
        }
        Debug.Log("--------------------인큐");
    }

    private void HeapifyUp((TElement Element, TPriority Priority) value)
    {
        int index = queue.IndexOf(value);

        if(index == 0)
        {
            return;
        }

        if (Comparer<TPriority>.Default.Compare(queue[index].Priority, queue[(index - 1) / 2].Priority) < 0)
        {
            var temp = queue[index];
            queue[index] = queue[(index - 1) / 2];
            queue[(index - 1) / 2] = temp;
        }
        else
        {
            return;
        }

        HeapifyUp(value);
    }


    public TElement Dequeue()
    {
        var value = queue[0].Element;

        var temp = queue[queue.Count - 1];
        queue.Remove(temp);
        queue[0] = temp;

        Count = queue.Count;

        HeapifyDown(queue[0]);

        foreach( var element in queue)
        {
            Debug.Log(element.Element);
        }
        Debug.Log("--------------------디큐");
        return value;
    }

    private void HeapifyDown((TElement Element, TPriority Priority) value)
    {
        int index = queue.IndexOf(value);

        if(2 * index + 1 > Count || 2 * index + 2 > Count)
        {
            return;
        }

        bool biggerThanLeft = Comparer<TPriority>.Default.Compare(queue[index].Priority, queue[2 * index + 1].Priority) > 0;
        bool biggerThanRight = Comparer<TPriority>.Default.Compare(queue[index].Priority, queue[2 * index + 2].Priority) > 0;

        var leftChild = queue[2 * index + 1];
        var rightChild = queue[2 * index + 2];

        if (biggerThanLeft && biggerThanRight)
        {
            if (Comparer<TPriority>.Default.Compare(rightChild.Priority, leftChild.Priority) >= 0)
            {
                var temp = queue[index];
                queue[index] = leftChild;
                leftChild = temp;
            }
            else
            {
                var temp = queue[index];
                queue[index] = rightChild;
                rightChild = temp;
            }
        }
        else if (biggerThanLeft)
        {
            var temp = queue[index];
            queue[index] = leftChild;
            leftChild = temp;
        }
        else if (biggerThanRight)
        {
            var temp = queue[index];
            queue[index] = rightChild;
            rightChild = temp;
        }
        else
        {
            return;
        }
        
        HeapifyDown(value);
    }

    public TElement Peek()
    {
        return queue[0].Element;
    }

    public void Clear()
    {
        queue.Clear();
    }
}
