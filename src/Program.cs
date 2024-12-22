using System;
using Raylib_cs;
using System.Data;
using System.Security;

namespace Sokoban;

class Program {
    public static void Main(string[] args) {

        // Check if there are not arguments (True = run GUI / False = parse arguments)
        if (args.Length == 0) {
            // Renderer rendererGUI = new Renderer();
        }
        else {

            if (args.Length > 0 && args.Length < 3) {
                Console.WriteLine("\nUsage: ./sokoban.exe '<level>' <strategy> <depth>\n");
                throw new ArgumentException("Required arguments not found.");
            }

            Param param = new Param(args);

            // Valid characters defined
            HashSet<char> validCharacters = new HashSet<char> { '#', '@', '$', '.', '*', '+', ' ', '\n' };

            // Check if all characters in the level string are valid
            foreach (char c in param.Level) {
                if (!validCharacters.Contains(c)) {
                    throw new InvalidOperationException("Character not valid: " + c);
                }
            }

            Level level = new Level(param.Level);
            State state = new State(param.Level);

            // Check if there is a different number of boxes and targets
            if (state.Boxes.Length > level.Targets.Length) {
                throw new InvalidOperationException("The level must have at least as many targets as boxes");
            }

            // Check if there are not at least one box and one target
            if (state.Boxes.Length == 0 || level.Targets.Length == 0) {
                throw new InvalidOperationException("The level must have at least one box and one target");
            }

            HashSet<string> validStrategies = new HashSet<string> { "BFS", "DFS", "UC", "GREEDY", "A*" };

            if (!validStrategies.Contains(param.Strategy)) {
                Console.WriteLine("\nValid strategies: BFS / DFS / UC / GREEDY / A*\n");
                throw new ArgumentException("Invalid strategy.");
            }

            // Parse strategy (optional)
            if (param.Strategy != null) {
                switch (param.Strategy) {
                    case "BFS":
                        break;

                    case "DFS":
                        break;

                    case "UC":
                        break;

                    case "GREEDY":
                        break;

                    case "A*":
                        break;

                    default:
                        Console.WriteLine("Unknown parameter: " + param.Strategy);
                        break;
                }
            }

            // ProblemDomain(level, state);
            List<Node> solutionPath = SearchAlgorithm(level, state, param.Depth, param.Strategy);
            Renderer rendererCLI = new Renderer(level, state);
            rendererCLI.Render(solutionPath);
        }
    } // End Main

    // Prints ID, rows, columns, walls, targets, player and boxes positions in the level
    static void ProblemDomain(Level level, State state) {

        Console.WriteLine("\nID: " + state.Id);
        Console.WriteLine("\tRows: " + level.Rows);
        Console.WriteLine("\tColumns: " + level.Cols);
        Console.Write("\tWalls: ");
        PrintCoordsArray(level.Walls);
        Console.Write("\tTargets: ");
        PrintCoordsArray(level.Targets);
        Console.Write("\tPlayer: (" + state.Player.Item1 + "," + state.Player.Item2 + ")");
        Console.Write("\n\tBoxes: ");
        PrintCoordsArray(state.Boxes);
    }

