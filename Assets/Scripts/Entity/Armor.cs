﻿
public class Armor {
    private const float C = 1.25f;
    private string name { get; set; }
    public string Name {
        get {
            return name;
        }
    }
    private EquipType type { get; set; }
    public EquipType Type {
        get {
            return type;
        }
    }
    private float defence { get; set; }
    public float Defence {
        get {
            return defence * (level * C);
        }
    }
    private float vsStrike { get; set; }
    public float VsStrike {
        get {
            return defence * C / 2;
        }
    }
    private float vsSlash { get; set; }
    public float VsSlash {
        get {
            return defence * C / 2;
        }
    }
    private float vsTrust { get; set; }
    public float VsTrust {
        get {
            return defence * C / 2;
        }
    }
    private int level { get; set; }
    public int Level {
        get {
            return level;
        }
        set {
            level = value;
        }
    }
    public Armor(string name, EquipType type, float defence, int level) {
        this.name = name;
        this.type = type;
        this.defence = defence;
        this.level = level;
    }
}

