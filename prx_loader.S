# Partially written by Emma (@InvoxiPlayGames) and ruined by @NotNite
# Thanks!

.set PRX_OPT_STACK, 0x40
.set PRX_NAME_STRING, 0x12345678
.set REGISTER_STACK_OFFSET, 0x50

# This sucks but it would crash randomly without it
preserve_registers:
    std 0, REGISTER_STACK_OFFSET(1)
    std 1, REGISTER_STACK_OFFSET + 0x8(1)
    std 2, REGISTER_STACK_OFFSET + 0x10(1)
    std 3, REGISTER_STACK_OFFSET + 0x18(1)
    std 4, REGISTER_STACK_OFFSET + 0x20(1)
    std 5, REGISTER_STACK_OFFSET + 0x28(1)
    std 6, REGISTER_STACK_OFFSET + 0x30(1)
    std 7, REGISTER_STACK_OFFSET + 0x38(1)
    std 8, REGISTER_STACK_OFFSET + 0x40(1)
    std 9, REGISTER_STACK_OFFSET + 0x48(1)
    std 10, REGISTER_STACK_OFFSET + 0x50(1)
    std 11, REGISTER_STACK_OFFSET + 0x58(1)
    std 12, REGISTER_STACK_OFFSET + 0x60(1)
    std 13, REGISTER_STACK_OFFSET + 0x68(1)
    std 14, REGISTER_STACK_OFFSET + 0x70(1)
    std 15, REGISTER_STACK_OFFSET + 0x78(1)
    std 16, REGISTER_STACK_OFFSET + 0x80(1)
    std 17, REGISTER_STACK_OFFSET + 0x88(1)
    std 18, REGISTER_STACK_OFFSET + 0x90(1)
    std 19, REGISTER_STACK_OFFSET + 0x98(1)
    std 20, REGISTER_STACK_OFFSET + 0xa0(1)
    std 21, REGISTER_STACK_OFFSET + 0xa8(1)
    std 22, REGISTER_STACK_OFFSET + 0xb0(1)
    std 23, REGISTER_STACK_OFFSET + 0xb8(1)
    std 24, REGISTER_STACK_OFFSET + 0xc0(1)
    std 25, REGISTER_STACK_OFFSET + 0xc8(1)
    std 26, REGISTER_STACK_OFFSET + 0xd0(1)
    std 27, REGISTER_STACK_OFFSET + 0xd8(1)
    std 28, REGISTER_STACK_OFFSET + 0xe0(1)
    std 29, REGISTER_STACK_OFFSET + 0xe8(1)
    std 30, REGISTER_STACK_OFFSET + 0xf0(1)
    std 31, REGISTER_STACK_OFFSET + 0xf8(1)

load_prx_module:
    # lv2 syscall -> sys_prx_load_module(prx_name_string, 0, NULL)
    lis 3, PRX_NAME_STRING@h
    ori 3, 3, PRX_NAME_STRING@l
    li 4, 0
    li 5, 0
    li 11, 0x1e0
    sc

start_prx_module:
    # pOpt = r1 + PRX_OPT_STACK
    # pOpt->size = 0x28
    li 11, 0x28
    std 11, PRX_OPT_STACK(1)
    # pOpt->cmd = 1
    li 11, 0x1
    std 11, PRX_OPT_STACK + 0x8(1)
    # pOpt->entry = -1;
    li 11, -1
    std 11, PRX_OPT_STACK + 0x10(1)
    # r3 is prx_id, returned from sys_prx_load_module
    # lv2 syscall -> sys_prx_start_module(prx_id, 0, pOpt)
    extsw 3, 3
    li 4, 0
    addi 5, 1, 0x40
    li 11, 0x1e1
    sc

run_prx_entry:
    ld 11, 0x50(1)
    cmpdi 11, -1
    beq finish # failed to load?
    # jump to pOpt->entry()
    rldicl 9, 11, 0x0, 0x20
    lwz 0, 0x0(9)
    lwz 2, 0x4(9)
    mtctr 0
    bctrl

finish:
    # Restore registers
    ld 0, REGISTER_STACK_OFFSET(1)
    ld 1, REGISTER_STACK_OFFSET + 0x8(1)
    ld 2, REGISTER_STACK_OFFSET + 0x10(1)
    ld 3, REGISTER_STACK_OFFSET + 0x18(1)
    ld 4, REGISTER_STACK_OFFSET + 0x20(1)
    ld 5, REGISTER_STACK_OFFSET + 0x28(1)
    ld 6, REGISTER_STACK_OFFSET + 0x30(1)
    ld 7, REGISTER_STACK_OFFSET + 0x38(1)
    ld 8, REGISTER_STACK_OFFSET + 0x40(1)
    ld 9, REGISTER_STACK_OFFSET + 0x48(1)
    ld 10, REGISTER_STACK_OFFSET + 0x50(1)
    ld 11, REGISTER_STACK_OFFSET + 0x58(1)
    ld 12, REGISTER_STACK_OFFSET + 0x60(1)
    ld 13, REGISTER_STACK_OFFSET + 0x68(1)
    ld 14, REGISTER_STACK_OFFSET + 0x70(1)
    ld 15, REGISTER_STACK_OFFSET + 0x78(1)
    ld 16, REGISTER_STACK_OFFSET + 0x80(1)
    ld 17, REGISTER_STACK_OFFSET + 0x88(1)
    ld 18, REGISTER_STACK_OFFSET + 0x90(1)
    ld 19, REGISTER_STACK_OFFSET + 0x98(1)
    ld 20, REGISTER_STACK_OFFSET + 0xa0(1)
    ld 21, REGISTER_STACK_OFFSET + 0xa8(1)
    ld 22, REGISTER_STACK_OFFSET + 0xb0(1)
    ld 23, REGISTER_STACK_OFFSET + 0xb8(1)
    ld 24, REGISTER_STACK_OFFSET + 0xc0(1)
    ld 25, REGISTER_STACK_OFFSET + 0xc8(1)
    ld 26, REGISTER_STACK_OFFSET + 0xd0(1)
    ld 27, REGISTER_STACK_OFFSET + 0xd8(1)
    ld 28, REGISTER_STACK_OFFSET + 0xe0(1)
    ld 29, REGISTER_STACK_OFFSET + 0xe8(1)
    ld 30, REGISTER_STACK_OFFSET + 0xf0(1)
    ld 31, REGISTER_STACK_OFFSET + 0xf8(1)

    # Empty the stack
    addi 1, 1, 0x150

    # We'll write a custom jump later
    nop
    nop
    nop
    nop
