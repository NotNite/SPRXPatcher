namespace SPRXPatcher.Elf;

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
