using System;
using System.Collections.Generic;

public class Menger {
    public string data;
    public string name;
    
    // data format:
    // ...previous entries, entrySeparator, parentItem, itemSeparator, "data to store"...
    // eg. root/"314"!!!/root/"hello"/"hi"!!!/root/"hello"

    
    public const string entrySeparator = "!!!/";
    public const string itemSeparator = "/"; //i don't think i'm gonna change this, but nice to abstract i guess. forward slash is a standard separator and changing it would be unconventional.
    public const int maxLogMessageRepeats = 1000;


    public Menger(string inputName) {
        data = "root" + itemSeparator + "default";
        name = inputName;
        addItem("root", "log");
    }
    
    public string[] dataList() {
        return data.Split(entrySeparator); //method might seem redundant but it used to be different with a lot more code and now i'm too lazy to remove all the calls that are already in place
    }

    private int alphabeticValue(char c) {
        return (int)(Char.ToLower(c));
    }

    //method alias
    private int lowercaseAscii(char c) {
        return (int)(Char.ToLower(c));
    }

    private string alphabeticallyFirst(string s1, string s2) {
        string shorterString = s1;
        if (s2.Length < shorterString.Length) {
            shorterString = s2;
        }
        int i = 0;
        while (i < shorterString.Length) {
            if (alphabeticValue(s1[i]) < alphabeticValue(s2[i])) {
                return s1;
            }
            else if (alphabeticValue(s2[i]) < alphabeticValue(s1[i])) {
                return s2;
            }
        }
        return shorterString;
    }

    private string[] alphabeticalSort(string[] words) {
        bool sorted = false;
        while (!sorted && words.Length >= 2) {
            sorted = true;
            for (int i = 0; i < words.Length - 1; i++) {
                string lesserWord = alphabeticallyFirst(words[i], words[i + 1]);
                if (words[i] != lesserWord) {
                    sorted = false;
                    words[i + 1] = words[i];
                    words[i] = lesserWord;
                }
            }
        }
        return words;
    }

    //method alias
    private string[] sortAlphabetically(string[] words) {
        return alphabeticalSort(words);
    }



    public string[] organize() {
        dataList = data.split(entrySeparator);
        string[,] dataListSplit = new();
        for (int i = 0; i < dataList.Length; i++) {
            dataListSplit[i] = dataList[i].split(itemSeparator);
        }
        List<string> sortedEntries = new List<string>();

        List<string> entriesByLevel = new List<string>(); //stores all the entries of one layer at index zero, two layer at index one, etc. root counts as a layer.

        foreach (string[] entry in dataListSplit) {
            while (entriesByLevel.Count() < entries.Length) {
                
            }
        }
    }



    

    public bool pathExists(string path) { //somewhat redundant method, but nice to abstract
        string[] thisDataList = dataList();
        foreach (string entry in thisDataList) {
            if (entry.Length >= path.Length) {
                if (path.Equals(entry.Substring(0, path.Length))) {
                    return true;
                }
            }
        }
        return false;
    }
    
    public void addItem(string path, dynamic item) {
        if (Convert.ToString(item).Contains(itemSeparator)) {
            string itemString = Convert.ToString(item);
            itemString = itemString.Replace(itemSeparator, " ");
            Console.WriteLine(itemString);
            log("addItem() error: cannot add item '" + itemString + "' because it contains the item separator");
            return;
        }
        
        string newEntry = path + itemSeparator + Convert.ToString(item);
        
        if (pathExists(path)) {
            if (pathExists(newEntry)) {
                int i = 1;
                string numberedNewEntry = "";
                while (pathExists(numberedNewEntry) || numberedNewEntry.Length < 1) {
                    numberedNewEntry = entrySeparator + newEntry + ' ' + i.ToString();
                }
                data += entrySeparator + numberedNewEntry;
            } else {
                data += entrySeparator + newEntry;
            }
        }
    }

    public void addPath(string path) {
        if (path.Length < 5) {
            log("addPath() error: unusable path (paths must start with root)");
        }
        if (path.Substring(0, 5) != "root/") {
            path = "root/" + path;
        }
        if (pathExists(path)) {
            log("addPath() error: path " + path + " already exists.");
        } else {
            data += entrySeparator + path;
        }
    }

    public void log(string text) {
        int repeats = 1;
        string originalText = text.Substring(0);
        while (pathExists("root/log/" + text)) {
            text = originalText + '(' + Convert.ToString(repeats) + ')';
            repeats++;
            if (repeats > maxLogMessageRepeats) {
                return;
            }
        }
        addItem("root/log", text);
    }

    public void editItem(string path, dynamic newItem) {
        
        if (Convert.ToString(newItem).Contains(itemSeparator)) {
            string itemString = Convert.ToString(newItem);
            itemString = itemString.Replace(itemSeparator, " ");
            Console.WriteLine(itemString);
            log("addItem() error: cannot edit item to " + itemString + " because it contains the item separator");
            return;
        }
        
        string newEntry = path + itemSeparator + Convert.ToString(newItem);
        
        if (pathExists(path)) {
            if (pathExists(newEntry)) {
                string[] subDirs = openDir(path);
                if (subDirs.Length > 1) {
                    log("editItem() error: " + path + "is a folder, not an item");
                    return;
                } else if (subDirs.Length == 1) {
                    path = path + '/' + subDirs[0];
                    newEntry = path + itemSeparator + Convert.ToString(newItem);
                }
                data.Replace(path, "");
                data += entrySeparator + newEntry;
            } else {
                log("editItem() error: item " + path + " does not exist");
            }
        }
    }

    public string[] openAll(string path = "root") {
        string[] thisDataList = dataList();
        List<string> hasPath = new List<string>();
        foreach (string entry in thisDataList) {
            if (entry.Length >= path.Length) {
                if (path.Equals(entry.Substring(0, path.Length))) {
                    hasPath.Add(entry);
                }
            }
        }
        return hasPath.ToArray();
    }

    public string[] openDir(string path = "root") {
        string[] thisDataList = dataList();
        List<string> inDir = new List<string>();
        foreach (string entry in thisDataList) {
            if (entry.Length > path.Length) {
                if (path.Equals(entry.Substring(0, path.Length))) {
                    if (!entry.Substring(path.Length + 1).Contains(itemSeparator)) {
                        inDir.Add(entry);
                    }
                }
            }
        }
        return inDir.ToArray();
    }

    public string getVal(string parent) {
        string[] children = openDir(parent);
        if (children.Length > 1) {
            log("cannot get value of " + parent + " because it has multiple subdirectories");
        } else {
            return children[0];
        }
        return "";
    }

    public void printAll(string path) {
        string[] rootItems = openAll(path);
        foreach (string item in rootItems) {
            Console.WriteLine(item);
        }
    }

    public void printDir(string path) {
        string[] rootItems = openDir(path);
        foreach (string item in rootItems) {
            Console.WriteLine(item);
        }
    }
}