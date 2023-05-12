using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}


class BankersAlgorithm
{
    static bool IsSafe(List<int> processes, List<int> avail, List<List<int>> maxm, List<List<int>> allot)
    {
        // calculate need matrix
        List<List<int>> need = new List<List<int>>();
        for (int i = 0; i < processes.Count; i++)
        {
            need.Add(new List<int>());
            for (int j = 0; j < avail.Count; j++)
            {
                need[i].Add(maxm[i][j] - allot[i][j]);
            }
        }

        // mark all processes as unfinished
        bool[] finish = new bool[processes.Count];
        for (int i = 0; i < processes.Count; i++)
        {
            finish[i] = false;
        }

        // initialize work and safety sequence
        List<int> work = new List<int>(avail);
        List<int> safe_seq = new List<int>();

        // find an unmarked process whose needs can be satisfied with available resources
        while (Array.IndexOf(finish, false) != -1)
        {
            bool found = false;
            for (int i = 0; i < processes.Count; i++)
            {
                if (!finish[i] && NeedCanBeSatisfied(need[i], work))
                {
                    found = true;
                    finish[i] = true;
                    safe_seq.Add(processes[i]);
                    for (int j = 0; j < avail.Count; j++)
                    {
                        work[j] += allot[i][j];
                    }
                    break;
                }
            }

            // if no such process exists, the system is unsafe
            if (!found)
            {
                return false;
            }
        }

        // if all processes were marked finished, the system is safe
        return true;
    }

    static bool NeedCanBeSatisfied(List<int> need, List<int> work)
    {
        for (int i = 0; i < need.Count; i++)
        {
            if (need[i] > work[i])
            {
                return false;
            }
        }
        return true;
    }
    static void Main(string[] args)
    {
        List<int> processes = new List<int>() { 0, 1, 2, 3, 4 };
        List<int> avail = new List<int>() { 3, 3, 2 };
        List<List<int>> maxm = new List<List<int>>() {
        new List<int>() { 7, 5, 3 },
        new List<int>() { 3, 2, 2 },
        new List<int>() { 9, 0, 2 },
        new List<int>() { 2, 2, 2 },
        new List<int>() { 4, 3, 3 },
    };
        List<List<int>> allot = new List<List<int>>() {
        new List<int>() { 0, 1, 0 },
        new List<int>() { 2, 0, 0 },
        new List<int>() { 3, 0, 2 },
        new List<int>() { 2, 1, 1 },
        new List<int>() { 0, 0, 2 },
    };

        // calculate current need
        List<List<int>> need = new List<List<int>>();
        for (int i = 0; i < processes.Count; i++)
        {
            need.Add(new List<int>());
            for (int j = 0; j < avail.Count; j++)
            {
                need[i].Add(maxm[i][j] - allot[i][j]);
            }
        }

        // print initial state of the system
        Console.WriteLine("Initial system state:");
        Console.Write("Processes: ");
        foreach (int p in processes)
        {
            Console.Write(p + " ");
        }
        Console.WriteLine();
        Console.Write("Available resources: ");
        foreach (int a in avail)
        {
            Console.Write(a + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Maximum resource need:");
        for (int i = 0; i < maxm.Count; i++)
        {
            Console.Write("\tProcess " + i + ": ");
            foreach (int m in maxm[i])
            {
                Console.Write(m + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Current allocation:");
        for (int i = 0; i < allot.Count; i++)
        {
            Console.Write("\tProcess " + i + ": ");
            foreach (int a in allot[i])
            {
                Console.Write(a + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        // simulate resource requests
        while (true)
        {
            Console.Write("Enter process requesting resource (-1 to exit): ");
            int req_proc = Convert.ToInt32(Console.ReadLine());
            if (req_proc == -1)
            {
                break;
            }

            Console.Write("Enter requested resources: ");
            List<int> req_res = new List<int>(Array.ConvertAll(Console.ReadLine().Split(), int.Parse));

            // check if request can be granted
            bool request_granted = true;
            for (int i = 0; i < avail.Count; i++)
            {
                if (req_res[i] > avail[i] - allot[req_proc][i])
                {
                    request_granted = false;
                    break;
                }
            }

            if (request_granted)
            {
                // simulate granting the request
                for (int i = 0; i < avail.Count; i++)
                {
                    avail[i] -= req_res[i];
                    allot[req_proc][i] += req_res[i];
                    need[req_proc][i] -= req_res[i];
                }

                // check if system is still in a safe state
                bool safe = IsSafe(processes, avail, maxm, allot, need, out List<int> safe_seq);
                if (safe)
                {
                    Console.WriteLine("Request granted");
                    Console.Write("Safe sequence: ");
                    foreach (int s in safe_seq)
                    {
                        Console.Write(s + " ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Request denied (system would enter unsafe state)");
                    Console.WriteLine("Reversing resource allocation...");
                    // reverse the allocation of the request
                    for (int i = 0; i < avail.Count; i++)
                    {
                        avail[i] += req_res[i];
                        allot[req_proc][i] -= req_res[i];
                        need[req_proc][i] += req_res[i];
                    }
                }
            }
            else
            {
                Console.WriteLine("Request denied (not enough resources available)");
            }
        }
    }

    private static bool IsSafe(List<int> processes, List<int> avail, List<List<int>> maxm, List<List<int>> allot, List<List<int>> need, out List<int> safe_seq)
    {
        throw new NotImplementedException();
    }
}