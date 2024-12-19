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

    // Counts the number of rows
    private int RowsCount(string level) {
        return level.Split('\n').Length;
    }

    // Counts the number of columns
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

    // Returns the coordinates of walls
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

    // Returns the coordinates of targets
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
