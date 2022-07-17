using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenView : MonoBehaviour {
    [SerializeField]
    private GameObject plotPrefab;
    [SerializeField]
    private GameObject lineModifierPrefab;
    [SerializeField]
    private GameObject cornerModifierPrefab;

    public readonly Garden garden = new Garden(new Random());
    private PlotView[] plotViews;

    // Start is called before the first frame update
    void Start() {
        plotViews = new PlotView[Garden.WIDTH * Garden.HEIGHT];

        for (int i = 0; i < Garden.HEIGHT; i++) {
            for (int j = 0; j < Garden.WIDTH; j++) {
                GameObject plotObj = Instantiate(plotPrefab, new Vector3(2.5f * j, 0, 10 - 2.5f * i), Quaternion.identity);
                plotObj.GetComponent<PlotView>().Init(this, i, j);
                plotViews[i * Garden.HEIGHT + j] = plotObj.GetComponent<PlotView>();
                plotObj.transform.SetParent(transform);
            }
        }

        GameObject lineModifierObj = Instantiate(lineModifierPrefab, new Vector3(5, 1.5f, 5), lineModifierPrefab.transform.rotation);
        lineModifierObj.transform.SetParent(transform);
        GameObject cornerModifierObj = Instantiate(cornerModifierPrefab, new Vector3(2, 1.5f, 2), cornerModifierPrefab.transform.rotation);
        cornerModifierObj.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update() {

    }
}
