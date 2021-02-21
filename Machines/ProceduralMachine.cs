using System.Collections.Generic;

namespace AppEntry.Machines
{
    public class ProceduralMachine : BaseMachine<string>
    {
        public ProceduralMachine(char[] allowedChars, Dictionary<(string, char), (string, char, Step)> transitions, char emptySlotChar = '#') :
            base(allowedChars, transitions, emptySlotChar) { }

        public override string Process(string sourceString) => new string(Run(sourceString).Item1);
    }
}
