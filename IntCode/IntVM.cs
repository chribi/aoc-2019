namespace IntCode;

using IntCode.IO;

using System.Text;
using System.Text.RegularExpressions;

public sealed partial class IntVM {
    public enum VMState { Running, ExitOk, ExitFail, Blocked }
    public enum ParamMode { Position, Immediate, Relative }
    public record struct Param(ParamMode Mode, long N);
    public record class Instr(int ArgCount, string Name, Action<IntVM, Param[]> Exec);

    private readonly long[] _initial;

    public long[] Memory { get; private set; } = Array.Empty<long>();
    private const int PAGESIZE = 256;
    public Dictionary<long, long[]> AdditionalMemoryPages = new Dictionary<long, long[]>();
    public long IP { get; private set; }
    public long RBP { get; private set; }
    public VMInput Input { get; set; }
    public VMOutput Output { get; set; }
    public VMState State { get; private set; }
    public Action<object[]>? LogCallback { get; set; }

    public IntVM(long[] memory, VMInput? input = null, VMOutput? output = null) {
        _initial = memory;
        Input = input ?? new Stdin();
        Output = output ?? new Stdout();
        Reset();
    }

    public IntVM(string memory, VMInput? input = null, VMOutput? output = null)
        : this(Read(memory), input, output) { }

    public void Reset() {
        Memory = (long[])_initial.Clone();
        AdditionalMemoryPages.Clear();
        IP = 0;
        RBP = 0;
        Input.Reset();
        Output.Reset();
        State = VMState.Running;
    }

    public void Run() {
        if (State == VMState.Blocked) {
            State = VMState.Running;
        }

        while (State == VMState.Running) {
            Step();
        }
    }

    public void Step() {
        var opCode = GetOpCode(SaveAccessMem(IP));

        if (!OpTable.TryGetValue(opCode, out var instr)) {
            State = VMState.ExitFail;
            throw new Errors.VMExecutionException($"Error executing {opCode} at IP = {IP}: Unknown OpCode");
        }

        try {
            // remember IP for Jump detection
            var ip = IP;
            var args = GetParams(instr.ArgCount);
            instr.Exec(this, args);
            if (IP == ip && State != VMState.Blocked)
                IP += instr.ArgCount + 1;
        } catch (Exception e) {
            State = VMState.ExitFail;

            DumpMem();
            throw new Errors.VMExecutionException($"Error executing {opCode} ({instr.Name}) at IP = {IP}: {e.Message}", e);
        }
    }

    public void DumpMem(int perLine = 10) {

        var sb = new StringBuilder();
        for (var i = 0; i < Memory.Length; i++) {
            if (i % perLine == 0) {
                if (i > 0) Console.WriteLine(sb);
                sb.Clear();
                sb.Append($"[{i,4}]:");
            }

            sb.Append($" {Memory[i],5}");
        }
    }

    public Param[] GetParams(int count) {
        var paramModes = Memory[IP] / 100;
        var result = new Param[count];
        for (var i = 0; i < count; i++) {
            var n = Memory[IP + i + 1];
            var mode = (ParamMode)(paramModes % 10);
            result[i] = new(mode, n);

            paramModes = paramModes / 10;
        }

        return result;
    }

    private void Log(params object[] values) {
        LogCallback?.Invoke(values);
    }

    private static long GetOpCode(long value) => value % 100;

    private long Eval(Param p) {
        return p.Mode switch {
            ParamMode.Position => SaveAccessMem(p.N),
            ParamMode.Immediate => p.N,
            ParamMode.Relative => SaveAccessMem(RBP + p.N),
            _ => throw new ArgumentOutOfRangeException(nameof(p.Mode)),
        };
    }

    private ref long SaveAccessMem(long index) {
        if (index < Memory.Length) return ref Memory[index];
        var pageNum = index / PAGESIZE;
        var pageOffset = index % PAGESIZE;
        if (!AdditionalMemoryPages.TryGetValue(pageNum, out var page)) {
            page = new long[PAGESIZE];
            AdditionalMemoryPages[pageNum] = page;
        }
        return ref page[pageOffset];
    }

    private void Set(Param p, long value) {
        if (p.Mode == ParamMode.Immediate)
            throw new InvalidOperationException("Can't set immediate value");
        var memLocation = p.Mode == ParamMode.Position ? p.N : RBP + p.N;
        SaveAccessMem(memLocation) = value;
    }

    public static long[] Read(string line) {
        var nums = new Regex(@"-?\d+").Matches(line);
        return nums.Select(m => long.Parse(m.Value)).ToArray();
    }
}
