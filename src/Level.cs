/*********************************************************************
* Class Name: Level
* Author/s name: Alberto del Real
* Class description: Stores and calculates inmutable level objects and 
* calculates the number of rows and columns
*********************************************************************/

namespace Sokoban;

public class Level {

    private int rows;
    private int cols;
    private int[][] walls;
    private int[][] targets;

    // Level constructor
    public Level(string level) {
        rows = RowsCount(level);
        cols = ColsCount(level);
        walls = ObtainWallsCoords(level);        // Initialize walls coords
        targets = ObtainTargetsCoords(level);    // Initialize targets coords
    }

    // Properties
    public int Rows => rows;                    // Getter rows
    public int Cols => cols;                    // Getter cols
    public int[][] Walls => walls;              // Getter walls
    public int[][] Targets => targets;          // Getter targets

    /*********************************************************************
     * Method name: RowsCount
     *
     * Description of the Method: Counts the number of rows
     *
     * Calling arguments: string level
     *
     * Return value: int, returns the number of rows
     *
     * Required Files: Does not make use of any external files
     *
     * List of Checked Exceptions and an indication of when each exception
     * is thrown: None
     *
     *********************************************************************/

    private int RowsCount(string level) {
        return level.Split('\n').Length;
    }

    /*********************************************************************
    * Method name: ColsCount
    *
    * Description of the Method: Counts the number of columns
    *
    * Calling arguments: string level
    *
    * Return value: int, returns the number of columns
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private int ColsCount(string level) {

        int count = 0, maxCols = 0;

        foreach (char c in level) {
            if (c == '\n') {
                if (count > maxCols) {
                    maxCols = count;
                }
                count = 0;
            }
            else {
                count++;
            }
        }
        return maxCols;
    }

    /*********************************************************************
    * Method name: ObtainWallsCoords
    *
    * Description of the Method: Determines the coordinates of walls
    *
    * Calling arguments: string level
    *
    * Return value: int[][], returns wall coordinates array
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private int[][] ObtainWallsCoords(string level) {

        int i = 0, j = 0;
        List<int[]> wallCoords = new List<int[]>();
        int[][] walls;

        foreach (char c in level) {
            if (c == '#') {
                wallCoords.Add(new int[] { i, j });
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

        walls = wallCoords.ToArray();
        return walls;
    }

    /*********************************************************************
    * Method name: ObtainTargetsCoords
    *
    * Description of the Method: Determines the coordinates of targets
    *
    * Calling arguments: string level
    *
    * Return value: int[][], returns target coordinates array
    *
    * Required Files: Does not make use of any external files
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private int[][] ObtainTargetsCoords(string level) {

        int i = 0, j = 0;
        List<int[]> targetsCoords = new List<int[]>();
        int[][] targets;

        foreach (char c in level) {
            if (c == '.' || c == '*' || c == '+') {
                targetsCoords.Add(new int[] { i, j });
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
        targets = targetsCoords.ToArray();
        return targets;
    }

}
