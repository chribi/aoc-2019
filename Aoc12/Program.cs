using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

int SolvePart1(IEnumerable<string> input) {
    return SolvePart1Steps(input, 1000);
}

long SolvePart2(IEnumerable<string> input) {
    var moons = ReadInput(input);

    var xInitial = moons.Select(m => (m.X, m.VX)).ToArray();
    var yInitial = moons.Select(m => (m.Y, m.VY)).ToArray();
    var zInitial = moons.Select(m => (m.Z, m.VZ)).ToArray();

    var xCycle = -1L;
    var yCycle = -1L;
    var zCycle = -1L;

    var steps = 0;

    while (xCycle < 0 || yCycle < 0 || zCycle < 0) {
        steps++;
        SimulateStep(moons);
        if (xCycle < 0 && Enumerable.Range(0, moons.Count)
                .All(i => moons[i].X == xInitial[i].X
                    && moons[i].VX == xInitial[i].VX)) {
            xCycle = steps;
        }
        if (yCycle < 0 && Enumerable.Range(0, moons.Count)
                .All(i => moons[i].Y == yInitial[i].Y
                    && moons[i].VY == yInitial[i].VY)) {
            yCycle = steps;
        }
        if (zCycle < 0 && Enumerable.Range(0, moons.Count)
                .All(i => moons[i].Z == zInitial[i].Z
                    && moons[i].VZ == zInitial[i].VZ)) {
            zCycle = steps;
        }
    }
    Log(xCycle, yCycle, zCycle);
    return MathUtils.Lcm(new[] { xCycle, yCycle, zCycle });
}


int SolvePart1Steps(IEnumerable<string> input, int steps) {
    var moons = ReadInput(input);
    Simulate(moons, steps);
    return moons.Sum(moon => moon.Energy());
}

List<Moon> ReadInput(IEnumerable<string> input) {
    return input.Select(line => new Moon(line)).ToList();
}

void SimulateStep(List<Moon> moons) {
    foreach (var moon in moons) {
        foreach (var other in moons) {
            if (moon == other) continue;
            moon.Gravity(other);
        }
    }

    foreach (var moon in moons) {
        moon.Update();
    }
}

void Simulate(List<Moon> moons, int steps) {
    for (var step = 1; step <= steps; step++) {
        SimulateStep(moons);
    }
}

if (args.Length == 0) {
    EnableLogging = true;
    AssertEqual(SolvePart1Steps(Utils.ReadLines("../input/sample.12.small.txt"), 10), 179, "sample short");
    AssertEqual(SolvePart1Steps(Utils.ReadLines("../input/sample.12.big.txt"), 100), 1940, "sample long");
    AssertEqual(SolvePart2(Utils.ReadLines("../input/sample.12.small.txt")), 2772, "sample short");
    AssertEqual(SolvePart2(Utils.ReadLines("../input/sample.12.big.txt")), 4686774924, "sample long");
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

public class Moon {
    public Moon(string definition) {
        var p = Input.GetIntNumbers(definition);
        X = p[0];
        Y = p[1];
        Z = p[2];
    }
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public int VX { get; set; }
    public int VY { get; set; }
    public int VZ { get; set; }

    public void Gravity(Moon other) {
        if (X > other.X) VX--;
        if (X < other.X) VX++;
        if (Y > other.Y) VY--;
        if (Y < other.Y) VY++;
        if (Z > other.Z) VZ--;
        if (Z < other.Z) VZ++;
    }

    public void Update() {
        X += VX;
        Y += VY;
        Z += VZ;
    }

    public int Energy() {
        return (Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z))
            * (Math.Abs(VX) + Math.Abs(VY) + Math.Abs(VZ));
    }

    public override string ToString() {
        return $"{X}, {Y}, {Z} | {VX}, {VY}, {VZ}";
    }
}
