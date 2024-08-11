namespace SPRXPatcher.Elf;

public class ElfProgramHeader {
    public ProgramType Type;
    public ProgramFlags Flags;
    public ulong Offset;
    public ulong VirtualAddress;
    public ulong PhysicalAddress;
    public ulong FileSize;
    public ulong MemorySize;
    public ulong Alignment;

    public ElfProgramHeader() { }

    public ElfProgramHeader(BigBinaryReader br) {
        this.Type = (ProgramType) br.ReadUInt32();
        this.Flags = (ProgramFlags) br.ReadUInt32();
        this.Offset = br.ReadUInt64();
        this.VirtualAddress = br.ReadUInt64();
        this.PhysicalAddress = br.ReadUInt64();
        this.FileSize = br.ReadUInt64();
        this.MemorySize = br.ReadUInt64();
        this.Alignment = br.ReadUInt64();
    }

    public void Write(BigBinaryWriter bw) {
        bw.Write((uint) this.Type);
        bw.Write((uint) this.Flags);
        bw.Write(this.Offset);
        bw.Write(this.VirtualAddress);
        bw.Write(this.PhysicalAddress);
        bw.Write(this.FileSize);
        bw.Write(this.MemorySize);
        bw.Write(this.Alignment);
    }

    public enum ProgramType : uint {
        Null = 0,
        Load = 1,
        Dynamic = 2,
        Interp = 3,
        Note = 4,
        Shlib = 5,
        Phdr = 6,
        Tls = 7,
        Loos = 0x60000000,
        Hios = 0x6FFFFFFF,
        Loproc = 0x70000000,
        Hiproc = 0x7FFFFFFF
    }

    [Flags]
    public enum ProgramFlags : uint {
        X = 1,
        W = 2,
        R = 4
    }
}
