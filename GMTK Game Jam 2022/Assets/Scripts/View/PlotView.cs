using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotView : MonoBehaviour {

    internal Plot plot = null;
    private int row = -1;
    private int col;
    private GardenView gardenView;
    private TextMeshPro text;

    public void Init(GardenView gardenView, int row, int col) {
        this.gardenView = gardenView;
        this.row = row;
        this.col = col;

        this.text = GetComponentInChildren<TextMeshPro>();
        this.plot = gardenView.garden[row][col];
    }

    // Update is called once per frame
    void Update() {
        if (plot == null) {
            return;
        }
        text.text = plot.Value.ToString();
    }
}
