/*********************************************************************
* Class Name: Renderer
* Author/s name: Alberto del Real
* Class description: main entry point for Sokoban game solver with
* graphical visualization of the level using Raylib.
*********************************************************************/

using System;
using Raylib_cs;
using System.Numerics;

namespace Sokoban;

public class Renderer {

    private const int TILE_SIZE = 30;
    private int screenWidth;
    private int screenHeight;
    private int offset;

    private Texture2D playerTexture;
    private Texture2D wallTexture;
    private Texture2D boxTexture;
    private Texture2D targetTexture;

    private Level level;
    private State state;

    // Renderer constructor
    public Renderer(Level level, State state) {
        this.level = level;
        this.state = state;

        // Calculate screen dimensions based on level size
        int cols = level.Cols;
        int rows = level.Rows;

        screenWidth = (TILE_SIZE * cols) + (TILE_SIZE * 4);     // screen width set by tile size
        screenHeight = (TILE_SIZE * rows) + (TILE_SIZE * 4);    // screen height set by tile size
        offset = TILE_SIZE * 2;                                 // offset to add margins
    }

    /*********************************************************************
    * Method name: Initialize
    *
    * Description of the Method: Initializes the game window, sets up the 
    * frame rate, and loads the necessary textures for the game elements.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Requires the texture files located in the 
    * "resources" folder: player.png, wall.png, box.png, and target.png.
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void Initialize() {

        Raylib.SetTraceLogLevel(TraceLogLevel.None);                // disable Raylib log info
        Raylib.InitWindow(screenWidth, screenHeight, "Sokoban");    // window initilizer
        Raylib.SetTargetFPS(5);                                     // set frame rate

        playerTexture = Raylib.LoadTexture("resources/player.png");   // load player texture
        wallTexture = Raylib.LoadTexture("resources/wall.png");       // load wall texture
        boxTexture = Raylib.LoadTexture("resources/box.png");         // load box texture
        targetTexture = Raylib.LoadTexture("resources/target.png");   // load target texture
    }

    /*********************************************************************
    * Method name: Render
    *
    * Description of the Method: Manages the game loop, including drawing 
    * the game elements on the screen and handling window events.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Depends on the Initialize method to load required 
    * resources and textures.
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    public void Render() {
        Initialize();

        Color lightWhite = new Color(255, 255, 255, 32); // Set grid color

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            DrawGrid(lightWhite);
            DrawPlayer();
            DrawWalls();
            DrawBoxes();
            DrawTargets();

            Raylib.EndDrawing();
        }

        Cleanup();
    }

    /*********************************************************************
    * Method name: DrawGrid
    *
    * Description of the Method: Draws a grid on the screen to represent 
    * the game board, using a specified color.
    *
    * Calling arguments: Color color - The color to use for the grid lines.
    *
    * Return value: void
    *
    * Required Files: None
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void DrawGrid(Color color) {
        // Draw vertical grid lines
        for (int i = 0; i < screenWidth / TILE_SIZE + 1; i++) {
            Raylib.DrawLineV(new Vector2(TILE_SIZE * i, 0), new Vector2(TILE_SIZE * i, screenHeight), color);
        }

        // Draw horizontal grid lines
        for (int i = 0; i < screenHeight / TILE_SIZE + 1; i++) {
            Raylib.DrawLineV(new Vector2(0, TILE_SIZE * i), new Vector2(screenWidth, TILE_SIZE * i), color);
        }
    }

    /*********************************************************************
    * Method name: DrawPlayer
    *
    * Description of the Method: Draws the player on the game board based 
    * on the player's current position.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Requires the player texture loaded in Initialize().
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void DrawPlayer() {
        (int x, int y) player = state.Player;
        Raylib.DrawTexture(playerTexture, player.Item2 * TILE_SIZE + offset, player.Item1 * TILE_SIZE + offset, Color.White);
    }

    /*********************************************************************
    * Method name: DrawWalls
    *
    * Description of the Method: Draws the walls of the game board at the 
    * specified positions.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Requires the wall texture loaded in Initialize().
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void DrawWalls() {
        foreach (var wall in level.Walls) {
            int x = wall[1];
            int y = wall[0];
            Raylib.DrawTexture(wallTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    /*********************************************************************
    * Method name: DrawBoxes
    *
    * Description of the Method: Draws the boxes on the game board based 
    * on their current positions.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Requires the box texture loaded in Initialize().
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void DrawBoxes() {
        foreach (var box in state.Boxes) {
            int x = box[1];
            int y = box[0];
            Raylib.DrawTexture(boxTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    /*********************************************************************
    * Method name: DrawTargets
    *
    * Description of the Method: Draws the target locations on the game 
    * board at the specified positions.
    *
    * Calling arguments: None
    *
    * Return value: void
    *
    * Required Files: Requires the target texture loaded in Initialize().
    *
    * List of Checked Exceptions and an indication of when each exception
    * is thrown: None
    *
    *********************************************************************/

    private void DrawTargets() {
        foreach (var target in level.Targets) {
            int x = target[1];
            int y = target[0];
            Raylib.DrawTexture(targetTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    /*********************************************************************
     * Method name: Cleanup
     *
     * Description of the Method: Releases resources by unloading textures 
     * and closing the game window.
     *
     * Calling arguments: None
     *
     * Return value: void
     *
     * Required Files: None
     *
     * List of Checked Exceptions and an indication of when each exception
     * is thrown: None
     *
     *********************************************************************/

    private void Cleanup() {
        // Unload textures and close window
        Raylib.UnloadTexture(playerTexture);
        Raylib.UnloadTexture(wallTexture);
        Raylib.UnloadTexture(boxTexture);
        Raylib.UnloadTexture(targetTexture);
        Raylib.CloseWindow();
    }
}
