using System;
using System.IO;

namespace ManipulateFiles
{
    internal class Program
    {
        static void WritePrompt(string prompt, List<string> options)
        {
           Console.WriteLine(prompt);

           options.ForEach(option => Console.WriteLine(option));

        }
        static void Main(string[] args)
        {
            List<string> introOptions = ["1. Copy", "2. Move"];
            WritePrompt("Would you like to copy or move a file?", introOptions);

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Copy();
                    break;

                case "2":
                    Move();
                    break;
            }
        }

        static void Copy()
        {
            Console.Clear();
            Console.WriteLine("Copy a file");
            Console.WriteLine("Please enter the path of the file that you want to copy: ");
            string sourcePath = Console.ReadLine();

            Console.WriteLine("Enter the destination path: ");
            string destinationPath = Console.ReadLine();

            try
            {
                File.Copy(sourcePath, destinationPath, overwrite: true);
                Console.WriteLine("File copied successfully.");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void Move()
        {
            Console.Clear();
            Console.WriteLine("Move a file");
            Console.WriteLine("Please enter the path of the file that you want to move: ");
            string sourcePath = Console.ReadLine();

            Console.WriteLine("Enter the destination path: ");
            string destinationPath = Console.ReadLine();

            try
            {
                File.Move(sourcePath, destinationPath, overwrite: true);
                Console.WriteLine("File moved successfully.");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
