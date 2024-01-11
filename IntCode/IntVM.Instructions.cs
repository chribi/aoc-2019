namespace IntCode;

public sealed partial class IntVM {
    private static Dictionary<int, Instr> OpTable;

    static IntVM() {
        OpTable = new Dictionary<int, Instr>();
        OpTable[1] = new Instr(3, "ADD", Add);
        OpTable[2] = new Instr(3, "MUL", Mult);
        OpTable[99] = new Instr(0, "EXIT", Exit);
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
}
