using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

using IntCode;
using IntCode.IO;

int SolvePart1(IEnumerable<string> input) {
    return 0;
}

if (args.Length == 0) {
    EnableLogging = true;
    var output = new CollectOutput();
    var quineVm = new IntVM("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99",
            null, output);
    var code = (int[])quineVm.Memory.Clone();
    quineVm.Run();
    AssertEqualLists(output.Output, code, "Quine");
} else {
    Utils.AocMain(args, SolvePart1);
    // Utils.AocMain(args, SolvePart2);
}
