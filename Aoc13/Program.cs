using LibAoc;
using static LibAoc.LogUtils;

using IntCode;
using IntCode.IO;

long SolvePart1(IEnumerable<string> input) {
    var arcade = new Arcade();
    var vm = new IntVM(input.First(), arcade, arcade);
    arcade.EnableOutput = false;
    vm.Run();
    return arcade.VideoBuffer.Data().Count(c => c == '#');
}

long SolvePart2(IEnumerable<string> input, bool interactive, int frameDelay) {
    var arcade = new Arcade();
    if (frameDelay > 0 || interactive) {
        arcade.EnableOutput = true;
        Console.Clear();
    }

    var vm = new IntVM(input.First(), arcade, arcade);
    // Insert coin
    vm.Memory[0] = 2;

    var undo = new List<(VMSnapshot, Map2D)>();
    var bot = new Bot(arcade);
    while (vm.State != IntVM.VMState.ExitOk) {
        vm.Run();
        long vmIn;
        if (interactive) {
            vmIn = GetHumanInput();
        } else {
            if (frameDelay > 1)
                Thread.Sleep(frameDelay);
            vmIn = bot.GetBotInput();
        }

        if (vmIn == 2) {
            // allow cheating by undoing the last update
            if (!undo.Any()) continue;
            vm.LoadSnapshot(undo[^1].Item1);
            arcade.VideoBuffer = undo[^1].Item2;
            undo.RemoveAt(undo.Count - 1);
            arcade.Redraw();
            continue;
        }

        if (interactive)
            undo.Add((vm.CreateSnapshot(), arcade.VideoBuffer.Clone()));
        arcade.FeedInput(vmIn);
    }
    if (arcade.EnableOutput)
        Console.Clear();

    return arcade.Score;
}

long GetHumanInput() {
    var c = Console.ReadKey().KeyChar;
    return c switch {
        'h' => -1,
        'l' => 1,
        'k' => 2, // undo cheat code!!
        _ => 0,
    };
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    var interactive = args.Any(arg => arg == "game");
    var animate = args.Any(arg => arg == "animate");
    Utils.AocMain(args, input => SolvePart2(input, interactive, animate ? 20 : 0));
}

class Bot {
    public Bot(Arcade arcade) {
        _arcade = arcade;
    }
    private Arcade _arcade;
    Point2D LastPos { get; set; }
    public long GetBotInput() {
        var buf = _arcade.VideoBuffer;
        var newPos = buf.Positions().First(p => buf[p] == 'o');
        var paddlePos = buf.Positions().First(p => buf[p] == '-');

        var move = 0L;
        // ball directly above paddle
        if (paddlePos.Col == newPos.Col) {
            if (paddlePos.Row - newPos.Row <= 1) {
                // ball is hitting paddle, don't move
                move = 0;
            } else {
                // move in same direction the ball is moving
                move = newPos.Col - LastPos.Col;
            }
        } else {
            // move to keep paddle below ball
            move = Math.Sign(newPos.Col - paddlePos.Col);
        }
        LastPos = newPos;
        return move;
    }
}

class Arcade : VMBlockingInput, VMOutput {
    public static char[] TILES = new[] {
        ' ', // Empty,
        '!', // Wall,
        '#', // Block,
        '-', // Paddle,
        'o' // Ball
    };

    public Map2D VideoBuffer { get; set; }
    private List<long> _readBuffer = new List<long>();
    public long Score { get; set; }
    public bool EnableOutput { get; set; }
    private string _status = "";
    public string Status {
        get => _status;
        set {
            _status = value;
            UpdateStatus();
        }
    }

    public Arcade() {
        VideoBuffer = new Map2D(38, 24);
    }

    ConsoleColor? Coloring(char c) {
        if (c == ' ') return ConsoleColor.White;
        if (c == '!') return ConsoleColor.Gray;
        if (c == '#') return ConsoleColor.DarkBlue;
        return null;
    }

    public void Redraw() {
        Console.Clear();
        VideoBuffer.PrintColored(Coloring);
        UpdateStatus();
    }

    long? _input = null;

    public void FeedInput(long value) {
        _input = value;
    }
    public bool CanRead() {
        Console.SetCursorPosition(0, 25);
        return _input != null;
    }

    public long Read() {
        if (_input is long value) {
            _input = null;
            return value;
        }
        throw new InvalidOperationException("Input read while none available");
    }

    public void UpdateStatus() {
        if (!EnableOutput) return;
        Console.SetCursorPosition(3, VideoBuffer.Height + 2);
        var status = $"Score: {Score} {Status}";
        Console.Write(status);
    }

    public void Reset() { }

    public void Write(long number) {
        _readBuffer.Add(number);
        if (_readBuffer.Count == 3) {
            var col = (int)_readBuffer[0];
            var row = (int)_readBuffer[1];
            if (col == -1 && row == 0) {
                Score = _readBuffer[2];
                UpdateStatus();
            } else {
                var tile = TILES[_readBuffer[2]];
                VideoBuffer[row, col] = tile;
                if (EnableOutput) {
                    Console.SetCursorPosition(col, row);
                    if (Coloring(tile) is ConsoleColor color)
                        Console.BackgroundColor = color;
                    Console.Write(tile);
                    Console.ResetColor();
                }
            }
            _readBuffer.Clear();
        }
    }
}
