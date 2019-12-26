using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoTab
{
  class Program
  {
    static bool noInteract = false;
    static bool infinityBelt = false;

    static void WriteIf(string msg, bool isInteractive = true)
    {
      if (!noInteract || !isInteractive)
      {
        Console.WriteLine(msg);
      }
    }

    static void ReadIf()
    {
      if (!noInteract)
      {
        Console.ReadKey();
      }
    }

    static void Main(string[] args)
    {
      try
      {
        noInteract = (args.Length >= 4 && args[3] == "SILENT");
        infinityBelt = (args.Length >= 5 && args[4]  == "INFINITYBELT");
        WriteIf("Strip tabs from:");
        WriteIf("Files matching this pattern      : " + args[1]);
        WriteIf("In this folder and all subfolders: " + args[0]);
        WriteIf("And tabs will become " + args[2] + " spaces");
        WriteIf("Press ENTER to proceed");

        ReadIf();
        Findit.FileFinder ff = new Findit.FileFinder(args[0], true, "*.xml", string.Empty);
        ff.Search();
        WriteIf((ff.Results == null || ff.Results.Count == 0 ? "No files found" : "Removing tabs from these files"));
        WriteIf("(U=unchanged.  C=at least one tab found)");
        foreach (string f in ff.Results)
        {
          try
          {
            WriteIf(StripTabsFromFile(f, int.Parse(args[2])), false);
            //special flag to do processing unique to the InfinityBelt project.
            //i know this is out of place in "NoTab", but I'm doing it to save time.
            //pretty sure nobody else uses this project but me anyway :)
            if (infinityBelt)
            {
              WriteIf(SpecialInfinityBeltProcessing(f));
            }
          }
          catch (Exception fileException)
          {
            WriteIf("============================");
            WriteIf("Error on file " + f);
            WriteIf(fileException.Message);
            WriteIf("============================");
          }
        }
      }
      catch(Exception generalException)
      {
        WriteIf("Error: " + generalException.Message);
      }
      WriteIf(string.Empty);
      WriteIf("Press any key to continue...");
      ReadIf();
    }

    static string SpecialInfinityBeltProcessing(string filename)
    {
      string originalText = File.ReadAllText(filename);
      string newText = originalText.Replace(" />", "/>");
      if (newText == originalText)
      {
        return "U:" + filename;
      }
      else
      {
        File.WriteAllText(filename, newText);//write the new version to disk
        return "IB:" + filename;
      }
    }

    static string StripTabsFromFile(string filename, int spaceCount)
    {
      string replacement = new string(' ', spaceCount);
      string originalText = File.ReadAllText(filename);
      string newText = originalText.Replace("\t", replacement);
      if(newText==originalText )
      {
        return "U:" + filename;
      }
      else
      {
        File.WriteAllText(filename, newText);//write the no-tabs version to disk
        return "C:" + filename;
      }
    }
  }
}
