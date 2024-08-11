namespace SPRXPatcher.Elf;

/// <summary>
/// A majority of the ELF header can be assumed to be constant due to only targeting the PS3.
/// </summary>
public class ElfHeader {
    public ulong Entry;
    public ulong ProgramHeaderOffset;
    public ulong SectionHeaderOffset;
    public uint Flags;
    public ushort ProgramHeaderCount;
    public ushort SectionHeaderCount;
    public ushort SectionHeaderStringIndex;

    public ElfHeader(BigBinaryReader br) {
        var magic = br.ReadBytes(4);
        if (magic[0] != 0x7F || magic[1] != (byte) 'E' || magic[2] != (byte) 'L' || magic[3] != (byte) 'F') {
            throw new Exception("Invalid ELF magic");
        }

        // 64-bit
        var eiClass = br.ReadByte();
        if (eiClass != 2) throw new Exception("Invalid ELF class (got " + eiClass + ", expected 2)");

        // Big endian
        var eiData = br.ReadByte();
        if (eiData != 2) throw new Exception("Invalid ELF data encoding (got " + eiData + ", expected 2)");

        // Version 1
        var ver = br.ReadByte();
        if (ver != 1) throw new Exception("Invalid ELF version (got " + ver + ", expected 1)");

        // PS3
        var osAbi = br.ReadByte();
        if (osAbi != 0x66) throw new Exception("Invalid ELF OS ABI (got " + osAbi + ", expected 0)");

        // Not used on PS3
        var abiVersion = br.ReadByte();
        if (abiVersion != 0) throw new Exception("Invalid ELF ABI version (got " + abiVersion + ", expected 0)");

        // Padding
        var pad = br.ReadBytes(7);
        if (pad.Any(b => b != 0)) throw new Exception("Invalid ELF padding");

        // Executable
        var type = br.ReadUInt16();
        if (type != 2) throw new Exception("Invalid ELF type (got " + type + ", expected 2)");

        // PowerPC 64-bit
        var machine = br.ReadUInt16();
        if (machine != 0x15) throw new Exception("Invalid ELF machine (got " + machine + ", expected 0x15)");

        var version = br.ReadUInt32();
        if (version != 1) throw new Exception("Invalid ELF version (got " + version + ", expected 1)");

        this.Entry = br.ReadUInt64();
        this.ProgramHeaderOffset = br.ReadUInt64();
        this.SectionHeaderOffset = br.ReadUInt64();

        this.Flags = br.ReadUInt32();

        var size = br.ReadUInt16();
        if (size != 0x40) throw new Exception("Invalid ELF header size (got " + size + ", expected 0x40)");

        var phEntSize = br.ReadUInt16();
        if (phEntSize != 0x38)
            throw new Exception("Invalid ELF program header entry size (got " + phEntSize + ", expected 0x38)");
        this.ProgramHeaderCount = br.ReadUInt16();

        var shEntSize = br.ReadUInt16();
        if (shEntSize != 0x40)
            throw new Exception("Invalid ELF section header entry size (got " + shEntSize + ", expected 0x40)");
        this.SectionHeaderCount = br.ReadUInt16();

        this.SectionHeaderStringIndex = br.ReadUInt16();
    }

    public void Write(BigBinaryWriter bw) {
        bw.Write(new byte[] {0x7F, (byte) 'E', (byte) 'L', (byte) 'F'});
        bw.Write((byte) 2);
        bw.Write((byte) 2);
        bw.Write((byte) 1);
        bw.Write((byte) 0x66);
        bw.Write((byte) 0);
        for (var i = 0; i < 7; i++) bw.Write((byte) 0);
        bw.Write((ushort) 2);
        bw.Write((ushort) 0x15);
        bw.Write((uint) 1);
        bw.Write(this.Entry);
        bw.Write(this.ProgramHeaderOffset);
        bw.Write(this.SectionHeaderOffset);
        bw.Write(this.Flags);
        bw.Write((ushort) 0x40);
        bw.Write((ushort) 0x38);
        bw.Write(this.ProgramHeaderCount);
        bw.Write((ushort) 0x40);
        bw.Write(this.SectionHeaderCount);
        bw.Write(this.SectionHeaderStringIndex);
    }
}
