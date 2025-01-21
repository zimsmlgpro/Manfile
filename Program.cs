using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string command;
        string sourceFilePath;
        string destinationDirectory;

        if (args.Length == 0)
        {
            Console.WriteLine("No command-line arguments detected. Enter your command manually.");
            Console.WriteLine("Valid commands: copy, move, delete");
            Console.Write("Enter command: ");
            command = Console.ReadLine()?.ToLower() ?? "";

            if (command == "delete")
            {
                Console.Write("Enter the file path to delete: ");
                sourceFilePath = Console.ReadLine() ?? "";
                DeleteFile(sourceFilePath);
                return;
            }
            else if (command == "copy" || command == "move")
            {
                Console.Write("Enter the source file path: ");
                sourceFilePath = Console.ReadLine() ?? "";

                Console.Write("Enter the destination directory: ");
                destinationDirectory = Console.ReadLine() ?? "";

                if (command == "copy")
                    CopyFile(sourceFilePath, destinationDirectory);
                else
                    MoveFile(sourceFilePath, destinationDirectory);

                return;
            }
            else
            {
                Console.WriteLine("Invalid command. Exiting program.");
                return;
            }
        }
        else
        {
            command = args[0];

            if (command == "delete" && args.Length >= 1)
            {
                //check args[1] args[1].StartsWith("--");

                if (args[1].StartsWith("--"))
                {
                    sourceFilePath = args[2];
                    DeleteFile(sourceFilePath, true);
                }
                else
                {
                    sourceFilePath = args[1];
                    DeleteFile(sourceFilePath);
                }

                return;
            }
            else if ((command == "copy" || command == "move") && args.Length >= 3)
            {
                sourceFilePath = args[1];
                destinationDirectory = args[2];

                if (command == "copy")
                    CopyFile(sourceFilePath, destinationDirectory);
                else
                    MoveFile(sourceFilePath, destinationDirectory);

                return;
            }
            else
            {
                StringBuilder sb = new();
                sb.AppendLine("Invalid arguments provided. Please check your inputs.");
                sb.AppendLine("Usage:");
                sb.AppendLine("  copy <sourceFilePath> <destinationDirectory>");
                sb.AppendLine("  move <sourceFilePath> <destinationDirectory>");
                sb.AppendLine("  delete <filePath>");
                Console.WriteLine(sb.ToString());
                return;
            }
        }
    }

    static void CopyFile(string source, string destination)
    {
        try
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Source file not found: {source}");
                return;
            }

            string filename = Path.GetFileName(destination).Length > 0 ? Path.GetFileName(destination) : Path.GetFileName(source);
            string destinationDir = Path.GetDirectoryName(destination) ?? ""; 
            string destinationFilePath = Path.Combine(destinationDir, filename);

            if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            File.Copy(source, destinationFilePath, overwrite: true);
            Console.WriteLine($"Copied file: {source} to {destinationFilePath}");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Directory Not Found Error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO Error: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Permission Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during copy operation: {ex.Message}");
        }
    }

    //static void Copy(string source, string destination)
    //{
    //    try
    //    {
    //        string filename = Path.GetFileName(destination).Length > 0 ? Path.GetFileName(destination) : Path.GetFileName(source);
    //        string destinationFilePath = Path.Combine(destination, filename);
    //        File.Copy(source, destinationFilePath, overwrite: true);
    //        Console.WriteLine($"Copied file: {source} to {destinationFilePath}");
    //    }
    //    catch (DirectoryNotFoundException ex)
    //    {
    //        Console.WriteLine($"Directory Not Found Exception Error: {ex.Message}");
    //    }
    //    catch (IOException ex)
    //    {
    //        Console.WriteLine($"IO Exception Error: {ex.Message}");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error during copy operation: {ex.Message}");
    //    }
    //}

    static void MoveFile(string source, string destination)
    {
        try
        {
            if (!File.Exists(source))
            {
                Console.WriteLine($"Source file not found: {source}");
                return;
            }

            string destinationDir = Path.GetDirectoryName(destination);
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            string destinationFilePath = Path.Combine(destinationDir, Path.GetFileName(source));
            if (File.Exists(destinationFilePath))
            {
                Console.WriteLine($"Destination file {destinationFilePath} already exists. Overwriting.");
                File.Delete(destinationFilePath);
            }
            Directory.Move(source, destinationFilePath);
            Console.WriteLine($"Moved file: {source} to {destinationFilePath}");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Directory Not Found Error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O Error: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Permission Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during move operation: {ex.Message}");
        }
    }


    static void DeleteFile(string path, bool recursive = false)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Console.WriteLine($"Deleted file: {path}");
            }
            else if (Directory.Exists(path))
            {
                if (!recursive)
                {
                    Directory.Delete(path);
                    Console.WriteLine($"Deleted empty directory: {path}");
                }
                else
                {
                    Directory.Delete(path, true);
                    Console.WriteLine($"Deleted directory and its contents: {path}");
                }
            }
            else
            {
                throw new IOException($"Directory or file does not exist {path}");
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Permission error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during delete operation: {ex.Message}");
        }
    }


    static void CopyDirectory(string sourceDir, string destinationDir)
    {
        try
        {
            Directory.CreateDirectory(destinationDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                File.Copy(file, destinationFilePath, overwrite: true);
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDir);
                string destinationSubDirPath = Path.Combine(destinationDir, subDirName);
                CopyDirectory(subDir, destinationSubDirPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error copying directory: {ex.Message}");
        }
    }
}
