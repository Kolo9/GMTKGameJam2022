using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GardenTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CreateGardenFillsPlots() {
        Garden garden = new Garden();
        Assert.AreEqual(garden.plots.Length, Garden.HEIGHT);
        Assert.AreEqual(garden.plots[0].Length, Garden.WIDTH);

        for(int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                Assert.IsTrue(Plot.MIN_NEW_PLOT_VALUE <= garden[i][j].Value && garden[i][j].Value <= Plot.MAX_NEW_PLOT_VALUE);
            }
        }
    }
}
