using System;
using System.Collections.Generic;

namespace Sokoban;

public class Param {
    private string task;
    private string firstFlag;
    private string level;
    private string? secondFlag;
    private string? strategy;
    private string? thirdFlag;
    private string? depth;
    private string? renderFlag;

    // Param constructor
    public Param(string[] args) {
        task = args[0];         // store task argument
        firstFlag = args[1];    // store first parameter argument
        level = args[2];        // store level argument

        // Set optional parameters if provided or assign default values
        secondFlag = args.Length > 3 ? args[3] : null;  // secondFlag is optional
        strategy = args.Length > 4 ? args[4] : null;    // strategy is optional
        thirdFlag = args.Length > 5 ? args[5] : null;   // thirdFlag is optional
        depth = args.Length > 6 ? args[6] : null;       // depth is optional
        renderFlag = args.Length > 7 ? args[7] : null;  // renderFlag is optional

        // Parse newlines properly in level string
        level = level.Replace("\\n", "\n");
    }

    // Properties
    public string Task => task;                 // Getter task
    public string FirstFlag => firstFlag;       // Getter firstFlag
    public string Level => level;               // Getter level
    public string? SecondFlag => secondFlag;    // Getter secondFlag
    public string? Strategy => strategy;        // Getter strategy
    public string? ThirdFlag => thirdFlag;      // Getter thirdFlag
    public string? Depth => depth;              // Getter depth
    public string? RenderFlag => renderFlag;    // Getter renderFlag
}
