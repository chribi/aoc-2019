namespace IntCode.Test;

using IntCode;

[TestFixture]
public class IntVMTest {

    [Test]
    public void Add_WithPositionMode() {
        var vm = new IntVM(new long[] { 1, 4, 5, 6, 10, 20, 0 });
        vm.Step();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 1, 4, 5, 6, 10, 20, 30 }));
        Assert.That(vm.IP, Is.EqualTo(4));
    }

    [Test]
    public void Add_WithImmediateMode() {
        var vm = new IntVM(new long[] { 11_01, 4, 5, 6, 10, 20, 0 });
        vm.Step();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 1101, 4, 5, 6, 10, 20, 9 }));
        Assert.That(vm.IP, Is.EqualTo(4));
    }

    [Test]
    public void Add_WithMixedParamMode1() {
        var vm = new IntVM(new long[] { 10_01, 4, 5, 6, 10, 20, 0 });
        vm.Step();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 1001, 4, 5, 6, 10, 20, 15 }));
        Assert.That(vm.IP, Is.EqualTo(4));
    }

    [Test]
    public void Add_WithMixedParamMode2() {
        var vm = new IntVM(new long[] { 1_01, 4, 5, 6, 10, 20, 0 });
        vm.Step();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 101, 4, 5, 6, 10, 20, 24 }));
        Assert.That(vm.IP, Is.EqualTo(4));
    }

    [Test]
    public void Add_WithInvalidTargetParamMode() {
        var vm = new IntVM(new long[] { 101_01, 4, 5, 6, 10, 20, 0 });
        Assert.Throws<IntCode.Errors.VMExecutionException>(() => vm.Step());
        Assert.That(vm.State, Is.EqualTo(IntVM.VMState.ExitFail));
    }

    [Test]
    public void Mult_WithImmediateMode() {
        var vm = new IntVM(new long[] { 11_02, 4, 5, 6, 10, 20, 0 });
        vm.Step();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 1102, 4, 5, 6, 10, 20, 20 }));
        Assert.That(vm.IP, Is.EqualTo(4));
    }

    [Test]
    public void Exit() {
        var vm = new IntVM(new long[] { 99, 1, 4, 5, 6, 10, 20, 0 });
        vm.Run();
        Assert.That(vm.Memory, Is.EqualTo(new[] { 99, 1, 4, 5, 6, 10, 20, 0 }));
        Assert.That(vm.State, Is.EqualTo(IntVM.VMState.ExitOk));
    }
}
