using System.Reflection;
using System.Text;

namespace SPRXPatcher;

public class ShellcodeCreator {
    private byte[] data;

    public ShellcodeCreator() {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "prx_loader.bin";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new InvalidOperationException("Shellcode not found");

        using var br = new BinaryReader(stream);
        this.data = br.ReadBytes((int) stream.Length);
    }

    public uint[] BuildJump(uint address) {
        var upper = (ushort) (address >> 16);
        var lower = (ushort) (address & 0xFFFF);
        return [
            0x3D600000 | (uint) upper, // lis r11, upper
            0x616B0000 | (uint) lower, // ori r11, r11, lower
            0x7D6903A6,                // mtctr r11
            0x4E800420                 // bctr
        ];
    }

    public (byte[] Payload, uint[] NewEntrypoint) Build(
        string sprxPath,
        uint[] entrypointInstructions,
        uint entrypointAddress,
        uint payloadAddress
    ) {
        var sprxPathBytes = Encoding.ASCII.GetBytes(sprxPath);
        var payloadSize = this.data.Length + (entrypointInstructions.Length * 4);
        var newBuffer = new byte[payloadSize + sprxPathBytes.Length + 1];
        Array.Copy(this.data, newBuffer, this.data.Length);
        Array.Copy(sprxPathBytes, 0, newBuffer, payloadSize, sprxPathBytes.Length);
        newBuffer[^1] = 0;

        var sprxPathAddress = (uint) (payloadAddress + (ulong) payloadSize);
        var sprxPathAddressUpper = (ushort) (sprxPathAddress >> 16);
        var sprxPathAddressLower = (ushort) (sprxPathAddress & 0xFFFF);

        using var rms = new MemoryStream(newBuffer);
        using var wms = new MemoryStream(newBuffer);
        using var br = new BigBinaryReader(rms);
        using var bw = new BigBinaryWriter(wms);

        // Write the entrypoint instructions
        var entrypointInstructionOffset = this.data.Length - (entrypointInstructions.Length * 4);
        for (var i = 0; i < entrypointInstructions.Length; i++) {
            bw.BaseStream.Seek(entrypointInstructionOffset + (i * 4), SeekOrigin.Begin);
            bw.Write(entrypointInstructions[i]);
        }

        // Write our jump
        var jump = this.BuildJump((uint) (entrypointAddress + (entrypointInstructions.Length * 4)));
        foreach (var instruction in jump) bw.Write(instruction);

        // Write the address of the sprx path to the payload
        const int pos = 128;
        bw.BaseStream.Seek(pos + 2, SeekOrigin.Begin);
        bw.Write(sprxPathAddressUpper);
        bw.BaseStream.Seek(pos + 4 + 2, SeekOrigin.Begin);
        bw.Write(sprxPathAddressLower);

        return (wms.ToArray(), this.BuildJump(payloadAddress));
    }
}
