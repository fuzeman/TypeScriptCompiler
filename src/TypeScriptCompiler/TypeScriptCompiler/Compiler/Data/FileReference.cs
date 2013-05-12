using System;
using System.Collections.Generic;

namespace TypeScript.Compiler.Data
{
    public class FileReference
    {
        public int MinChar { get; private set; }
        public int LimChar { get; private set; }
        public string Path { get; private set; }
        public bool IsResident { get; private set; }

        public static FileReference FromJavascriptResult(Dictionary<string, object> jsResult)
        {
            return new FileReference
            {
                MinChar = (int) jsResult["minChar"],
                LimChar = (int) jsResult["limChar"],
                Path = (string) jsResult["path"],
                IsResident = (bool) jsResult["isResident"]
            };
        }
    }
}
