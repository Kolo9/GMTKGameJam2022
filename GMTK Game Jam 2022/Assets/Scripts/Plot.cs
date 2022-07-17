using System;

public class Plot {
    public const int MIN_NEW_PLOT_VALUE = 1;
    public const int MAX_NEW_PLOT_VALUE = 6;
    private static Random rng = new Random();

    private int _value;
    public int Value {
        get => _value;
        set => _value = value;
    }

    public Plot() {
        _value = getNewValue();
    }

    public void sprout() {
        _value = getNewValue();
    }

    private int getNewValue() {
        return rng.Next(MIN_NEW_PLOT_VALUE, MAX_NEW_PLOT_VALUE + 1);
    }
}
