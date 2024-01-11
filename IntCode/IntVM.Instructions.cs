namespace IntCode;

public sealed partial class IntVM {
    private static Dictionary<int, Instr> OpTable;

    static IntVM() {
        OpTable = new Dictionary<int, Instr>();

        // day 2
        OpTable[99] = new Instr(0, "EXIT", Exit);
        OpTable[1] = new Instr(3, "ADD", Add);
        OpTable[2] = new Instr(3, "MUL", Mult);

        // day 5
        OpTable[3] = new Instr(1, "INP", ReadInput);
        OpTable[4] = new Instr(1, "PRNT", WriteOutput);

        OpTable[5] = new Instr(2, "JNZ", JumpIfTrue);
        OpTable[6] = new Instr(2, "JZ", JumpIfFalse);

        OpTable[7] = new Instr(3, "LT", LessThan);
        OpTable[8] = new Instr(3, "EQ", Equal);
    }

    private static void Add(IntVM vm, Param[] args) {
        var a = vm.Eval(args[0]);
        var b = vm.Eval(args[1]);
        vm.Set(args[2], a + b);
    }

    private static void Mult(IntVM vm, Param[] args) {
        var a = vm.Eval(args[0]);
        var b = vm.Eval(args[1]);
        vm.Set(args[2], a * b);
    }

    private static void Exit(IntVM vm, Param[] args) {
        vm.State = VMState.ExitOk;
    }

    private static void ReadInput(IntVM vm, Param[] args) {
        vm.Set(args[0], vm.Input.Read());
    }

    private static void WriteOutput(IntVM vm, Param[] args) {
        var a = vm.Eval(args[0]);
        vm.Output.Write(a);
    }

    private static void JumpIfTrue(IntVM vm, Param[] args) {
        var jmp = vm.Eval(args[0]) != 0;
        if (jmp)
            vm.IP = vm.Eval(args[1]);
    }

    private static void JumpIfFalse(IntVM vm, Param[] args) {
        var jmp = vm.Eval(args[0]) == 0;
        if (jmp)
            vm.IP = vm.Eval(args[1]);
    }

    private static void LessThan(IntVM vm, Param[] args) {
        var a = vm.Eval(args[0]);
        var b = vm.Eval(args[1]);
        vm.Set(args[2], a < b ? 1 : 0);
    }

    private static void Equal(IntVM vm, Param[] args) {
        var a = vm.Eval(args[0]);
        var b = vm.Eval(args[1]);
        vm.Set(args[2], a == b ? 1 : 0);
    }
}
