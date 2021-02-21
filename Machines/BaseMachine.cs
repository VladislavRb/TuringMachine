using System;
using System.Collections.Generic;
using System.Linq;

namespace AppEntry.Machines
{
    public abstract class BaseMachine<TResult>
    {
        private const int EmptySlotsBefore = 10;
        private const int EmptySlotsAfter = 10;

        private readonly char _emptySlotChar;
        private readonly Dictionary<(string, char), (string, char, Step)> _transitions;

        protected BaseMachine() { }

        protected BaseMachine(char[] allowedChars, Dictionary<(string, char), (string, char, Step)> transitions, char emptySlotChar = '#')
        {
            _emptySlotChar = emptySlotChar;
            var startPoints = transitions.Keys;
            var actions = transitions.Values;

            if (!StatesAreCorrectlyConfigured(
                startPoints.Select(startPoint => startPoint.Item1).ToList(),
                actions.Select(action => action.Item1).ToList()))
            {
                throw new Exception("Transitions are incorrectly configured");
            }

            if (!AllCharsAreAllowed(
                startPoints.Select(startPoint => startPoint.Item2).ToList(),
                actions.Select(action => action.Item2).ToList(),
                allowedChars))
            {
                throw new Exception("Transitions contain characters that are not allowed");
            }

            _transitions = transitions;
        }

        public abstract TResult Process(string sourceString);

        protected (char[], string) Run(string sourceString)
        {
            var tape = InitTape(sourceString);
            var position = EmptySlotsBefore;
            var currentSymbol = tape[position];
            var currentState = "q0";

            while (!(currentState == "qY" || currentState == "qN"))
            {
                if (!_transitions.TryGetValue((currentState, currentSymbol), out var action))
                {
                    throw new Exception($"There is no action corresponding to state {currentState} with symbol {currentSymbol}");
                }

                tape[position] = action.Item2;
                position += (int)action.Item3;
                currentSymbol = tape[position];
                currentState = action.Item1;
            }

            return (tape, currentState);
        }

        private bool StatesAreCorrectlyConfigured(List<string> fromStates, List<string> toStates) =>
            fromStates.Contains("q0") &&
            !(fromStates.Contains("qY") || fromStates.Contains("qN")) &&
            (toStates.Contains("qY") || toStates.Contains("qN"));

        private bool AllCharsAreAllowed(List<char> fromChars, List<char> toChars, char[] allowedChars) =>
            fromChars.TrueForAll(ch => CharIsAllowed(ch, allowedChars)) &&
            toChars.TrueForAll(ch => CharIsAllowed(ch, allowedChars));

        private bool CharIsAllowed(char ch, char[] allowedChars) =>
            ch == _emptySlotChar || allowedChars.Contains(ch);

        private char[] GetEmptySlots(int slotsAmount)
        {
            var slots = new char[slotsAmount];
            for (int i = 0; i < slotsAmount; i++)
            {
                slots[i] = _emptySlotChar;
            }

            return slots;
        }

        private char[] InitTape(string sourceString)
        {
            var tape = new char[EmptySlotsBefore + sourceString.Length + EmptySlotsAfter];

            GetEmptySlots(EmptySlotsBefore).CopyTo(tape, 0);
            sourceString.ToCharArray().CopyTo(tape, EmptySlotsBefore);
            GetEmptySlots(EmptySlotsAfter).CopyTo(tape, EmptySlotsBefore + sourceString.Length);

            return tape;
        }
    }
}
