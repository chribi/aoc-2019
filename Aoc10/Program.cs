using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

int SolvePart1(IEnumerable<string> input) {
    var asteroids = ReadInput(input);
    var best = GetBestAsteroid(asteroids);
    return best.Count;
}

long SolvePart2(IEnumerable<string> input) {
    var asteroids = ReadInput(input);
    var best = GetBestAsteroid(asteroids);
    var target200 = Shoot(best.Asteroid, asteroids, 200);
    return target200.Col * 100 + target200.Row;
}

Point2D Shoot(Point2D from, List<Point2D> asteroids, int shots) {
    var laserAngle = Math.Tau / 4 + 2e-6;
    var target = new Point2D(0, 0);
    for (var shot = 1; shot <= shots; shot++) {
        (target, var angle) = GetNextLaserTarget(laserAngle, from, asteroids);
        asteroids.Remove(target);
        laserAngle = angle;
        // Log("Shot", shot, target);
    }
    return target;
}

(Point2D Target, double Angle) GetNextLaserTarget(double currentAngle,
        Point2D from, List<Point2D> asteroids) {
    var currentBest = from;
    var currentBestAngle = Math.Tau;
    var currentBestDeltaAngle = Math.Tau;
    foreach (var target in asteroids) {
        if (target == from) continue;
        var angle = Angle(from, target);
        var delta = DeltaAngle(angle, currentAngle);
        if (1e-6 < delta && delta < currentBestDeltaAngle) {
            if (IsVisible(from, target, asteroids)) {
                currentBest = target;
                currentBestAngle = angle;
                currentBestDeltaAngle = delta;
            }
        }
    }

    return (currentBest, currentBestAngle);
}

double Angle(Point2D origin, Point2D p) {
    var d = p - origin;
    return Math.Atan2(-d.Row, d.Col);
}

// Angle from alpha to beta, range [0, tau)
double DeltaAngle(double alpha, double beta) {
    var result = beta - alpha;
    if (result < 0) result += Math.Tau;
    if (result > Math.Tau) result -= Math.Tau;
    return result;
}

List<Point2D> ReadInput(IEnumerable<string> input) {
    var map = new Map2D(input.ToList());
    var asteroids = GetAsteroids(map);
    return asteroids;
}

(Point2D Asteroid, int Count) GetBestAsteroid(List<Point2D> asteroids) {
    return asteroids.Select(a => (a, CountVisible(a, asteroids)))
            .MaxBy(p => p.Item2);
}


List<Point2D> GetAsteroids(Map2D map) {
    return map.Positions()
        .Where(pos => map[pos.Row, pos.Col] == '#')
        .Select(pos => new Point2D(pos.Row, pos.Col))
        .ToList();
}

int CountVisible(Point2D asteroid, List<Point2D> asteroids) {
    return asteroids.Count(a => IsVisible(asteroid, a, asteroids));
}

bool IsVisible(Point2D asteroid1, Point2D asteroid2, List<Point2D> asteroids) {
    if (asteroid1 == asteroid2) return false;
    return asteroids.All(other => !BlocksView(asteroid1, asteroid2, other));
}

bool BlocksView(Point2D from, Point2D to, Point2D other) {
    if (other == from || other == to) return false;
    var vTo = to - from;
    var vOther = other - from;
    return 0 < vOther.DistOrigin() && vOther.DistOrigin() < vTo.DistOrigin()
        && IsPositiveMult(vTo, vOther);
}

bool IsPositiveMult(Point2D a, Point2D b) {
    return a.Row * b.Col == a.Col * b.Row
        && Math.Sign(a.Row) == Math.Sign(b.Row)
        && Math.Sign(a.Col) == Math.Sign(b.Col);
}

if (args.Length == 0) {
    EnableLogging = true;
    AssertEqual(SolvePart1(Utils.ReadLines("../input/sample.10_0.txt")), 8, "sample0");
    AssertEqual(SolvePart1(Utils.ReadLines("../input/sample.10_1.txt")), 33, "sample1");
    AssertEqual(SolvePart1(Utils.ReadLines("../input/sample.10_2.txt")), 35, "sample2");
    AssertEqual(SolvePart1(Utils.ReadLines("../input/sample.10_3.txt")), 41, "sample3");
    AssertEqual(SolvePart1(Utils.ReadLines("../input/sample.10_4.txt")), 210, "sample4");

    AssertEqual(Angle(new(0, 0), new(0, 2)), 0);
    AssertEqual(Angle(new(0, 0), new(2, 0)), - Math.Tau / 4);
    AssertEqual(Angle(new(0, 0), new(-2, 0)), Math.Tau / 4);
    AssertEqual(Angle(new(0, 0), new(0, -2)), Math.Tau / 2);
    AssertEqual(Angle(new(0, 0), new(-2, 2)), Math.Tau / 8);

    AssertEqual(SolvePart2(Utils.ReadLines("../input/sample.10_4.txt")), 802, "sample4");
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

