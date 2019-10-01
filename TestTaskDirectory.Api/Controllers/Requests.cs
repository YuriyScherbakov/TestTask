using System.Collections.Generic;

namespace TestTaskDirectory.Api.Controllers
{
    public enum FileProperty
    {
        Name,
        Extension,
        Size,
        LastChanged,
    }

    public class DirectoryRequest
    {
        public string Directory { get; set; }
    }

    public class FilesRequest : DirectoryRequest
    {
        public FileProperty OrderFilesBy { get; set; }
    }

    public class FilesGroupingRequest : FilesRequest
    {
        public FileProperty GroupFilesBy { get; set; }
    }

    public class GroupedFilesResult
    {
        public object Key { get; set; }
        public IEnumerable<string> Files { get; set; }
    }

    public class FilesOnDepthRequest : DirectoryRequest
    {
        public int Depth { get; set; }
    }
}
