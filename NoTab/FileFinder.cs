/*
 * hi, my job is to take a list of folders and a file pattern, and return matches.
 * What I need:
 * 1 - The folders you want to search
 * 2 - The pattern you want to match
 * 3 - Whether to be recursive or not
 * 
 * Optional:
 * 1 - You can also tell me a pattern you'd like to **exclude**
 * 
 * Results get dumped into "Results"
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Findit
{
    class FileFinder
    {
        public Boolean recurse = true;
        public string rootfolder = "";
        public string includepattern = "*.*";
        public string excludepattern = "";
        public List<string> Results = new List<string>();

        public FileFinder(string directory, Boolean recursive, string matchpattern, string exclpattern)
        {
            rootfolder = directory;
            recurse = recursive;
            includepattern = matchpattern;
            excludepattern = exclpattern;
        }

        public void Search()
        {
            System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(rootfolder);
            System.IO.FileInfo[] IncludedFiles = folder.GetFiles(includepattern);                
            System.IO.FileInfo[] ExcludedFiles = folder.GetFiles(excludepattern);
            foreach (System.IO.FileInfo inclFile in IncludedFiles)
            {
                if (!IsFileInFileArray(ref ExcludedFiles, inclFile))
                {
                    Results.Add(inclFile.FullName);
                }
            }

            if (recurse)
            {
                List<System.IO.DirectoryInfo> foldersToSearch = new List<System.IO.DirectoryInfo>();
                foreach(System.IO.DirectoryInfo dir in folder.GetDirectories())
                {
                    FileFinder f = new FileFinder(dir.FullName, true, includepattern, excludepattern);
                    f.Search();
                    foreach (string s in f.Results)
                    {
                        Results.Add(s);
                    }
                }
            }
        }

        private Boolean IsFileInFileArray(ref System.IO.FileInfo[] arry, System.IO.FileInfo f)
        {
            foreach (System.IO.FileInfo fileelement in arry)
            {
                if (fileelement.Name == f.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
