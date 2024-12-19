namespace Sokoban;

public class PriorityQueue<T> {
    private SortedSet<T> sortedSet;
    private Comparer<T> comparer;

    public PriorityQueue(Comparer<T> customComparer) {
        comparer = customComparer;
        sortedSet = new SortedSet<T>(customComparer);
    }

    public SortedSet<T> SortedSet => sortedSet;     // Getter sortedSet
    public int Count => sortedSet.Count;            // Items count


    // Add an item to the priority queue
    public void Add(T item) {
        sortedSet.Add(item);
    }

    // Returns the first element of the priority and removes it from the queue
    public T Poll() {
        if (sortedSet.Count == 0) {
            throw new InvalidOperationException("The queue is empty.");
        }

        var firstElement = sortedSet.Min;
        sortedSet.Remove(firstElement);
        return firstElement;
    }

    // Returns the first element of the priority
    public T Peek() {
        if (sortedSet.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        return sortedSet.Min; // returns minimum value without remove it
    }
}