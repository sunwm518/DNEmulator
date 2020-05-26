﻿using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DNEmulator.OpCodes.Fields
{
    public class Ldsfld : IOpCode
    {
        public Code Code => Code.Ldsfld;

        public EmulationResult Emulate(Context ctx)
        {
            if(!(ctx.Instruction.Operand is IField field))
                throw new InvalidILException(ctx.Instruction.ToString());

            if (!field.ResolveFieldDef().IsStatic)
                throw new InvalidILException(ctx.Instruction.ToString());

            ctx.Stack.Push(ctx.Emulator.FieldMap.Get(field));
            return new NormalResult();
        }
    }
}
