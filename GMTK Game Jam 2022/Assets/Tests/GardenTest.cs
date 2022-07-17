using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GardenTest {

    class MockRandom : IRandom {
        private readonly int[] values;
        private int i = 0;

        internal MockRandom(params int[] values) {
            this.values = values;
        }

        public int NextInclusive(int _unused, int _unused2) {
            int val = values[i];
            if (i < values.Length - 1) {
                i++;
            }
            return val;
        }
    }

    [Test]
    public void CreateGardenFillsPlots() {
        Garden garden = new Garden(new Random());
        Assert.AreEqual(garden.plots.Length, Garden.HEIGHT);
        Assert.AreEqual(garden.plots[0].Length, Garden.WIDTH);

        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                Assert.GreaterOrEqual(garden[i][j].Value, Plot.MIN_NEW_PLOT_VALUE);
                Assert.LessOrEqual(garden[i][j].Value, Plot.MAX_NEW_PLOT_VALUE);
            }
        }
    }

    [Test]
    public void SproutAllRows() {
        Garden garden = new Garden(new MockRandom(
            1, 1, 1, 1, 1,
            2, 2, 2, 2, 2,
            3, 3, 3, 3, 3,
            4, 4, 4, 4, 4,
            5, 5, 5, 5, 5,
            // Always -1 after popping
            -1));

        garden.sproutOnce();

        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                Assert.AreEqual(garden[i][j].Value, -1);
            }
        }
    }

    [Test]
    public void SproutAllColumns() {
        Garden garden = new Garden(new MockRandom(
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            // Always -1 after popping
            -1));

        garden.sproutOnce();

        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                Assert.AreEqual(garden[i][j].Value, -1);
            }
        }
    }

    [Test]
    public void SproutUpperLeft3By3() {
        Garden garden = new Garden(new MockRandom(
            1, 1, 1, 3, 4,
            1, 2, 3, 4, 5,
            1, 3, 4, 5, 6,
            2, 4, 5, 6, 1,
            3, 5, 6, 1, 2,
            // Always -1 after popping
            -1));

        garden.sproutOnce();

        Assert.AreEqual(getPlotValues(garden), new int[]{
            -1, -1, -1, 3, 4,
            -1,  2,  3, 4, 5,
            -1,  3,  4, 5, 6,
             2,  4,  5, 6, 1,
             3,  5,  6, 1, 2});
    }

    [Test]
    public void SproutLowerRight3By3() {
        Garden garden = new Garden(new MockRandom(
            6, 1, 2, 3, 4,
            1, 2, 3, 4, 5,
            1, 3, 4, 5, 1,
            2, 4, 5, 6, 1,
            3, 5, 1, 1, 1,
            // Always -1 after popping
            -1));

        garden.sproutOnce();

        Assert.AreEqual(getPlotValues(garden), new int[]{
            6, 1,  2,  3,  4,
            1, 2,  3,  4,  5,
            1, 3,  4,  5, -1,
            2, 4,  5,  6, -1,
            3, 5, -1, -1, -1});
    }

    [Test]
    public void SproutChainReaction() {
        Garden garden = new Garden(new MockRandom(
            // Entire board clears
            1, 1, 1, 1, 1,
            2, 2, 2, 2, 2,
            3, 3, 3, 3, 3,
            4, 4, 4, 4, 4,
            5, 5, 5, 5, 5,
            // Then a row of three 2s and a column of four 3s.
            1, 2, 2, 2, 4,
            2, 3, 4, 5, 3,
            3, 4, 5, 6, 3,
            4, 5, 6, 1, 3,
            5, 6, 1, 2, 3,
            // Replace the three 2s
            2, 3, 4,
            // Replace the four 3s
            5, 6, 1, 2));

        garden.sprout();

        Assert.AreEqual(getPlotValues(garden), new int[]{
            1, 2, 3, 4, 4,
            2, 3, 4, 5, 5,
            3, 4, 5, 6, 6,
            4, 5, 6, 1, 1,
            5, 6, 1, 2, 2});
    }

    private static int[] getPlotValues(Garden garden) {
        int[] plotValues = new int[Garden.WIDTH * Garden.HEIGHT];
        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                plotValues[i * Garden.HEIGHT + j] = garden[i][j].Value;
            }
        }
        return plotValues;
    }
}
