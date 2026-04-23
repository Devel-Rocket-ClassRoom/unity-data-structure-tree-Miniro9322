using UnityEngine;

public class PriorityQueueTest : MonoBehaviour
{
    private PriorityQueue<int, int> queue = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        queue.Enqueue(3, 1);
        queue.Enqueue(6, 2);
        queue.Enqueue(8, 3);
        queue.Enqueue(9, 2);
        queue.Enqueue(10, 3);
        queue.Enqueue(12, 4);
        queue.Enqueue(7, 2);

        queue.Dequeue();
    }
}
