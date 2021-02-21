using System;
using System.Collections.Generic;
using AppEntry.Machines;

namespace AppEntry
{
    public class Program
    {
        private const char EmptySlotChar = '#';

        public static void Main(string[] args)
        {
            Task1_2();
            // Task2_14();
        }

        private static void Task1_2()
        {
            var transitions = new Dictionary<(string, char), (string, char, Step)>
            {
                { ("q0", EmptySlotChar), ("qN", '0', 0) },
                { ("q0", 'a'), ("qWte", '0', Step.Forward) },
                { ("q0", 'b'), ("qWte", '0', Step.Forward) },
                { ("q0", '1'), ("qCheck", '1', Step.Forward) },
                { ("q0", '0'), ("qCheck", '1', Step.Forward) },
                { ("qCheck", EmptySlotChar), ("qY", EmptySlotChar, Step.OnCurrent) },
                { ("qCheck", 'a'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qCheck", 'b'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qCheck", '1'), ("qCheck", EmptySlotChar, Step.Forward) },
                { ("qCheck", '0'), ("qCheck", EmptySlotChar, Step.Forward) },
                { ("qWte", EmptySlotChar), ("qGoToN", EmptySlotChar, Step.Backward) },
                { ("qWte", 'a'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qWte", 'b'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qWte", '1'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qWte", '0'), ("qWte", EmptySlotChar, Step.Forward) },
                { ("qGoToN", EmptySlotChar), ("qGoToN", EmptySlotChar, Step.Backward) },
                { ("qGoToN", '0'), ("qN", '0', Step.OnCurrent) },
                { ("qGoToN", '1'), ("qN", '0', Step.OnCurrent) }
            };

            var machine = new BooleanMachine(new[] {'a', 'b', '0', '1'}, transitions);
            Console.WriteLine(machine.Process("ab0b10b0b1"));
            Console.WriteLine(machine.Process("1010001010111"));
        }

        private static void Task2_14()
        {
            var transitions = new Dictionary<(string, char), (string, char, Step)>
            {
                { ("q0", 'a'), ("qFwd", EmptySlotChar, Step.Forward) },
                { ("q0", 'b'), ("qY", 'b', Step.OnCurrent) },
                { ("qFwd", 'a'), ("qFwd", 'a', Step.Forward) },
                { ("qFwd", 'b'), ("qFwd", 'b', Step.Forward) },
                { ("qFwd", '|'), ("qFwd", '|', Step.Forward) },
                { ("qFwd", EmptySlotChar), ("qBwd", '|', Step.Backward) },
                { ("qBwd", 'a'), ("qBwd", 'a', Step.Backward) },
                { ("qBwd", 'b'), ("qBwd", 'b', Step.Backward) },
                { ("qBwd", '|'), ("qBwd", '|', Step.Backward) },
                { ("qBwd", EmptySlotChar), ("q0", 'a', Step.Forward) }
            };

            var machine = new ProceduralMachine(new[] {'a', 'b', '|'}, transitions);
            Console.WriteLine(machine.Process("bbababaabbab"));
            Console.WriteLine(machine.Process("aaabbab"));
            Console.WriteLine(machine.Process("abbbbaabba"));
        }
    }
}
