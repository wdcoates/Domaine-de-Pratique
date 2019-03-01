﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace ConsoleA1._29_Core_XAML
{
    public class _Main
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine($"This is all about XAML Core!");

            //Windows using Code Only
            var b = new System.Windows.Controls.Button { Content = @"What a long way to get a fucking button!", IsCancel=true};

            var w = new Window {Title = "Code First!", Content = b};


            var app = new Application();
            app.Run(w);

            Console.Write($"Thank you.");
            Console.ReadLine();
        }
    }
}
