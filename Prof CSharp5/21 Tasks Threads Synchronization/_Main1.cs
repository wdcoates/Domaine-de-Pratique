﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Microsoft.Ajax.Utilities;
using static ConsoleA1._00_Common.Statics;

using con = System.Console;

namespace ConsoleA1._21_Tasks_Threads_Synchronization
{
    class _Main1
    {
        public static void Main(string[] args)
        {
            int doCase = 8;

            switch (doCase)
            {
                case 1:
                    con.WriteLine($"Start Looping with Parallel.For!");

                    ParallelLoopResult pRes = Parallel.For(0, 10, i =>
                    {
                        Thread.Sleep(10000 - i * 1000); //Old Way of doing it... 
                        con.WriteLine(
                            $"Iteration {i}, Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                    });

                    con.WriteLine($"First set completed? {pRes.IsCompleted}");
                    break;
                case 2:
                    //Adding the async and wait key words
                    ParallelLoopResult pRes2 = Parallel.For(100, 110, async i =>
                    {
                        try
                        {
                            con.WriteLine(
                                $"Iteration {i}, Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                            await Task.Delay(10000 - i * 10);
                        }
                        catch (Exception e)
                        {
                            con.WriteLine($"Error: {e.Message}!");
                        }

                        con.WriteLine(
                            $"Iteration {i}, Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                    });

                    con.WriteLine($"All Threads completed! {pRes2.IsCompleted}");
                    break;
                
                case 3:
                    //Adding the Break method which dose not quite work like the documentation seems to be telling me! Break must be before await keyword!          
                    var brk = 15;
                    ParallelLoopResult pRes3 = Parallel.For(10, 20, async (i, pLS) =>
                    {
                        if (i >= brk)
                            pLS.Break();

                        try
                        {
                            con.WriteLine(
                                $"3rd Loop Iteration {i}, Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}");
                            await Task.Delay(10);
                        }
                        catch (Exception e)
                        {
                            con.WriteLine($"Iteration {i}, Error: {e.Message}!");
                        }

                        con.WriteLine(
                            $"3rd Loop Iteration {i}, Task: {Task.CurrentId}, Thread: {Thread.CurrentThread.ManagedThreadId}");

                    });

                    con.WriteLine($"3rd Loop - All Threads completed! {pRes3.IsCompleted}");
                    con.WriteLine($"Last iteration: {pRes3.LowestBreakIteration}"); //This is null 
                    break;
                case 4:
                    //Getting funky now...
                    con.WriteLine($"Lets get funky when you are ready hit a key!");
                    con.ReadLine();
                    Parallel.For<string>(0, 2, () =>
                        {
                            // Invoked once for each thread...
                            con.WriteLine(
                                $"Init for thread {Thread.CurrentThread.ManagedThreadId}, task {Task.CurrentId}");

                            return String.Format($"t{Thread.CurrentThread.ManagedThreadId}");   //returns to initStr1.
                        }, (i, pLs, initStr1) =>
                        {
                            // Invoked for each member.
                            con.WriteLine(
                                $"Body i {i}, str {initStr1}, thread {Thread.CurrentThread.ManagedThreadId}, task {Task.CurrentId}");
                            Thread.Sleep(10);
                            return $"i {i}";    //this is the return type for the For<string>
                        },
                        (str1) =>   //exit method
                        {
                            //Final action on each thread
                            con.WriteLine($"Finally {str1}");
                        });

                    con.WriteLine($"What happened there!");
                    break;
                case 5:
                    //Looping with the Parallel.ForEach
                    string[] sample =
                    {
                        "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf", "dix", "onze",
                        "douze", "trize", "quatorze", "quinze", "seize"
                    };
                    ParallelLoopResult plRes = Parallel.ForEach<string>(sample, (s, spl, lItr) => {con.WriteLine($"{lItr} - {s}"); });
                    break;

                case 6:
                    // Multiple Methods with Parallel.Invoke
                    try
                    {
                        Parallel.Invoke(bonjour("Jane"),bonjour("Paul"),bonjour("Jon"), bienvenue("Jane"), au_revoir("Jon"));
                    }
                    catch (Exception e)
                    {     
                        con.WriteLine($"{e.Message} - Actions should be null!");
                    }
                    break;
                case 7:
                    //Starting Tasks 1. TaskFactory 2. Factory via class 3. Task Constructor 4. Run method 
                    var tf = new TaskFactory();
                    Task t1 = tf.StartNew(Tasks.TaskMethod, "Using a TaskFactory Object!");
                  
                    Task t2 = Task.Factory.StartNew(Tasks.TaskMethod, "Using the Factory via a Task class");
                    
                    var t3 = new Task(Tasks.TaskMethod, "Using a Task constructor and the Start method");
                    t3.Start();

                    Task.Run(() => Tasks.TaskMethod("Using the Run method and Lambda syntax"));
                    
                    break;

                case 8:
                    //Now with TaskCreation Options
                    Tasks.TaskMethod("Now sure this will work!");
                    var t4 = new Task(Tasks.TaskMethod, "Running on new Thread.");
                    t4.Start();
                    Tasks.TaskMethod("What Thread Am I?");
                    var t5 = new Task(Tasks.TaskMethod, "Running Synchronously on the same Thread.");
                    t5.RunSynchronously();

                    //What if we know a Task is going to take some time!
                    var lT1 = new Task(Tasks.TaskMethod, "A Long Runner!", TaskCreationOptions.LongRunning);
                    lT1.Start();
                    Thread.Sleep(10);
                    if (lT1.Status == TaskStatus.RanToCompletion)
                        lT1.Dispose();
                        // lT1.RunSynchronously(); can't do this !

                    //Futures because the result it going to be in the future!
                    var rT1 = new Task<Tuple<int, int>>(Tasks.TaskWithRes, Tuple.Create<int, int>(19, 3));
                    rT1.Start();
                    rT1.Wait();
                    con.WriteLine(rT1.Result);                    
                    con.WriteLine($"Results: Res = {rT1.Result.Item1}; Rem = {rT1.Result.Item2}");                                        
                    break;
                default:
                    break;
            }

            con.ReadLine();
        }
    
      
    }
    
}
