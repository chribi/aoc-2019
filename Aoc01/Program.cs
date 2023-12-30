using LibAoc;
using static LibAoc.LogUtils;
using static LibAoc.Test;

int SolvePart1(IEnumerable<string> input) {
    return input.Select(int.Parse)
        .Select(RequiredFuel)
        .Sum();
}

int SolvePart2(IEnumerable<string> input) {
    return input.Select(int.Parse)
        .Select(TotalRequiredFuel)
        .Sum();
}

int RequiredFuel(int mass) => mass / 3 - 2;

int TotalRequiredFuel(int mass) {
    var fuel = RequiredFuel(mass);
    if (fuel <= 8) {
        return fuel;
    }
    return fuel + TotalRequiredFuel(fuel);
}


if (args.Length == 0) {
    EnableLogging = true;
    AssertEqual(RequiredFuel(12), 2, "Required Fuel");
    AssertEqual(RequiredFuel(14), 2, "Required Fuel");
    AssertEqual(RequiredFuel(1969), 654, "Required Fuel");
    AssertEqual(RequiredFuel(100756), 33583, "Required Fuel");

    AssertEqual(TotalRequiredFuel(12), 2, "Total Required Fuel");
    AssertEqual(TotalRequiredFuel(14), 2, "Total Required Fuel");
    AssertEqual(TotalRequiredFuel(1969), 966, "Total Required Fuel");
    AssertEqual(TotalRequiredFuel(100756), 50346, "Total Required Fuel");

} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
