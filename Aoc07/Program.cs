using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

using IntCode;
using IntCode.IO;

int SolvePart1(IEnumerable<string> input) {
    var program = IntVM.Read(input.First());
    var (best, _) = Optimize(AmpChain(program), Enumerable.Range(0, 5).ToArray());
    return best;
}

int SolvePart2(IEnumerable<string> input) {
    var program = IntVM.Read(input.First());
    var (best, _) = Optimize(AmpLoop(program), Enumerable.Range(5, 5).ToArray());
    return best;
}

IntVM[] AmpChain(int[] program) {
    var vms = new IntVM[5];
    var linkBack = new ChainIO();
    for (var i = 0; i < 5; i++) {
        var linkForward = new ChainIO();
        vms[i] = new IntVM(program, linkBack, linkForward);
        vms[i].LogCallback = args => Log($"AMP {i}:", string.Join(" ", args));
        linkBack = linkForward;
    }

    return vms;
}

IntVM[] AmpLoop(int[] program) {
    var chain = AmpChain(program);
    var loopLink = chain[0].Input as ChainIO;
    chain[^1].Output = loopLink!;

    return chain;
}

int RunAmps(IntVM[] amps, int[] phaseSettings) {
    for (var i = 0; i < amps.Length; i++) {
        amps[i].Reset();
    }

    for (var i = 0; i < amps.Length; i++) {
        (amps[i].Input as ChainIO)!.Feed(phaseSettings[i]);
    }
    (amps[0].Input as ChainIO)!.Feed(0);

    var stopped = false;
    while (!stopped) {
        stopped = true;
        foreach (var vm in amps) {
            vm.Run();
            if (vm.State == IntVM.VMState.Blocked) {
                stopped = false;
            }
        }
    }

    return (amps[^1].Output as ChainIO)!.Values.Dequeue();
}

(int, int[]) Optimize(IntVM[] ampChain, int[] phaseOptions) {
    var best = int.MinValue;
    var bestSettings = Array.Empty<int>();
    foreach (var phaseSettings in Permutations(phaseOptions)) {
        var result = RunAmps(ampChain, phaseSettings);
        if (result > best) {
            Log("PhaseSettings", string.Join(", ", phaseSettings), "=>", result);
            best = result;
            bestSettings = phaseSettings;
        }
    }
    Log(best, string.Join(", ", bestSettings));
    return (best, bestSettings);
}

// Generates permuations in-place
IEnumerable<int[]> Permutations(int[] list) {
    return perms(0);

    IEnumerable<int[]> perms(int keep) {
        if (keep == list.Length) yield return list;
        for (var i = keep; i < list.Length; i++) {
            swap(i, keep);
            foreach (var p in perms(keep + 1)) yield return p;
            swap(i, keep);
        }
    }

    void swap(int i, int j) {
        var tmp = list[i];
        list[i] = list[j];
        list[j] = tmp;
    }
}

if (args.Length == 0) {
    EnableLogging = true;
    AssertEqual(Permutations(new[] { 1, 2, 3 }).Count(), 6);
    AssertEqual(Permutations(new[] { 1, 2, 3, 4 }).Count(), 24);

    var sample1 = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0";
    AssertEqual(SolvePart1(new[] { sample1 }), 43210);
    var sample2 = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5";
    var loop = AmpLoop(IntVM.Read(sample2));
    AssertEqual(RunAmps(loop, new [] { 9, 8, 7, 6, 5 }), 139629729);
    AssertEqual(SolvePart2(new[] { sample2 }), 139629729);
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
