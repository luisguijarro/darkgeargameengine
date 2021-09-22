using System;
using System.IO;

namespace dge.GUI.Dialogs
{
    internal class ParentDirectoryInfo
    {
        private DirectoryInfo di;
        internal ParentDirectoryInfo(DirectoryInfo DI)
        {
            this.di = DI;
        }
        public override string ToString()
        {
            return "../";
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return this.di; }
        }

        public string FullName
        {
            get { return this.di.FullName; }
        }
    }
}