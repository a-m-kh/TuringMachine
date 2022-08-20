using System;
using System.Collections.Generic;

namespace TouringMachine
{
    class State
    {
        public string Name = "";
        public List<Edge> Edgies = new List<Edge>();
        public bool final = false;
        public State(string n)
        {
            Name = n;
        }
        public State() { }
    }
    class Edge
    {
        public State Now = new State();
        public State After = new State();
        public bool Movie = false;
        public string TapeReed = "";
        public string TapeWrite = "";
        public Edge(State N, State A, bool M, string TR, string TW)
        {
            Now = N;
            After = A;
            Movie = M;
            TapeReed = TR;
            TapeWrite = TW;
        }
    }
    class Program
    {
        public static List<State> CreateState(string[] Staties)
        {
            int Maximum = 0;
            List<State> staties = new List<State>();
            for (int i = 0; i < Staties.Length; i++)
            {
                string str = Staties[i];
                string[] separate = str.Split(new string[] { "0" }, StringSplitOptions.None);
                if (separate[0].Length > Maximum)
                {
                    Maximum = separate[0].Length;
                }
                if (separate[2].Length > Maximum)
                {
                    Maximum = separate[2].Length;
                }
                if (staties.Find(a => a.Name == separate[0]) == null)
                {
                    State s = new State(separate[0]);
                    staties.Add(s);
                }
                if (staties.Find(a => a.Name == separate[2]) == null)
                {
                    State s = new State(separate[2]);
                    staties.Add(s);
                }
            }
            for (int i = 0; i < Staties.Length; i++)
            {
                string str = Staties[i];
                string[] separate = str.Split(new string[] { "0" }, StringSplitOptions.None);
                bool status = false;
                if (separate[4] == "11")
                {
                    status = true;
                }
                Edge E = new Edge(staties.Find(a => a.Name == separate[0]), staties.Find(a => a.Name == separate[2]), status, separate[1], separate[3]);
                staties.Find(a => a.Name == separate[0]).Edgies.Add(E);
            }

            staties.Find(a => a.Name.Length == Maximum).final = true;
            return staties;
        }

        public static bool Transiation(string[] Tape, List<State> staties)
        {
            int pointer = 101;
            string[] FinalTape = new string[101 + Tape.Length + 101];
            int ind = 0;
            for (int i = 0; i < 101 + Tape.Length + 101; i++)
            {
                if (i < 101)
                {
                    FinalTape[i] = "1";
                }
                if (101 <= i && i < 101 + Tape.Length)
                {
                    FinalTape[i] = Tape[ind];
                    ind++;
                }
                if (i >= 101 + Tape.Length)
                {
                    FinalTape[i] = "1";
                }
            }
            var NowState = staties.Find(a => a.Name == "1");
            var FinalState = staties.Find(a => a.final);
            while (true)
            {
                if (NowState == FinalState)
                {
                    return true;
                }
                var E = NowState.Edgies.Find(a => a.TapeReed == FinalTape[pointer]);
                if (E == null)
                {
                    return false;
                }
                NowState = E.After;
                FinalTape[pointer] = E.TapeWrite;
                if (E.Movie)
                {
                    pointer++;
                }
                else
                {
                    pointer--;
                }
            }
        }
        static void Main(string[] args)
        {
            string touring = Console.ReadLine();
            string[] Staties = touring.Split(new string[] { "00" }, StringSplitOptions.None);
            List<State> staties = CreateState(Staties);
            int number = int.Parse(Console.ReadLine());
            for (int i = 0; i < number; i++)
            {
                string str = Console.ReadLine();
                if (str == "")
                {
                    str = "1";
                }
                string[] strTape = str.Split(new string[] { "0" }, StringSplitOptions.None);
                if (Transiation(strTape, staties))
                {
                    Console.WriteLine("Accepted");
                }
                else
                {
                    Console.WriteLine("Rejected");
                }
            }

        }
    }
}
