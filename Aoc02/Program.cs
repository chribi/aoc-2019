using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

int SolvePart1(IEnumerable<string> input) {
    var program = ReadProgram(input.First());
    return Run(program, 12, 2);
}

int SolvePart2(IEnumerable<string> input) {
    var program = ReadProgram(input.First());
    var target = 19690720;
    for (var sum = 1; ; sum++) {
        for (var noun = 0; noun <= sum; noun++) {
            var verb = sum - noun;
            if (Run(program, noun, verb) == target)
                return 100 * noun + verb;
        }
    }
}

int[] ReadProgram(string input) {
    return Input.GetIntNumbers(input).ToArray();
}

int Run(int[] program, int noun, int verb) {
    // Create copy
    program = program.ToArray();
    program[1] = noun;
    program[2] = verb;
    RunProgram(program);
    return program[0];
}

void RunProgram(int[] program) {
    var ip = 0;
    while (program[ip] != 99) {
        var ap = program[ip + 1];
        var bp = program[ip + 2];
        var cp = program[ip + 3];

        var a = program[ap];
        var b = program[bp];
        var result = program[ip] == 1 ? a + b : a * b;

        program[cp] = result;

        ip += 4;
    }
}

if (args.Length == 0) {
    EnableLogging = true;
    var program = ReadProgram("1,9,10,3,2,3,11,0,99,30,40,50");
    RunProgram(program);
    AssertEqual(program[0], 3500);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
