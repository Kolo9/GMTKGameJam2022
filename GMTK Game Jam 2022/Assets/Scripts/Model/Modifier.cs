using System;

public class Modifier {
    public enum Shape {
        HORIZONTAL,
        VERTICAL,
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }
    public enum Value {
        MINUS_ONE,
        PLUS_ONE,
        PLUS_TWO,
        PLUS_THREE
    }

    public readonly Shape shape;
    public readonly int value;

    public Modifier(Shape shape, Value value) {
        this.shape = shape;
        switch(value) {
            case Value.MINUS_ONE:
                this.value = -1;
                break;
            case Value.PLUS_ONE:
                this.value = 1;
                break;
            case Value.PLUS_TWO:
                this.value = 2;
                break;
            case Value.PLUS_THREE:
                this.value = 3;
                break;
            default:
                throw new Exception("Unhandled modifier value: " + value);
        }
    }

    public override string ToString() {
        return shape + " " + value;
    }
}
