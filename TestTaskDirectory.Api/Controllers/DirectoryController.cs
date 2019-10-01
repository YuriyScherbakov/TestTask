using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TestTaskDirectory.Api.Controllers
{
    [Route("directory")]
    public class DirectoryController : Controller
    {
        public DirectoryController()
        {
            
        }

        /// <summary>
        /// Implement api endpoint that returns list of file full names in selected directory (including sub-directories).
        /// File names should be ordered by one of the file properties <see cref="FileProperty"/>.
        /// </summary>
        /// <returns>Array of file names.</returns>
        [HttpPost("files")]
        public ActionResult<IEnumerable<string>> GetDirectoryFiles([FromBody]FilesRequest request)
        {
            TestTaskDirectory.BL.DirectoryService directoryService =
                new TestTaskDirectory.BL.DirectoryService();
           
           return directoryService.GetFilesInfo(request.Directory)
                .OrderedBy(request.OrderFilesBy);
        }

        /// <summary>
        /// Implement api endpoint that groups files of selected directory (including sub-directories)
        /// by one the file properties <see cref="FileProperty"/>.
        /// File names in every group should be ordered by one of the file properties <see cref="FileProperty"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("grouped-files")]
        public ActionResult<IEnumerable<GroupedFilesResult>> GetGroupedFiles([FromBody]FilesGroupingRequest request)
        {
            TestTaskDirectory.BL.DirectoryService directoryService =
                new TestTaskDirectory.BL.DirectoryService();

           return directoryService.GetFilesInfo(request.Directory)
                .GroupedBy(request.GroupFilesBy)
                .OrderedInGroups(request.OrderFilesBy);
            
        }

        /// <summary>
        /// Implement api endpoint that builds a string that represents content of selected directory.
        /// Every nested level should have 2 space padding from previous level:
        /// 
        /// TestFiles
        ///   Dir1
        ///     file1.txt
        ///     Dir2
        ///       file1.dll
        ///       file2.dll
        ///     Dir3
        ///       Dir4
        ///         Dir5
        ///           file1.cs
        ///           file2.cs
        ///           file3.cs
        /// </summary>
        [HttpPost("file-tree")]
        public ActionResult<string> FileTree([FromBody]DirectoryRequest request)
        {
            TestTaskDirectory.BL.DirectoryService directoryService =
               new TestTaskDirectory.BL.DirectoryService();
            return directoryService.FileTree(request.Directory)
                .Tree;
        }

        /// <summary>
        /// Implement api endpoint that returns files only on specified depth of selected directory.
        /// For specified structure it should return file1.txt if you passed 2 and file1.dll, file2.dll if you passed 3.
        ///
        /// TestFiles
        ///   Dir1
        ///     file1.txt
        ///     Dir2
        ///       file1.dll
        ///       file2.dll
        ///     Dir3
        ///       Dir4
        ///         Dir5
        ///           file1.cs
        ///           file2.cs
        ///           file3.cs
        /// </summary>
        [HttpPost("files-on-depth")]
        public ActionResult<IEnumerable<string>> FilesOnDepth([FromBody]FilesOnDepthRequest request)
        {
            TestTaskDirectory.BL.DirectoryService directoryService =
                new TestTaskDirectory.BL.DirectoryService();
            var c = directoryService.FileTreeOnDepth(request.Directory, request.Depth)
                .TreeAsEnumerable.ToArray();
            return c;
        }
    }
}
