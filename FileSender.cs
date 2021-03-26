using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace client_server
{
    public class FileSender
    {
        public void CreateFileFromBytes(byte[] buffer)
        {
            buffer = buffer.Skip(1).ToArray();
            byte[] patchBuffer = buffer.TakeWhile(x => x != 25).ToArray();
            string patch = Encoding.UTF8.GetString(patchBuffer);
            byte[] contentBuffer = buffer.Take(Array.IndexOf(buffer, 25) + 1).ToArray();

            using (FileStream fs = new FileStream(patch, FileMode.Create, FileAccess.Write))
                fs.Write(contentBuffer, 0, contentBuffer.Length);
        }
        public byte[] CreateByteArrayFromFile(string filename)
        {
            byte[] OPCODE = { 4 };
            byte[] patchBuffer = Encoding.UTF8.GetBytes(filename);
            byte[] SEPARATOR = { 25 };
            byte[] fileContentBuffer = File.ReadAllBytes(filename);
            byte[] buffer = OPCODE
                            .Concat(patchBuffer)
                            .Concat(SEPARATOR)
                            .Concat(fileContentBuffer)
                            .ToArray();

            return buffer;
        }
    }
}
