namespace SPRXPatcher;

public class BigBinaryReader(Stream input) : BinaryReader(input) {
    public override short ReadInt16() {
        var bytes = base.ReadBytes(2);
        Array.Reverse(bytes);
        return BitConverter.ToInt16(bytes, 0);
    }

    public override int ReadInt32() {
        var bytes = base.ReadBytes(4);
        Array.Reverse(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public override long ReadInt64() {
        var bytes = base.ReadBytes(8);
        Array.Reverse(bytes);
        return BitConverter.ToInt64(bytes, 0);
    }

    public override ushort ReadUInt16() {
        var bytes = base.ReadBytes(2);
        Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }

    public override uint ReadUInt32() {
        var bytes = base.ReadBytes(4);
        Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public override ulong ReadUInt64() {
        var bytes = base.ReadBytes(8);
        Array.Reverse(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }
}
