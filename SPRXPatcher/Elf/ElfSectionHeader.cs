namespace SPRXPatcher.Elf;

public class ElfSectionHeader {
    public uint NameOffset;
    public SectionType Type;
    public SectionFlags Flags;
    public ulong VirtualAddress;
    public ulong PhysicalAddress;
    public ulong FileSize;
    public uint Link;
    public uint Info;
    public ulong Alignment;
    public ulong EntrySize;

    public byte[]? Data;

    public ElfSectionHeader() { }

    public ElfSectionHeader(BigBinaryReader br) {
        this.NameOffset = br.ReadUInt32();
        this.Type = (SectionType) br.ReadUInt32();
        this.Flags = (SectionFlags) br.ReadUInt64();
        this.VirtualAddress = br.ReadUInt64();
        this.PhysicalAddress = br.ReadUInt64();
        this.FileSize = br.ReadUInt64();
        this.Link = br.ReadUInt32();
        this.Info = br.ReadUInt32();
        this.Alignment = br.ReadUInt64();
        this.EntrySize = br.ReadUInt64();
    }

    public void Write(BigBinaryWriter bw) {
        bw.Write(this.NameOffset);
        bw.Write((uint) this.Type);
        bw.Write((ulong) this.Flags);
        bw.Write(this.VirtualAddress);
        bw.Write(this.PhysicalAddress);
        bw.Write(this.FileSize);
        bw.Write(this.Link);
        bw.Write(this.Info);
        bw.Write(this.Alignment);
        bw.Write(this.EntrySize);
    }


    public enum SectionType : uint {
        Null = 0,
        Progbits = 1,
        Symtab = 2,
        Strtab = 3,
        Rela = 4,
        Hash = 5,
        Dynamic = 6,
        Note = 7,
        Nobits = 8,
        Rel = 9,
        Shlib = 10,
        Dynsym = 11,
        InitArray = 14,
        FiniArray = 15,
        PreinitArray = 16,
        Group = 17,
        SymtabShndx = 18,
        Loos = 0x60000000,
    }

    [Flags]
    public enum SectionFlags : ulong {
        Write = 1,
        Alloc = 2,
        ExecInstr = 4,
        Merge = 16,
        Strings = 32,
        InfoLink = 64,
        LinkOrder = 128,
        OsNonConforming = 256,
        Group = 512,
        Tls = 1024,
        MaskOs = 0x0FF00000,
        MaskProc = 0xF0000000,
        Ordered = 0x40000000,
        Exclude = 0x80000000
    }
}