    // Returns the list of successors, which are all the possible moves that the player can make in a given state
    static List<Successor> SuccessorFunction(Level level, State state) {

        List<Successor> successors = new List<Successor>();

        (int, int) playerMove;
        (int, int) boxMove;
        int cost = 1;

        // Store direction coordinates
        var directions = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1), };

        // Hashsets of walls and boxes array to check faster
        HashSet<(int, int)> wallsSet = new HashSet<(int, int)>();
        HashSet<(int, int)> boxesSet = new HashSet<(int, int)>();

        foreach (var wall in level.Walls) {
            wallsSet.Add((wall[0], wall[1]));
        }

        foreach (var box in state.Boxes) {
            boxesSet.Add((box[0], box[1]));
        }

        // Copy of boxes array
        int[][] newBoxes = new int[state.Boxes.Length][];
        for (int i = 0; i < state.Boxes.Length; i++) {
            newBoxes[i] = new int[state.Boxes[i].Length];
            Array.Copy(state.Boxes[i], newBoxes[i], state.Boxes[i].Length);
        }

        // Loop to check every direction (clock-wise)
        for (int i = 0; i < directions.Length; i++) {
            playerMove = (state.Player.Item1 + directions[i].Item1, state.Player.Item2 + directions[i].Item2);

            // Check wall collision
            if (!wallsSet.Contains(playerMove)) {

                State sucState = new State(playerMove, newBoxes);
                Successor successor = new Successor("NOTHING", sucState, 0);

                // Check box collision
                if (boxesSet.Contains(playerMove)) {
                    // Box collision

                    for (int j = 0; j < newBoxes.Length; j++) {
                        boxMove.Item1 = newBoxes[j][0] + directions[i].Item1;
                        boxMove.Item2 = newBoxes[j][1] + directions[i].Item2;

                        // Check if new box position is empty
                        if (!wallsSet.Contains(boxMove) && !boxesSet.Contains(boxMove)) {
                            // Find the box that is getting pushed by the player
                            if (newBoxes[j][0] == playerMove.Item1 && newBoxes[j][1] == playerMove.Item2) {

                                sucState.MovePlayer(newBoxes[j][0], newBoxes[j][1]);

                                newBoxes[j][0] += directions[i].Item1;
                                newBoxes[j][1] += directions[i].Item2;

                                // Sort coordinates (disabled as it was causing rendering issues)
                                sucState.Boxes = newBoxes
                                        // .OrderBy(box => box[0])
                                        // .ThenBy(box => box[1])
                                        .Select(box => (int[])box.Clone()) // Deep copy each box
                                        .ToArray();

                                // Calculate new state id
                                sucState.Id = sucState.CalculateMD5Hash(sucState.Player, sucState.Boxes);

                                switch (i) {
                                    case 0:
                                        successor.Action = "U";
                                        successor.Cost = cost;
                                        successors.Add(successor);
                                        break;
                                    case 1:
                                        successor.Action = "R";
                                        successor.Cost = cost;
                                        successors.Add(successor);
                                        break;
                                    case 2:
                                        successor.Action = "D";
                                        successor.Cost = cost;
                                        successors.Add(successor);
                                        break;
                                    case 3:
                                        successor.Action = "L";
                                        successor.Cost = cost;
                                        successors.Add(successor);
                                        break;
                                }

                                // Revert boxes move to reset to original coords
                                newBoxes[j][0] -= directions[i].Item1;
                                newBoxes[j][1] -= directions[i].Item2;
                            }
                        }
                    }
                }
                else {
                    // No box collision
                    sucState.Id = sucState.CalculateMD5Hash(playerMove, newBoxes);

                    switch (i) {
                        case 0:
                            successor.Action = "u";
                            successor.Cost = cost;
                            successors.Add(successor);
                            break;
                        case 1:
                            successor.Action = "r";
                            successor.Cost = cost;
                            successors.Add(successor);
                            break;
                        case 2:
                            successor.Action = "d";
                            successor.Cost = cost;
                            successors.Add(successor);
                            break;
                        case 3:
                            successor.Action = "l";
                            successor.Cost = cost;
                            successors.Add(successor);
                            break;
                    }
                }
            }
        }

        return successors;
    }

    // Returns if the objective function (all boxes are on targets) is achieved.
    static bool ObjectiveFunction(Level level, State state) {

        HashSet<(int, int)> boxesSet = new HashSet<(int, int)>();
        HashSet<(int, int)> targetsSet = new HashSet<(int, int)>();

        foreach (var box in state.Boxes) {
            boxesSet.Add((box[0], box[1]));
        }

        foreach (var target in level.Targets) {
            targetsSet.Add((target[0], target[1]));
        }

        bool allBoxesOnTarget = boxesSet.IsSubsetOf(targetsSet);

        if (allBoxesOnTarget) {
            state.CalculateMD5Hash(state.Player, state.Boxes);
        }

        return allBoxesOnTarget;
    }

    // Returns the list of nodes from the root
    static List<Node> SearchAlgorithm(Level level, State state, string depth, string strategy) {

        var comparator = Comparer<Node>.Create((x, y) => {
            int comparatorValue = x.ValueNode.CompareTo(y.ValueNode);
            return comparatorValue != 0 ? comparatorValue : x.IdNode.CompareTo(y.IdNode);
        });

        var frontier = new PriorityQueue<Node>(comparator);
        HashSet<string> visited = new HashSet<string>();
        bool solution = false;

        int maxDepth = int.Parse(depth);
        int totalNodes = 0;

        Node rootNode = new Node(totalNodes, state, null, "NOTHING", 0, 0.00f, 0.00f, 0.00f);
        rootNode.AssignValue(strategy, level);
        frontier.Add(rootNode); // insert root node in frontier

        Node node = rootNode;

        // Check if frontier is not empty and is not solution
        while (frontier.Count != 0 && !solution) {
            // is solution
            // extract node from frontier
            node = frontier.Poll();

            if (ObjectiveFunction(level, node.StateNode)) {
                //node state is objective
                solution = true;
            }
            else {
                if (node.Depth < maxDepth && !visited.Contains(node.StateNode.Id)) {
                    // correct depth and state not visited
                    visited.Add(node.StateNode.Id);

                    List<Successor> nodeSuccessors = SuccessorFunction(level, node.StateNode);
                    // expand node
                    foreach (var nodeSuc in nodeSuccessors) {
                        totalNodes++;
                        Node childNode = new Node(totalNodes, nodeSuc.State, node, nodeSuc.Action, node.Depth + 1, node.Cost + 1, 0.00f, 0.00f);
                        childNode.AssignValue(strategy, level);
                        frontier.Add(childNode);
                    }
                }
            }
        }

        // Check if a solution was found
        if (solution) {
            List<Node> solutionPath = new List<Node>();

            // Traverse from the current node to the root
            while (node != null) {
                solutionPath.Add(node);
                node = node.ParentNode;
            }
            solutionPath.Reverse();

            return solutionPath;
        }

        else {
            Console.WriteLine("There is no solution.");
            return null;
        }
    }

    // Prints a jagged array for debugging purposes
    static void PrintCoordsArray(int[][] array) {
        Console.Write("[");

        for (int i = 0; i < array.Length; i++) {
            Console.Write("(" + array[i][0] + "," + array[i][1] + ")");

            if (i < array.Length - 1) {
                Console.Write(",");
            }
        }

        Console.WriteLine("]");
    }
}