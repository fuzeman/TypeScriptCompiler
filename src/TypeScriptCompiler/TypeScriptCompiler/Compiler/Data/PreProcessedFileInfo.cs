using System;
using System.Collections.Generic;

namespace TypeScript.Compiler.Data
{
    public class PreProcessedFileInfo
    {
        public Dictionary<string, object> Settings { get; private set; }
        public List<FileReference> ReferencedFiles { get; private set; }
        //public List<object> ImportedFiles { get; private set; }
        public bool IsLibFile { get; private set; }

        public static PreProcessedFileInfo FromJavascriptResult(Dictionary<string, object> jsResult)
        {
            var result = new PreProcessedFileInfo
            {
                Settings = (Dictionary<string, object>) jsResult["settings"],
                IsLibFile = (bool)jsResult["isLibFile"],
                ReferencedFiles = new List<FileReference>()
            };

            // TODO: importedFiles
            var importedFiles = (object[])jsResult["importedFiles"];
            if(importedFiles.Length > 0)
                throw new NotImplementedException("\"importedFiles\" Not Supported");

            var referencedFiles = (object[])jsResult["referencedFiles"];

            foreach (var jsReferenceFile in referencedFiles)
            {
                result.ReferencedFiles.Add(FileReference.FromJavascriptResult(
                    (Dictionary<string, object>) jsReferenceFile));
            }

            return result;
        }
    }
}
