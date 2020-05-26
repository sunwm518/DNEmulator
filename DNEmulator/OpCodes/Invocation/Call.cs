﻿using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Linq;

namespace DNEmulator.OpCodes.Invocation
{
    public class Call : IOpCode
    {
        public Code Code => Code.Call;

        public EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IMethod iMethod))
                throw new InvalidILException(ctx.Instruction.ToString());

            var method = (iMethod is MethodDef methodDef) ? methodDef : iMethod.ResolveMethodDef();
            var emulator = new Emulator(method, ctx.Stack.Pop(method.Parameters.Count).Reverse());
            emulator.Emulate();

            if (method.ReturnType.ElementType != ElementType.Void)
                ctx.Stack.Push(emulator.ValueStack.Pop());

            return new NormalResult();
        }
    }
}
