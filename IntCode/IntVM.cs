namespace IntCode;

using IntCode.IO;

using System.Text;
using System.Text.RegularExpressions;

public sealed partial class IntVM {
    public enum VMState { Running, ExitOk, ExitFail }
    public enum ParamMode { Position, Immediate }
    public record struct Param(ParamMode Mode, int N);
    public record class Instr(int ArgCount, string Name, Action<IntVM, Param[]> Exec);

    private readonly int[] _initial;

    public int[] Memory { get; private set; } = Array.Empty<int>();
    public int IP { get; private set; }
    public VMInput Input { get; }
    public VMOutput Output { get; }
    public VMState State { get; private set; }

    public IntVM(int[] memory, VMInput? input = null, VMOutput? output = null) {
        _initial = memory;
        Input = input ?? new Stdin();
        Output = output ?? new Stdout();
        Reset();
    }

    public IntVM(string memory, VMInput? input = null, VMOutput? output = null)
        : this(Read(memory), input, output) { }

    public void Reset() {
        Memory = (int[])_initial.Clone();
        IP = 0;
        Input.Reset();
        Output.Reset();
        State = VMState.Running;
    }

    public void Run() {
        while (State == VMState.Running) {
            Step();
        }
    }

    public void Step() {
        var opCode = GetOpCode(Memory[IP]);

        if (!OpTable.TryGetValue(opCode, out var instr)) {
            State = VMState.ExitFail;
            throw new Errors.VMExecutionException($"Error executing {opCode} at IP = {IP}: Unknown OpCode");
        }

        try {
            // remember IP for Jump detection
            var ip = IP;
            var args = GetParams(instr.ArgCount);
            instr.Exec(this, args);
            if (IP == ip)
                IP += instr.ArgCount + 1;
        } catch (Exception e) {
            State = VMState.ExitFail;
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

    private static int GetOpCode(int value) => value % 100;

    private int Eval(Param p) {
        return p.Mode switch {
            ParamMode.Position => Memory[p.N],
            ParamMode.Immediate => p.N,
            _ => throw new ArgumentOutOfRangeException(nameof(p.Mode)),
        };
    }

    private void Set(Param p, int value) {
        if (p.Mode != ParamMode.Position)
            throw new InvalidOperationException("Can't set immediate value");
        Memory[p.N] = value;
    }

    private static int[] Read(string line) {
        var nums = new Regex(@"-?\d+").Matches(line);
        return nums.Select(m => int.Parse(m.Value)).ToArray();
    }
}
