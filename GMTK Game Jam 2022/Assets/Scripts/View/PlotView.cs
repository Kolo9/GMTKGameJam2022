using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotView : MonoBehaviour {
    private static Color32 NORMAL_COLOR = new Color32(31, 156, 30, 212);
    private static Color32 MODIFIED_COLOR = new Color32(221, 130, 49, 212);

    private Plot plot;
    public int row { get; private set; } = -1;
    public int col { get; private set; } = -1;
    private GardenView gardenView;
    private TextMeshPro text;

    public void Init(GardenView gardenView, int row, int col) {
        this.gardenView = gardenView;
        this.row = row;
        this.col = col;

        this.text = GetComponentInChildren<TextMeshPro>();
        this.plot = gardenView.garden[row][col];
        ResetColor();
        plot.OnModified += OnModified;
    }

    // Update is called once per frame
    void Update() {
        if (plot == null) {
            return;
        }
        text.text = plot.Value.ToString();
    }

    private void OnModified() {
        text.color = MODIFIED_COLOR;
    }

    public void ResetColor() {
        text.color = NORMAL_COLOR;
    }
}
