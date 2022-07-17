using System;
using UnityEngine;

public class Plot {
    public const int MIN_NEW_PLOT_VALUE = 1;
    public const int MAX_NEW_PLOT_VALUE = 6;

    private readonly IRandom rng;
    private int _value;

    public delegate void OnModifiedDelegate();
    public event OnModifiedDelegate OnModified;
    public int Value {
        get => _value;
        set => _value = value;
    }

    public Plot(IRandom rng) {
        this.rng = rng;
        getNewValue();
    }

    public void modify(Modifier modifier) {
        _value = Math.Max(0, _value + modifier.value);
        if (OnModified != null) {
            OnModified();
        }
    }

    public void sprout() {
        getNewValue();
    }
    
    public void Reset() {
        getNewValue();
    }

    private void getNewValue() {
        _value = rng.NextInclusive(MIN_NEW_PLOT_VALUE, MAX_NEW_PLOT_VALUE);
    }
}
