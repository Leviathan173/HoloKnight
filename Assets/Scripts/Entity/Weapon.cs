

public class Weapon {
    private float C = 1.25f;
    private int id { get; set; }
    public int Id {
        get {
            return id;
        }
    }
    private string name { get; set; }
    public string Name {
        get {
            return name;
        }
    }
    private float attack { get; set; }
    public float Attack {
        get {
            return attack * (level * C);
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
    public Weapon(string name, float attack, int level) {
        this.name = name;
        this.attack = attack;
        this.level = level;
    }
}

    
