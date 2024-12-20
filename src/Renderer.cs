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

    // Initializes the game window, sets up the frame rate, and loads the necessary textures for the game elements
    private void Initialize() {

        Raylib.SetTraceLogLevel(TraceLogLevel.None);                 // disable Raylib log info
        Raylib.InitWindow(screenWidth, screenHeight, "Suckabunch");  // window initilizer
        Raylib.SetTargetFPS(30);                                     // set frame rate

        playerTexture = Raylib.LoadTexture("resources/player.png");   // load player texture
        wallTexture = Raylib.LoadTexture("resources/wall.png");       // load wall texture
        boxTexture = Raylib.LoadTexture("resources/box.png");         // load box texture
        targetTexture = Raylib.LoadTexture("resources/target.png");   // load target texture
    }

    // Manages the game loop, including drawing the game elements on the screen and handling window events
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

    // Draws a grid on the screen to represent the game board, using a specified color
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

    // Draws the player on the game board based on the player's current position
    private void DrawPlayer() {
        (int x, int y) player = state.Player;
        Raylib.DrawTexture(playerTexture, player.Item2 * TILE_SIZE + offset, player.Item1 * TILE_SIZE + offset, Color.White);
    }

    // Draws the walls of the game board at the specified positions
    private void DrawWalls() {
        foreach (var wall in level.Walls) {
            int x = wall[1];
            int y = wall[0];
            Raylib.DrawTexture(wallTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    // Draws the boxes on the game board based on their current positions
    private void DrawBoxes() {
        foreach (var box in state.Boxes) {
            int x = box[1];
            int y = box[0];
            Raylib.DrawTexture(boxTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    // Draws the target locations on the game board at the specified positions.
    private void DrawTargets() {
        foreach (var target in level.Targets) {
            int x = target[1];
            int y = target[0];
            Raylib.DrawTexture(targetTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
        }
    }

    // Releases resources by unloading textures and closing the game window.
    private void Cleanup() {
        // Unload textures and close window
        Raylib.UnloadTexture(playerTexture);
        Raylib.UnloadTexture(wallTexture);
        Raylib.UnloadTexture(boxTexture);
        Raylib.UnloadTexture(targetTexture);
        Raylib.CloseWindow();
    }
}
