using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Garden {
    public const int WIDTH = 5;
    public const int HEIGHT = 5;

    public Plot[][] plots { get; private set; }
    public int score { get; private set; }
    public int turn { get; private set; }
    private readonly IRandom rng;

    public Garden(IRandom rng, bool skipSproutForTest = false) {
        plots = new Plot[HEIGHT][];
        for (int i = 0; i < HEIGHT; i++) {
            plots[i] = new Plot[WIDTH];
            for (int j = 0; j < WIDTH; j++) {
                plots[i][j] = new Plot(rng);
            }
        }

        this.rng = rng;
        Reset(skipSproutForTest);
    }

    public void Reset(bool skipSproutForTest = false) {
        for (int i = 0; i < HEIGHT; i++) {
            for (int j = 0; j < WIDTH; j++) {
                plots[i][j].Reset();
            }
        }
        if (!skipSproutForTest) {
            sprout();
        }
        score = 0;
        turn = 1;
    }

    public Plot[] this[int key] {
        get => plots[key];
    }

    /// <summary>
    /// Applies a modifier piece.
    /// For lines, expects the middle cell.
    /// For corners, expects the corner cell.
    /// </summary>
    public bool modify(Modifier modifier, int row, int col) {
        List<Plot> plotsToModify = new List<Plot>();
        try {
            switch (modifier.shape) {
                case Modifier.Shape.HORIZONTAL:
                    plotsToModify.Add(plots[row][col - 1]);
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row][col + 1]);
                    break;
                case Modifier.Shape.VERTICAL:
                    plotsToModify.Add(plots[row - 1][col]);
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row + 1][col]);
                    break;
                case Modifier.Shape.TOP_LEFT:
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row + 1][col]);
                    plotsToModify.Add(plots[row][col + 1]);
                    break;
                case Modifier.Shape.TOP_RIGHT:
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row][col - 1]);
                    plotsToModify.Add(plots[row + 1][col]);
                    break;
                case Modifier.Shape.BOTTOM_LEFT:
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row - 1][col]);
                    plotsToModify.Add(plots[row][col + 1]);
                    break;
                case Modifier.Shape.BOTTOM_RIGHT:
                    plotsToModify.Add(plots[row][col]);
                    plotsToModify.Add(plots[row - 1][col]);
                    plotsToModify.Add(plots[row][col - 1]);
                    break;
                default:
                    throw new Exception("Unhandled shape: " + modifier.shape);
            }
        } catch (IndexOutOfRangeException) {
            Debug.Log(String.Format("Invalid modifier position passed: {0}, {1}, {2} -- ", modifier, row, col));
            return false;
        }

        foreach (Plot plot in plotsToModify) {
            plot.modify(modifier);
        }
        return true;
    }

    /// <summary>
    /// Sprouts the garden repeatedly until there is nothing else to sprout.
    /// </summary>
    /// <returns>Whether anything was sprouted.</returns>
    public bool sprout() {
        bool sprouted = false;
        while (sproutOnce()) {
            sprouted = true;
        };
        turn++;
        return sprouted;
    }

    /// <summary>
    /// Sprouts the garden once, looking for rows and columns of 3+ matching numbers.
    /// </summary>
    /// <returns>Whether anything was sprouted.</returns>
    public bool sproutOnce() {
        HashSet<Plot> sproutedPlots = new HashSet<Plot>();
        for (int i = 0; i < HEIGHT; i++) {
            score += sproutSection(plots[i], sproutedPlots);
        }
        for (int j = 0; j < WIDTH; j++) {
            Plot[] col = new Plot[HEIGHT];
            for (int i = 0; i < HEIGHT; i++) {
                col[i] = plots[i][j];
            }
            score += sproutSection(col, sproutedPlots);
        }

        foreach (Plot plot in sproutedPlots) {
            plot.sprout();
        }

        return sproutedPlots.Count > 0;
    }

    private static int sproutSection(Plot[] plots, HashSet<Plot> sproutedPlots) {
        int score = 0;
        List<Plot> matchingPlots = new List<Plot>();
        for (int start = 0; start < plots.Length;) {
            matchingPlots.Clear();
            matchingPlots.Add(plots[start]);

            int plotValue = plots[start].Value;
            int end = start + 1;
            while(end < plots.Length && plots[end].Value == plotValue) {
                matchingPlots.Add(plots[end]);
                end++;
            }
            if (matchingPlots.Count >= 3) {
                score += (matchingPlots.Count - 2) * plotValue;
                sproutedPlots.UnionWith(matchingPlots);
            }
            start = end;
        }
        return score;
    }
}
