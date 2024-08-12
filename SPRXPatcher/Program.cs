using System.Text;
using SPRXPatcher.Elf;

if (args.Length != 3) {
    Console.WriteLine("Usage: SPRXPatcher <input.elf> </path/to/sprx> <output.elf>");
    Console.WriteLine("Example: SPRXPatcher input.elf /dev_hdd0/tmp/plugin.sprx output.elf");
    return;
}

var input = args[0];
var sprx = args[1];
var output = args[2];

Console.WriteLine("Patching ELF to load SPRX at " + sprx);

using var inputStream = File.OpenRead(input);
var elf = new ElfFile(inputStream);

elf.SprxPath = sprx;

using var outputStream = File.Create(output);
var ppuHash = elf.Write(outputStream);

Console.WriteLine("Wrote patched ELF to " + output);

var ppuHashHex = BitConverter.ToString(ppuHash).Replace("-", string.Empty).ToLower();
Console.WriteLine("PPU hash: " + ppuHashHex);
