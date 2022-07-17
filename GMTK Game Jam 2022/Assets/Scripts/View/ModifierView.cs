using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModifierView : MonoBehaviour {
    // Plots are on layer 7.
    private static int PLOT_LAYER_MASK = 1 << 7;

    private GardenView gardenView;
    private Modifier modifier;
    private Vector3 startPos;

    public void Init(GardenView gardenView, Modifier modifier) {
        this.gardenView = gardenView;
        this.modifier = modifier;
        TextMeshPro textObj = GetComponentInChildren<TextMeshPro>();
        switch (modifier.shape) {
            case Modifier.Shape.HORIZONTAL:
                transform.Rotate(new Vector3(0, 0, 90));
                textObj.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case Modifier.Shape.TOP_LEFT:
                transform.position += new Vector3(-transform.localScale.x, 0, transform.localScale.y);
                textObj.rectTransform.localPosition += new Vector3(0, .3f, 0);
                break;
            case Modifier.Shape.TOP_RIGHT:
                transform.Rotate(new Vector3(0, 0, 90));
                textObj.transform.Rotate(new Vector3(0, 0, 90));
                textObj.rectTransform.localPosition += new Vector3(.3f, 0, 0);
                transform.position += new Vector3(transform.localScale.x, 0, transform.localScale.y);
                break;
            case Modifier.Shape.BOTTOM_RIGHT:
                transform.Rotate(new Vector3(0, 0, 180));
                textObj.transform.Rotate(new Vector3(0, 0, 180));
                textObj.rectTransform.localPosition += new Vector3(0, -.3f, 0);
                transform.position += new Vector3(transform.localScale.x, 0, -transform.localScale.y);
                break;
            case Modifier.Shape.BOTTOM_LEFT:
                transform.Rotate(new Vector3(0, 0, -90));
                textObj.transform.Rotate(new Vector3(0, 0, -90));
                textObj.rectTransform.localPosition += new Vector3(-.3f, 0, 0);
                transform.position += new Vector3(-transform.localScale.x, 0, -transform.localScale.y);
                break;
        }
        textObj.text = (modifier.value > 0 ? "+" : "") + modifier.value;
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void OnMouseDrag() {
        if (modifier == null) {
            return;
        }

        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Keep the same y value. Only move on the {x, z} plane.
        newPos.y = transform.position.y;
        transform.position = newPos;
    }

    void OnMouseUp() {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, PLOT_LAYER_MASK)) {
            transform.position = startPos;
            return;
        }

        PlotView hitPlotView = hit.transform.gameObject.GetComponent<PlotView>();
        if (gardenView.garden.modify(modifier, hitPlotView.row, hitPlotView.col)) {
            gardenView.remainingModifiers--;
            Destroy(gameObject);
        } else {
            transform.position = startPos;
        }
    }
}
 