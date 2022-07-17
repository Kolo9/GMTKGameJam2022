using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    [SerializeField]
    private GameObject harvestCompletePanel;
    [SerializeField]
    private GameObject instructionsPanel;
    [SerializeField]
    private TextMeshProUGUI harvestCompleteText;
    [SerializeField]
    private ShapeDice shapeDie;
    [SerializeField]
    private ValueDice valueDie;

    [SerializeField]
    private AudioClip modifierPickupSound;
    [SerializeField]
    private AudioClip modifierPlacedSound;
    [SerializeField]
    private AudioClip sproutSound;
    [SerializeField]
    private AudioClip diceRollSound;
    [SerializeField]
    private AudioClip harvestCompleteSound;

    private AudioSource audioSource;

    private int _remainingModifiers;
    public int RemainingModifiers {
        get { return _remainingModifiers;  }
        set {
            if (value < _remainingModifiers) {
                audioSource.PlayOneShot(modifierPlacedSound);
            }
            _remainingModifiers = value;
        }
    }
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
        audioSource = GetComponent<AudioSource>();

        _remainingModifiers = MODIFIERS_PER_TURN;
    }

    void Update() {
        if (_remainingModifiers == 0) {
            _remainingModifiers = 3;
            StartCoroutine(NextTurn());
        }
    }

    private IEnumerator NextTurn() {
        ResetPlots();
        UpdateGameText();
        if (garden.sprout()) {
            audioSource.PlayOneShot(sproutSound);
            yield return GenerateModifiers();
        } else {
            OnHarvestComplete();
        }
    }

    private IEnumerator GenerateModifiers() {
        var shapeOptions = Enum.GetValues(typeof(Modifier.Shape));
        var valueOptions = Enum.GetValues(typeof(Modifier.Value));
        for (int i = 0; i < MODIFIERS_PER_TURN; i++) {
            //Modifier.Shape shape = (Modifier.Shape)shapeOptions.GetValue(rng.NextInclusive(0, shapeOptions.Length - 1));
            yield return StartCoroutine(RollDice());
            //Modifier.Value value = (Modifier.Value)valueOptions.GetValue(rng.NextInclusive(0, valueOptions.Length - 1));
            Modifier.Value value = (Modifier.Value)Enum.Parse(typeof(Modifier.Value), ValueDiceCheck.result);
            Modifier.Shape shape = (Modifier.Shape)Enum.Parse(typeof(Modifier.Shape), ShapeDiceCheck.result);

            GameObject prefab = shape == Modifier.Shape.HORIZONTAL || shape == Modifier.Shape.VERTICAL ? lineModifierPrefab : cornerModifierPrefab;
            GameObject modifierObj = Instantiate(prefab, new Vector3(16f, 1.5f, 1.6f + 5.4f * i), prefab.transform.rotation);
            modifierObj.GetComponent<ModifierView>().Init(this, new Modifier(shape, value));
            modifierObj.transform.SetParent(transform);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private IEnumerator RollDice() {
        audioSource.PlayOneShot(diceRollSound);
        ShapeDiceCheck.result = null;
        ValueDiceCheck.result = null;
        shapeDie.Roll();
        valueDie.Roll();
        yield return new WaitUntil(() => ShapeDiceCheck.result != null && ValueDiceCheck.result != null);
    }

    private void ResetPlots() {
        foreach (PlotView plotView in plotViews) {
            plotView.ResetColor();
        }
    }

    private void UpdateGameText() {
        gameText.text = String.Format("Score: {0}\nTurn: {1}", garden.score, garden.turn);
    }

    private void OnHarvestComplete() {
        harvestCompleteText.text = String.Format("Harvest complete!\n\nFinal score: {0}\nTurns: {1}", garden.score, garden.turn);
        harvestCompletePanel.SetActive(true);
        audioSource.PlayOneShot(harvestCompleteSound);
    }

    public void OnReplant() {
        instructionsPanel.SetActive(false);
        harvestCompletePanel.SetActive(false);
        garden.Reset();
        StartCoroutine(GenerateModifiers());
        UpdateGameText();
    }

    public void OnModifierPickup() {
        audioSource.PlayOneShot(modifierPickupSound);
    }
}
