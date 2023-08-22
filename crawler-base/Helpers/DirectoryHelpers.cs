using System;
using System.IO;
using System.Reflection;

namespace crawler_base.Helpers
{
    public static class DirectoryHelpers
    {
        public static string GetCurrentSolutionDirectory()
        {
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;

            // This will get the current solution directory
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;

            return projectDirectory;
        }
    }
}