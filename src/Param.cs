using System;
using System.Collections.Generic;

namespace Sokoban;

public class Param {
    private string level;
    private string strategy;
    private string depth;

    // Param constructor
    public Param(string[] args) {
        level = args[0];
        strategy = args[1];
        depth = args[2];

        // Parse newlines properly in level string
        level = level.Replace("\\n", "\n");
    }

    // Properties
    public string Level => level;               // Level Getter
    public string Strategy => strategy;        // Strategy Getter
    public string Depth => depth;              // Depth Getter
}
