namespace Sokoban;
public class Node {

    private int idNode;
    private State stateNode;
    private Node? parentNode;
    private string action;
    private int depth;
    private float cost;
    private float heuristic;
    private float valueNode;

    // Node constructor
    public Node(int idNode, State stateNode, Node? parentNode, string action, int depth, float cost, float heuristic, float valueNode) {
        this.idNode = idNode;
        this.stateNode = stateNode;
        this.parentNode = parentNode;
        this.action = action;
        this.depth = depth;
        this.cost = cost;
        this.heuristic = heuristic;
        this.valueNode = valueNode;
    }

    // IdNode Getter and Setter
    public int IdNode {
        get => idNode;
        set => idNode = value;
    }

    // StateNode Getter and Setter
    public State StateNode {
        get => stateNode;
        set => stateNode = value;
    }

    // ParentNode Getter and Setter
    public Node? ParentNode {
        get => parentNode;
        set => parentNode = value;
    }

    // Action Getter and Setter
    public string Action {
        get => action;
        set => action = value;
    }

    // Depth Getter and Setter
    public int Depth {
        get => depth;
        set => depth = value;
    }

    // Cost Getter and Setter
    public float Cost {
        get => cost;
        set => cost = value;
    }

    // Heuristic Getter and Setter
    public float Heuristic {
        get => heuristic;
        set => heuristic = value;
    }

    // ValueNode Getter and Setter
    public float ValueNode {
        get => valueNode;
        set => valueNode = value;
    }

    /*********************************************************************
     * Method name: AssignValue
     *
     * Description of the Method: Assign value to the value depending on
     * the entered strategy
     *
     * Calling arguments: string strategy, Level level
     *
     * Return value: void, does not return any values
     *
     * Required Files: Does not make use of any external files
     *
     * List of Checked Exceptions and an indication of when each exception
     * is thrown: None
     *
     *********************************************************************/

    public void AssignValue(string strategy, Level level) {
        switch (strategy) {
            case "BFS":
                valueNode = depth;
                break;
            case "DFS":
                valueNode = 1.0f / (depth + 1);
                break;
            case "UC":
                valueNode = cost;
                break;
            case "GREEDY":
                heuristic = HeuristicManhattan(level, stateNode);
                valueNode = heuristic;
                break;
            case "A*":
                heuristic = HeuristicManhattan(level, stateNode);
                valueNode = cost + heuristic;
                break;
        }
    }

    /*********************************************************************
     * Method name: HeuristicManhattan
     *
     * Description of the Method: Calculate the Manhattan distance between 
     * a box and a target
     *
     * Calling arguments: Level level, State state
     *
     * Return value: int, returns the minimum distance
     *
     * Required Files: Does not make use of any external files
     *
     * List of Checked Exceptions and an indication of when each exception
     * is thrown: None
     *
     *********************************************************************/

    public int HeuristicManhattan(Level level, State state) {
        int[][] boxes = state.Boxes;
        int[][] targets = level.Targets;
        int[][] dMBoxesTargets = new int[boxes.Length][];
        int[] minDistances = new int[boxes.Length];

        for (int i = 0; i < boxes.Length; i++) {
            dMBoxesTargets[i] = new int[targets.Length];
        }

        int totalMin = 0;

        // Manhattan distances for each box
        for (int i = 0; i < boxes.Length; i++) {

            minDistances[i] = int.MaxValue;

            for (int j = 0; j < targets.Length; j++) {
                int Trow = targets[j][0];
                int Tcolumn = targets[j][1];
                int Brow = boxes[i][0];
                int Bcolumn = boxes[i][1];

                // Manhattan distance formula
                int dManhattan = Math.Abs(Trow - Brow) + Math.Abs(Tcolumn - Bcolumn);
                dMBoxesTargets[i][j] = dManhattan;

                // Update the minimum distance for the current box
                if (dManhattan < minDistances[i]) {
                    minDistances[i] = dManhattan;
                }
            }
        }

        // Calculate the total of the minimum distances
        for (int i = 0; i < minDistances.Length; i++) {
            totalMin += minDistances[i];
        }

        return totalMin;
    }

    /*********************************************************************
     * Method name: PrintNode
     *
     * Description of the Method: Concatenate every variable of a node into
     * a string and prints it
     *
     * Calling arguments: None
     *
     * Return value: void, does not return any values
     *
     * Required Files: Does not make use of any external files
     *
     * List of Checked Exceptions and an indication of when each exception
     * is thrown: None
     *
     *********************************************************************/

    public void PrintNode() {
        float roundedValueNode = (float)Math.Round(valueNode, 2, MidpointRounding.AwayFromZero);

        string result = $"{idNode},{stateNode.Id},{(parentNode != null ? parentNode.idNode.ToString() : "None")},{action},{depth},{cost:F2},{heuristic:F2},{roundedValueNode:F2}";
        Console.WriteLine(result);
    }

}
