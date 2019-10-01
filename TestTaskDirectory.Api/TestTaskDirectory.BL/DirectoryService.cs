using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestTaskDirectory.Api.Controllers;

namespace TestTaskDirectory.Api.TestTaskDirectory.BL
{
    public class DirectoryService
    {
          FileInfo[] fi;
        IEnumerable<IGrouping<object, FileInfo>> fiGrouped; 
        public DirectoryService GetFilesInfo(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            fi = di.GetFiles("*.*", SearchOption.AllDirectories);
           return this;
        }

        string tree = "";
        int shift = 1;
        public string Tree {
            get {return this.tree;  }
        }
        List<string> treeAsEnumerable = new List<string>();
        public IEnumerable<string> TreeAsEnumerable
        {
            get {return this.treeAsEnumerable; }
        }
       
        
            public DirectoryService FileTree(string path)
        {
            
            DirectoryInfo di = new DirectoryInfo(path);
            for (int i = 0; i < shift; i++)
            {
                tree += " ";
            }
            tree += di.Name;
            tree += Environment.NewLine;
            shift += 2;
            foreach (var item in di.GetFiles())
            {                
                for (int i = 0; i < shift; i++)
                {
                    tree += " ";
                }
                tree += item.Name;
                tree += Environment.NewLine;
            }
            foreach (var item in di.GetDirectories())
            {
                    FileTree(item.FullName);
                shift -= 2;
            }
            return this;

        }
        int _depth;
        private bool firstEnter = true;
        int currLevel = 1;

        public DirectoryService FileTreeOnDepth(string path,int depth)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (depth == currLevel)
            {
                foreach (var i in di.GetFiles())
                {

                    treeAsEnumerable.Add(i.FullName);
                }


            }
            foreach (var item in di.GetDirectories())
            {
                currLevel++;
               
                    FileTreeOnDepth(item.FullName, depth);
                currLevel--;

            }
         
            return this;

        }
        public List<GroupedFilesResult> OrderedInGroups(FileProperty fileProperty)
        {
            List<GroupedFilesResult> ls = new List<GroupedFilesResult>();
            switch (fileProperty)
            {
                case FileProperty.Extension:
                    foreach (var item in fiGrouped)
                    {
                        GroupedFilesResult groupedFilesResult = new GroupedFilesResult();
                        groupedFilesResult.Files = new List<string>();
                        groupedFilesResult.Key = item.Key;
                        foreach (var i in item.OrderBy(o => o.Extension))
                        {
                            ((List<string>)groupedFilesResult.Files).Add(i.FullName);
                        }
                        ls.Add(groupedFilesResult);
                    }
                    return ls;
                case FileProperty.Name:
                    foreach (var item in fiGrouped)
                    {
                        GroupedFilesResult groupedFilesResult = new GroupedFilesResult();
                        groupedFilesResult.Files = new List<string>();
                        groupedFilesResult.Key = item.Key;
                        foreach (var i in item.OrderBy(o => o.Name))
                        {
                            ((List<string>)groupedFilesResult.Files).Add(i.Name);
                        }
                        ls.Add(groupedFilesResult);
                    }
                    return ls;
                case FileProperty.LastChanged:
                    foreach (var item in fiGrouped)
                    {
                        GroupedFilesResult groupedFilesResult = new GroupedFilesResult();
                        groupedFilesResult.Files = new List<string>();
                        groupedFilesResult.Key = item.Key;
                        foreach (var i in item.OrderBy(o => o.LastWriteTime))
                        {
                            ((List<string>)groupedFilesResult.Files).Add(i.Name);
                        }
                        ls.Add(groupedFilesResult);
                    }
                    return ls;
                case FileProperty.Size:
                    foreach (var item in fiGrouped)
                    {
                        GroupedFilesResult groupedFilesResult = new GroupedFilesResult();
                        groupedFilesResult.Files = new List<string>();
                        groupedFilesResult.Key = item.Key;
                        foreach (var i in item.OrderBy(o => o.Length))
                        {
                            ((List<string>)groupedFilesResult.Files).Add(i.Name);
                        }
                        ls.Add(groupedFilesResult);
                    }
                    return ls;
                default: return ls;
            }
        }
        List<string> ArrayInit(FileInfo[] orderedFileInfos)
        {
            List<string> vs = new List<string>();
            foreach (var item in orderedFileInfos)
            {
                vs.Add(item.FullName);
            }
            return vs;
        }

        public DirectoryService GroupedBy(FileProperty fileProperty)
        {
            switch (fileProperty)
            {
                case FileProperty.Extension:
                    fiGrouped = from f in fi
                            group f by f.Extension into fGroup
                            select fGroup;
                    return this;
                case FileProperty.LastChanged:
                    fiGrouped = from f in fi
                                group f by (object)f.LastWriteTime into fGroup
                                select fGroup;
                    return this;
                case FileProperty.Name:
                    fiGrouped = from f in fi
                                group f by f.Name into fGroup
                                select fGroup;
                    return this;
                case FileProperty.Size:
                    fiGrouped = from f in fi
                                group f by (object)f.Length into fGroup
                                select fGroup;
                    return this;
            }
            fiGrouped = from f in fi
                        group f by f.Name into fGroup
                        select fGroup;
            return this;

        }
            public List<string> OrderedBy(FileProperty fileProperty) 
        {
            switch (fileProperty)
            {
                case FileProperty.Extension:
                    return ArrayInit(fi.OrderBy(o => o.Extension).ToArray());
                case FileProperty.LastChanged:
                    return ArrayInit(fi.OrderBy(o => o.LastWriteTime).ToArray());
                case FileProperty.Name:
                    return ArrayInit(fi.OrderBy(o => o.Name).ToArray());
                case FileProperty.Size:
                    return ArrayInit(fi.OrderBy(o => o.Length).ToArray());                    
            }
            return ArrayInit(fi.OrderBy(o => o.Name).ToArray());
        }
    }
}
