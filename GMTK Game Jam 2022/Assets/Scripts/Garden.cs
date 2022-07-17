using System.Collections.Generic;
public class Garden {
    public const int WIDTH = 5;
    public const int HEIGHT = 5;

    public Plot[][] plots { get; }

    public Garden() {
        plots = new Plot[HEIGHT][];
        for (int i = 0; i < HEIGHT; i++) {
            plots[i] = new Plot[WIDTH];
            for (int j = 0; j < WIDTH; j++) {
                plots[i][j] = new Plot();
            }
        }
    }

    public Plot[] this[int key] {
        get => plots[key];
    }

    /// <summary>
    /// Sprouts the garden, looking for rows and columns of 3+ matching numbers.
    /// </summary>
    /// <returns>The added score (including chain reactions)</returns>
    public int sprout() {
        int score = 0;
        HashSet<Plot> sproutedPlots = new HashSet<Plot>();
        for (int i = 0; i < HEIGHT; i++) {
            score += sprout(plots[i], sproutedPlots);
        }
        for (int j = 0; j < WIDTH; j++) {
            Plot[] col = new Plot[HEIGHT];
            for (int i = 0; i < HEIGHT; i++) {
                col[j] = plots[i][j];
            }
            score += sprout(col, sproutedPlots);
        }

        if (sproutedPlots.Count > 0) {
            foreach (Plot plot in sproutedPlots) {
                plot.sprout();
            }
            score += sprout();
        }
        return score;
    }

    private static int sprout(Plot[] plots, HashSet<Plot> sproutedPlots) {
        int score = 0;
        List<Plot> matchingPlots = new List<Plot>();
        for (int start = 0; start < plots.Length; start++) {
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
