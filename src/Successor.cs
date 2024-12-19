namespace Sokoban;

public class Successor {

    private string action;
    private State state;
    private int cost;

    // Constructor
    public Successor(string action, State state, int cost) {
        this.action = action;   // Initialize action
        this.state = state;     // Initialize id
        this.cost = cost;       // Initialize cost
    }

    // Properties
    public string Action {
        get => action;          // Getter for action
        set => action = value;  // Setter for action
    }

    public State State {
        get => state;           // Getter for id
        set => state = value;   // Setter for id
    }

    public int Cost {
        get => cost;            // Getter for cost
        set => cost = value;    // Setter for cost
    }

    // Concatenate every variable of a successor into and prints it
    public void PrintSuccessor() {
        string result = $"[{action},{state.Id},{cost}]";
        Console.WriteLine("\t" + result); // Print the result to the console
    }
}