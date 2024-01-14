using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

using IntCode;
using IntCode.IO;

long SolvePart1(IEnumerable<string> input) {
    var robot = new Robot();
    var vm = new IntVM(input.First(), robot, robot);
    vm.Run();
    return robot.Painted.Values.Count;
}

ConsoleColor? Coloring(char c) {
    if (c == '.') return ConsoleColor.Blue;
    if (c == '#') return ConsoleColor.Red;
    return null;
}
long SolvePart2(IEnumerable<string> input) {
    var robot = new Robot();
    var vm = new IntVM(input.First(), robot, robot);
    robot.Painted[robot.Position] = Robot.WHITE;
    vm.Run();
    var painting = ConvertToMap(robot.Painted);
    painting.PrintColored(Coloring);
    return 0L;
}

Map2D ConvertToMap(Dictionary<Point2D, long> painted) {
    var minRow = painted.Keys.Min(pos => pos.Row);
    var maxRow = painted.Keys.Max(pos => pos.Row);
    var minCol = painted.Keys.Min(pos => pos.Col);
    var maxCol = painted.Keys.Max(pos => pos.Col);
    // + 3 for a nice border around our painting
    var height = maxRow - minRow + 3;
    var width = maxCol - minCol + 1;
    var adjust = new Point2D(1 - minRow, - minCol);
    var map = new Map2D((int)width, (int)height, initial: '.');
    foreach (var (pos, color) in painted) {
        map[pos + adjust] = color == Robot.BLACK ? '.' : '#';
    }

    return map;
}

if (args.Length == 0) {
    EnableLogging = true;
    AssertEqual(Direction.U.NextCounterClockwise(), Direction.L);
    AssertEqual(Direction.U.NextClockwise(), Direction.R);
    AssertEqual(Direction.L.NextCounterClockwise(), Direction.D);
    AssertEqual(Direction.L.NextClockwise(), Direction.U);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}

class Robot : VMInput, VMOutput {
    public const long BLACK = 0;
    public const long WHITE = 1;

    public Direction Direction { get; private set; }
    public Point2D Position { get; private set; }
    public Dictionary<Point2D, long> Painted { get; }
    bool _paintMode = true;

    public Robot() {
        Painted = new Dictionary<Point2D, long>();
        Position = new Point2D(0, 0);
        Direction = Direction.U;
    }

    public long Read() {
        return Painted.TryGetValue(Position, out var color) ? color : BLACK;
    }

    public void Reset() { }

    public void Write(long number) {
        if (_paintMode) {
            Painted[Position] = number;
        } else {
            if (number == 0) {
                Direction = Direction.NextCounterClockwise();
            } else {
                Direction = Direction.NextClockwise();
            }
            Position = Position.Move(Direction, 1);

        }
        _paintMode = !_paintMode;
    }
}
