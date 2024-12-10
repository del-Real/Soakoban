/*********************************************************************
* Class Name: State
* Author/s name: Alberto del Real
* Class description: Stores and calculates mutable objects coordinates
*********************************************************************/

using System;
using System.Security.Cryptography;
using System.Text;

namespace Sokoban;

public class State {
    private string id;
    private (int, int) player;
    private int[][] boxes;

    // State constructor 1
    public State(string level) {
        player = ObtainPlayerCoords(level);
        boxes = ObtainBoxesCoords(level);
        id = CalculateMD5Hash(player, boxes);
    }

    // State constructor 2
    public State((int, int) player, int[][] boxes) {
        this.player = player;
        this.boxes = boxes;
        id = CalculateMD5Hash(player, boxes);
    }

    // Properties
    // Player Getter and Setter
    public (int, int) Player {
        get => player;
        set => player = value;
    }

    // Boxes Getter and Setter
    public int[][] Boxes {
        get => boxes;
        set => boxes = value;
    }

    // Id Getter and Setter
    public string Id {
        get => id;
        set => id = value;
    }

    /*********************************************************************
    * Method name: ObtainPlayerCoords
    *
    * Description of the Method: Determines the coordinates of player
    *
    * Calling arguments: string level
    *   
    * Return value: (int, int), returns player position tuple
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: Checks if there is more than one player and if there is 
    * not a player in the level
    *
    *********************************************************************/

    private (int, int) ObtainPlayerCoords(string level) {

        int playerCount = 0;
        int i = 0, j = 0;
        int x = 0, y = 0;

        foreach (char c in level) {
            if (c == '@' || c == '+') {
                playerCount++;

                // Throw exception if more than one player found
                if (playerCount > 1) {
                    throw new InvalidOperationException("More than one player found in the level.");
                }

                x = i;
                y = j;
            }
            else if (c == '\n') {
                i++;
                j = 0;
            }
            else {
                j++;
            }
        }

        // Throw exception if no player found
        if (playerCount == 0) {
            throw new InvalidOperationException("No player found in the level.");
        }

        return (x, y);
    }

    public void MovePlayer(int x, int y) {
        player = (x, y);  // Set the player to the new position
    }

    /*********************************************************************
    * Method name: ObtainBoxesCoords
    *
    * Description of the Method: Determines the coordinates of boxes that
    * are already on target
    *
    * Calling arguments: string level
    *
    * Return value: int[][], returns the boxes on target coordinates array.
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private int[][] ObtainBoxesCoords(string level) {

        int i = 0, j = 0;
        List<int[]> boxesCoords = new List<int[]>();
        int[][] boxes;

        foreach (char c in level) {
            if (c == '$' || c == '*') {
                boxesCoords.Add(new int[] { i, j });
                j++;
            }
            else if (c == '\n') {
                i++;
                j = 0;
            }
            else {
                j++;
            }
        }

        boxes = boxesCoords.OrderBy(box => box[0])
                            .ThenBy(box => box[1])
                            .ToArray();
        return boxes;
    }


    /*********************************************************************
    * Method name: GetMD5Hash
    *
    * Description of the Method: Generates a MD5 hash based on the player 
    * and box position
    *
    * Calling arguments: (int, int) player, int[][] boxes
    *
    * Return value: string, returns the MD5 hash
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    public string CalculateMD5Hash((int, int) player, int[][] boxes) {

        string input = $"({player.Item1},{player.Item2})[";

        for (int i = 0; i < boxes.Length; i++) {
            input += $"({String.Join(",", boxes[i])})";
            if (i < boxes.Length - 1) {
                input += ",";
            }
        }
        input += "]";

        using (MD5 md5 = MD5.Create()) {
            // Convert string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Compute MD5 hash
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++) {
                sb.Append(hashBytes[i].ToString("X2")); // X2 formats to hexadecimal
            }

            return sb.ToString();
        }
    }
}
