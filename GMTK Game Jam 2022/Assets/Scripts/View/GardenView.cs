using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GardenView : MonoBehaviour {
    private const float PLOT_SPACING = 2.5f;
    private const int MODIFIERS_PER_TURN = 3;

    private static IRandom rng = new Random();

    [SerializeField]
    private GameObject plotPrefab;
    [SerializeField]
    private GameObject lineModifierPrefab;
    [SerializeField]
    private GameObject cornerModifierPrefab;
    [SerializeField]
    private TextMeshPro gameText;

    public int remainingModifiers { get; set; }
    public readonly Garden garden = new Garden(new Random());
    private PlotView[] plotViews;

    void Start() {
        plotViews = new PlotView[Garden.WIDTH * Garden.HEIGHT];

        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                GameObject plotObj = Instantiate(plotPrefab, new Vector3(PLOT_SPACING * j, 0, 10 - PLOT_SPACING * i), Quaternion.identity);
                plotObj.GetComponent<PlotView>().Init(this, i, j);
                plotObj.GetComponent<BoxCollider>().size = new Vector3(PLOT_SPACING, PLOT_SPACING, PLOT_SPACING);
                plotViews[i * Garden.HEIGHT + j] = plotObj.GetComponent<PlotView>();
                plotObj.transform.SetParent(transform);
            }
        }

        remainingModifiers = MODIFIERS_PER_TURN;
        GenerateModifiers();
        UpdateGameText();
    }

    void Update() {
        if (remainingModifiers == 0) {
            GenerateModifiers();
            if (!garden.sprout()) {
                // Game over.
                garden.Reset();
            }
            ResetPlots();
            UpdateGameText();
        }
    }

    private void GenerateModifiers() {
        var shapeOptions = Enum.GetValues(typeof(Modifier.Shape));
        var valueOptions = Enum.GetValues(typeof(Modifier.Value));
        for (int i = 0; i < MODIFIERS_PER_TURN; i++) {
            Modifier.Shape shape = (Modifier.Shape)shapeOptions.GetValue(rng.NextInclusive(0, shapeOptions.Length - 1));
            Modifier.Value value = (Modifier.Value)valueOptions.GetValue(rng.NextInclusive(0, valueOptions.Length - 1));

            GameObject prefab = shape == Modifier.Shape.HORIZONTAL || shape == Modifier.Shape.VERTICAL ? lineModifierPrefab : cornerModifierPrefab;
            GameObject modifierObj = Instantiate(prefab, new Vector3(16f, 1.5f, 1.6f + 5.4f * i), prefab.transform.rotation);
            modifierObj.GetComponent<ModifierView>().Init(this, new Modifier(shape, value));
            modifierObj.transform.SetParent(transform);
        }
        remainingModifiers = 3;
    }

    private void ResetPlots() {
        foreach (PlotView plotView in plotViews) {
            plotView.ResetColor();
        }
    }

    private void UpdateGameText() {
        gameText.text = String.Format("Score: {0}\nTurn: {1}", garden.score, garden.turn);
    }
}
