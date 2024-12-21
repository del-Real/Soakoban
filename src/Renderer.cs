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

    /*

        // Renderer constructor
        public Renderer() {
            screenWidth = 1280;     // screen width set by tile size
            screenHeight = 720;    // screen height set by tile size
            offset = TILE_SIZE * 2;                                 // offset to add margins

            LaunchMenu();
        }

    */

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
        Raylib.SetTargetFPS(60);                                     // set frame rate

        playerTexture = Raylib.LoadTexture("resources/player.png");   // load player texture
        wallTexture = Raylib.LoadTexture("resources/wall.png");       // load wall texture
        boxTexture = Raylib.LoadTexture("resources/box.png");         // load box texture
        targetTexture = Raylib.LoadTexture("resources/target.png");   // load target texture
    }

    // Manages the game loop, including drawing the game elements on the screen and handling window events
    public void Render(List<Node> nodeSolution) {
        Initialize();

        float duration = 0.3f;    // Duration for each movement
        float timeElapsed = 0.0f; // Tracks time for interpolation

        // Player movement
        (float, float) startPlayerPos = (0f, 0f);
        (float, float) endPlayerPos = (0f, 0f);
        (float, float) playerMove = (0f, 0f);

        // Boxes movement
        (float, float) startBoxPos = (0f, 0f);
        (float, float) endBoxrPos = (0f, 0f);
        (float, float) boxMove = (0f, 0f);

        int currentNodeIndex = 0;

        // Render loop
        while (!Raylib.WindowShouldClose()) {
            float dt = Raylib.GetFrameTime(); // Delta time

            // Lerp
            if (currentNodeIndex < nodeSolution.Count - 1) {
                // Set start and end positions for the current node transition
                startPlayerPos = (nodeSolution[currentNodeIndex].StateNode.Player.Item1,
                                  nodeSolution[currentNodeIndex].StateNode.Player.Item2);
                endPlayerPos = (nodeSolution[currentNodeIndex + 1].StateNode.Player.Item1,
                                nodeSolution[currentNodeIndex + 1].StateNode.Player.Item2);

                // Update interpolation progress
                if (timeElapsed < duration) {
                    float t = timeElapsed / duration; // Normalized time (0 to 1)
                    playerMove = Lerp(startPlayerPos, endPlayerPos, t);
                    timeElapsed += dt;
                }
                else {
                    // Move to the next node
                    playerMove = endPlayerPos;
                    timeElapsed = 0.0f; // Reset elapsed time for the next transition
                    currentNodeIndex++;
                }
            }

            // Drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            DrawGrid();
            DrawPlayer(playerMove);
            DrawBoxes();
            DrawTargets();
            DrawWalls();

            Raylib.EndDrawing();
        }

        Cleanup();
    }

    // Lerp from start to end
    private static (float, float) Lerp((float, float) startPos, (float, float) endPos, float amount) {
        float x = startPos.Item1 + (endPos.Item1 - startPos.Item1) * amount;
        float y = startPos.Item2 + (endPos.Item2 - startPos.Item2) * amount;
        return (x, y);
    }

    /*
        public void LaunchMenu() {
            Initialize();

            Color lightWhite = new Color(255, 255, 255, 32); // Set grid color

            while (!Raylib.WindowShouldClose()) {

                float deltaTime = Raylib.GetFrameTime();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.EndDrawing();
            }

            Cleanup();
        }
    */

    // Draws a grid on the screen to represent the game board, using a specified color
    private void DrawGrid() {

        Color lightWhite = new Color(255, 255, 255, 32);

        // Draw vertical grid lines
        for (int i = 0; i < screenWidth / TILE_SIZE + 1; i++) {
            Raylib.DrawLineV(new Vector2(TILE_SIZE * i, 0), new Vector2(TILE_SIZE * i, screenHeight), lightWhite);
        }

        // Draw horizontal grid lines
        for (int i = 0; i < screenHeight / TILE_SIZE + 1; i++) {
            Raylib.DrawLineV(new Vector2(0, TILE_SIZE * i), new Vector2(screenWidth, TILE_SIZE * i), lightWhite);
        }
    }

    // Draws the player on the game board based on the player's current position
    private void DrawPlayer((float, float) playerCoords) {
        // Calculate pixel positions using TILE_SIZE and offset
        int x = (int)(playerCoords.Item2 * TILE_SIZE + offset); // Convert to pixel X
        int y = (int)(playerCoords.Item1 * TILE_SIZE + offset); // Convert to pixel Y

        // Draw the player texture at the calculated position
        Raylib.DrawTexture(playerTexture, x, y, Color.White);
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

    // Draws the walls of the game board at the specified positions
    private void DrawWalls() {
        foreach (var wall in level.Walls) {
            int x = wall[1];
            int y = wall[0];
            Raylib.DrawTexture(wallTexture, x * TILE_SIZE + offset, y * TILE_SIZE + offset, Color.White);
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
