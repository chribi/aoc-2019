using LibAoc;
using static LibAoc.LogUtils;

int SolvePart1(IEnumerable<string> input) {
    var layers = ReadLayers(input.First());
    var minZeroLayer = layers.MinBy(img => img.Data().Count(c => c == '0'))!;
    var ones = minZeroLayer.Data().Count(c => c == '1');
    var twos = minZeroLayer.Data().Count(c => c == '2');
    return ones * twos;
}

ConsoleColor? Coloring(char c) {
    if (c == '0') return ConsoleColor.Blue;
    if (c == '1') return ConsoleColor.Red;
    return null;
}

int SolvePart2(IEnumerable<string> input) {
    var layers = ReadLayers(input.First());
    var result = CombineLayers(layers);
    result.PrintColored(Coloring);
    return 0;
}

Map2D CombineLayers(List<Map2D> layers) {
    var result = new Map2D(layers[0].Width, layers[0].Height);
    foreach (var (row, col) in result.Positions()) {
        // 0 = black
        // 1 = white
        // 2 = transparent
        var color = layers.First(img => img[row, col] != '2')[row, col];
        result[row, col] = color;
    }

    return result;
}

List<Map2D> ReadLayers(string data) {
    var offset = 0;
    List<Map2D> layers = new List<Map2D>();
    while (offset < data.Length) {
        layers.Add(ReadImage(data, ref offset, 25, 6));
    }
    return layers;
}

Map2D ReadImage(string data, ref int offset, int width, int height) {
    var image = new string[height];
    for (var row = 0; row < height; row++) {
        image[row] = data[offset..(offset+width)];
        offset += width;
    }
    return new Map2D(image);
}

if (args.Length == 0) {
    EnableLogging = true;
} else {
    Utils.AocMain(args, SolvePart1);
    Utils.AocMain(args, SolvePart2);
}
