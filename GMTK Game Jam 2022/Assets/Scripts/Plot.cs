using System;

public class Plot {
    public const int MIN_NEW_PLOT_VALUE = 1;
    public const int MAX_NEW_PLOT_VALUE = 6;

    private readonly IRandom rng;
    private int _value;
    public int Value {
        get => _value;
        set => _value = value;
    }

    public Plot(IRandom rng) {
        this.rng = rng;
        _value = getNewValue();
    }

    public void sprout() {
        _value = getNewValue();
    }

    private int getNewValue() {
        return rng.NextInclusive(MIN_NEW_PLOT_VALUE, MAX_NEW_PLOT_VALUE);
    }
}
