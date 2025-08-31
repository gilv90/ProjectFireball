using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using ProjectFireball.Instructions;

[GlobalClass]
public partial class InstructionList : Resource
{
    [Export] public Array<Instruction> Instructions { get; set; }
}
