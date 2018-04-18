﻿using System;

namespace algorithms
{

    class Program
    {
        static void Main(string[] args)
        {
            SnackVendorFactory.RunProblem("astar");
            SnackVendorFactory.RunProblem("dijstra");

            Console.ReadLine();
        }
    }
}