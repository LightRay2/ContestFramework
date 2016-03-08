using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class UploadingFileInfo
    {
        static int currentId = 0;
 
         public string fileName = null;
        public int id ;

        public int partCount;
        public byte[][] bytes;
        public UploadingFileInfo(int partCount){
            id = currentId++;
            this.partCount = partCount;
            bytes = new byte[partCount][];
        }

        public bool AddPartAndCheckFinish(byte[] part, int number)
        {
            bytes[number] = part;
            bool finished = bytes.Count(x => x == null) == partCount;
            return finished;
        }
       
    }
}
