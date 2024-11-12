using System;
using System.Threading;

public class Program{
    public static string mainThreadName;

    public static void Main (){
        mainThreadName = Thread.CurrentThread.Name;
        new Login();
    }
}