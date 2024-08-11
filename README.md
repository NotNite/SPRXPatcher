# SPRXPatcher

A modern PlayStation 3 ELF patcher to load SPRX files.

## Usage

Obtain the .ELF you want to patch (*not* a [.SELF](https://psdevwiki.com/ps3/SELF_-_SPRX)/EBOOT.BIN, you must decrypt them first). Then, run the patcher with the path to your .sprx on the console. If your .sprx is at `/dev_hdd0/tmp/mycoolplugin.sprx`, and you have the decrypted executable at `input.elf`, run:

```sh
dotnet run --project SPRXPatcher -- ./input.elf /dev_hdd0/tmp/mycoolplugin.sprx ./output.elf
```

## Why?

The PS3 scene is old. So old, that [I am two months older than the PlayStation 3](https://en.wikipedia.org/wiki/PlayStation_3#Launch). As it grows in age, tools and knowledge become lost, and the community and passion fades away. I wanted to make a simple mod for a PS3 game, and all I could find was random forum posts with 0 answers and YouTube tutorials that I didn't want to watch.

This hurt to see. After spending multiple days getting a .sprx to build, I found out the most popular public tool is a closed source application that's just a wrapper for another closed source CLI tool. I ended up looking into how they work (writing shellcode into the executable), and tried my hand at making something similar.

The patcher functions by parsing the ELF file format, relocating the program header to the end of the file, adding a custom section/segment with some shellcode, and modifying the entrypoint to jump to the segment. It's not perfect, and there are probably lots of bugs and broken scenarios, but it works for my use cases. If it doesn't work for you, please leave an issue (preferably with the game/executable you tested on). **I have not tested this on a real console yet, only in emulators.**

## Credits

- [Aly](https://github.com/s5bug) for suggesting program header shenanigans
- [Emma](https://github.com/InvoxiPlayGames) for the shellcode and infinite wisdom
- [RPCS3](https://github.com/RPCS3/rpcs3) for ~~hammering my CPU at 100% while compiling~~ providing useful tools for figuring out what I broke
