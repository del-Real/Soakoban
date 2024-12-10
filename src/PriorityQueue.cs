/*********************************************************************
* Class Name: PriorityQueue
* Author/s name: Alberto del Real
* Class description: creation of a priority queue and its functions
* used for the node frontier
*********************************************************************/

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

    /*********************************************************************
    * Method name: Add
    *
    * Description of the Method: Add an item to the priority queue
    *
    * Calling arguments: T item
    *
    * Return value: void, does not return any values.
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    public void Add(T item) {
        sortedSet.Add(item);
    }

    /*********************************************************************
    * Method name: Poll
    *
    * Description of the Method: Returns the first element of the priority
    * queue and removes it
    *
    * Calling arguments: None
    *
    * Return value: T element
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: Check if the queue is empty
    *
    *********************************************************************/

    public T Poll() {
        if (sortedSet.Count == 0) {
            throw new InvalidOperationException("The queue is empty.");
        }

        var firstElement = sortedSet.Min;
        sortedSet.Remove(firstElement);
        return firstElement;
    }

    /*********************************************************************
    * Method name: Peek
    *
    * Description of the Method: Returns the first element of the priority
    * queue (does not remove it)
    *
    * Calling arguments: None
    *
    * Return value: T element
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: Checks if the queue is empty
    *
    *********************************************************************/

    public T Peek() {
        if (sortedSet.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        return sortedSet.Min; // returns minimum value without remove it
    }
}