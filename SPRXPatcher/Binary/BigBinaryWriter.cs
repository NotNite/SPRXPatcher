namespace SPRXPatcher;

public class BigBinaryWriter(Stream output) : BinaryWriter(output) {
    public override void Write(short value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(int value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(long value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(ushort value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(uint value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }

    public override void Write(ulong value) {
        var bytes = BitConverter.GetBytes(value);
        Array.Reverse(bytes);
        base.Write(bytes);
    }
}
