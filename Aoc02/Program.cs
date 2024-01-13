using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;
using IntCode;

long SolvePart1(IEnumerable<string> input) {
    var vm = new IntVM(input.First());
    return Run(vm, 12, 2);
}

long SolvePart2(IEnumerable<string> input) {
    var vm = new IntVM(input.First());
    var target = 19690720;
    for (var sum = 1; ; sum++) {
        for (var noun = 0; noun <= sum; noun++) {
            var verb = sum - noun;
            if (Run(vm, noun, verb) == target)
                return 100 * noun + verb;
        }
    }
}

long Run(IntVM vm, long noun, long verb) {
    vm.Reset();
    vm.Memory[1] = noun;
    vm.Memory[2] = verb;
    vm.Run();
    return vm.Memory[0];
}

if (args.Length == 0) {
    EnableLogging = true;
    var program = new IntVM("1,9,10,3,2,3,11,0,99,30,40,50");
    program.Run();
    AssertEqual(program.Memory[0], 3500);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
