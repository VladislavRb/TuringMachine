using System;
using System.Collections.Generic;

namespace AppEntry.Machines
{
    public class BooleanMachine : BaseMachine<bool>
    {
        public BooleanMachine(char[] allowedChars, Dictionary<(string, char), (string, char, Step)> transitions, char emptySlotChar = '#') :
            base(allowedChars, transitions, emptySlotChar)
        { }

        public override bool Process(string sourceString) => GetResultFromState(Run(sourceString).Item2);

        private bool GetResultFromState(string state)
        {
            switch (state)
            {
                case "qY":
                    return true;
                case "qN":
                    return false;
                default:
                    throw new Exception("Final state of boolean machine must be qY or qN");
            }
        }
    }
}
