using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

long SolvePart1(IEnumerable<string> input) {
    var paths = input.ToArray();
    var intersection = ClosestIntersection(paths[0], paths[1]);
    Log(intersection);

    return intersection.DistOrigin();
}

long SolvePart2(IEnumerable<string> input) {
    var paths = input.ToArray();
    return ClosestIntersectionViaCircuit(paths[0], paths[1]);
}

Line[] GetSegments(string path) =>
    GetSteps(ReadPath(path))
    .Pairs()
    .Select(pair => new Line(pair.Item1, pair.Item2))
    .ToArray();

// Intersection closest to center (part 1)
Point2D ClosestIntersection(string path0, string path1) {
    var segments0 = GetSegments(path0);
    var segments1 = GetSegments(path1);

    var min = long.MaxValue;
    var best = default(Point2D);
    foreach (var segmentA in segments0) {
        foreach (var segmentB in segments1) {
            var p = Line.IntersectOrtho(segmentA, segmentB);
            if (p.HasValue) {
                var d = p.Value.DistOrigin();
                Log($"{segmentA} intersects {segmentB} at {p}, dist {d}");
                if (0 < d && d < min) {
                    min = d;
                    best = p.Value;
                }
            }
        }
    }

    return best;
}
//
// Intersection closest along the circuit (part 2)
long ClosestIntersectionViaCircuit(string path0, string path1) {
    var segments0 = GetSegments(path0);
    var segments1 = GetSegments(path1);

    var min = long.MaxValue;
    var best = default(Point2D);
    var steps0 = 0L;
    foreach (var segmentA in segments0) {
        var steps1 = 0L;
        foreach (var segmentB in segments1) {
            var p = Line.IntersectOrtho(segmentA, segmentB);
            if (p.HasValue) {
                var d = steps0 + steps1
                    + new Line(segmentA.P, p.Value).Length
                    + new Line(segmentB.P, p.Value).Length;
                Log($"{segmentA} intersects {segmentB} at {p}, dist {d}");
                if (0 < d && d < min) {
                    min = d;
                    best = p.Value;
                }
            }
            steps1 += segmentB.Length;
        }
        steps0 += segmentA.Length;
    }
    Log(best);

    return min;
}

IEnumerable<Point2D> GetSteps((Direction, long)[] path)
    => path.AggregateCollect(new Point2D(0, 0),
            (p, step) => p.Move(step.Item1, step.Item2));

(Direction, long)[] ReadPath(string line) {
    var moves = line.Split(',');
    return moves.Select(m => (m[0].AsDirection(), Input.GetNumber(m))).ToArray();
}

if (args.Length == 0) {
    EnableLogging = true;

    AssertEqual(Line.IntersectOrtho(
            new Line((-5, 8), (-5, 3)),
            new Line((-7, 6), (-3, 6))),
            (-5, 6));

    AssertEqual(ClosestIntersection(
        "R8,U5,L5,D3",
        "U7,R6,D4,L4").DistOrigin(),
            6);
    AssertEqual(ClosestIntersection(
        "R75,D30,R83,U83,L12,D49,R71,U7,L72",
        "U62,R66,U55,R34,D71,R55,D58,R83").DistOrigin(),
            159);
    AssertEqual(ClosestIntersectionViaCircuit(
        "R8,U5,L5,D3",
        "U7,R6,D4,L4"),
            30);
    AssertEqual(ClosestIntersectionViaCircuit(
        "R75,D30,R83,U83,L12,D49,R71,U7,L72",
        "U62,R66,U55,R34,D71,R55,D58,R83"),
            610);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
