using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Garden {
    public const int WIDTH = 5;
    public const int HEIGHT = 5;

    public Plot[][] plots { get; private set; }

    public Garden(IRandom rng) {
        plots = new Plot[HEIGHT][];
        for (int i = 0; i < HEIGHT; i++) {
            plots[i] = new Plot[WIDTH];
            for (int j = 0; j < WIDTH; j++) {
                plots[i][j] = new Plot(rng);
            }
        }
    }

    public Plot[] this[int key] {
        get => plots[key];
    }

    public void modify(Modifier modifier, int topLeftRow, int topLeftColumn) {
        List<Plot> plotsToModify = new List<Plot>();
        try {
            switch (modifier.shape) {
                case Modifier.Shape.HORIZONTAL:
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn + 1]);
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn + 2]);
                    break;
                case Modifier.Shape.VERTICAL:
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow + 1][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow + 2][topLeftColumn]);
                    break;
                case Modifier.Shape.TOP_LEFT:
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow + 1][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn + 1]);
                    break;
                case Modifier.Shape.TOP_RIGHT:
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn + 1]);
                    plotsToModify.Add(plots[topLeftRow][topLeftColumn + 2]);
                    plotsToModify.Add(plots[topLeftRow + 1][topLeftColumn + 2]);
                    break;
                case Modifier.Shape.BOTTOM_LEFT:
                    plotsToModify.Add(plots[topLeftRow + 1][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow + 2][topLeftColumn]);
                    plotsToModify.Add(plots[topLeftRow + 2][topLeftColumn + 1]);
                    break;
                case Modifier.Shape.BOTTOM_RIGHT:
                    plotsToModify.Add(plots[topLeftRow + 1][topLeftColumn + 2]);
                    plotsToModify.Add(plots[topLeftRow + 2][topLeftColumn + 1]);
                    plotsToModify.Add(plots[topLeftRow + 2][topLeftColumn + 2]);
                    break;
                default:
                    throw new Exception("Unhandled shape: " + modifier.shape);
            }
        } catch (IndexOutOfRangeException e) {
            throw new Exception(String.Format("Invalid modifier position passed: %s, %d, %d -- ", modifier, topLeftRow, topLeftColumn), e);
        }

        foreach (Plot plot in plotsToModify) {
            plot.modify(modifier);
        }
    }

    /// <summary>
    /// Sprouts the garden until there is nothing else to sprout.
    /// </summary>
    /// <returns>The added score (including chain reactions)</returns>
    public int sprout() {
        int score = 0;

        // Remove.
        int[] plotValues = new int[Garden.WIDTH * Garden.HEIGHT];
        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                plotValues[i * Garden.HEIGHT + j] = plots[i][j].Value;
            }
        }

        Tuple<int, bool> sproutResult = sproutOnce();
        score += sproutResult.Item1;

        if (sproutResult.Item2) {
            score += sprout();
        }
        return score;
    }

    /// <summary>
    /// Sprouts the garden once, looking for rows and columns of 3+ matching numbers.
    /// </summary>
    /// <returns>The added score and whether anything was sprouted.</returns>
    public Tuple<int, bool> sproutOnce() {
        int score = 0;
        HashSet<Plot> sproutedPlots = new HashSet<Plot>();
        for (int i = 0; i < HEIGHT; i++) {
            score += sprout(plots[i], sproutedPlots);
        }
        for (int j = 0; j < WIDTH; j++) {
            Plot[] col = new Plot[HEIGHT];
            for (int i = 0; i < HEIGHT; i++) {
                col[i] = plots[i][j];
            }
            score += sprout(col, sproutedPlots);
        }

        foreach (Plot plot in sproutedPlots) {
            plot.sprout();
        }

        return new Tuple<int, bool>(score, sproutedPlots.Count > 0);
    }

    private static int sprout(Plot[] plots, HashSet<Plot> sproutedPlots) {
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
