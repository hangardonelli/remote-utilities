using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;


namespace client_server
{
    [Serializable]
    class ClipboardManager
    {
        public byte[] GetClipboardData()
        {
            DataObject data = Clipboard.GetDataObject() as DataObject;
            if (data == null || !data.GetDataPresent(typeof(Byte[])))
                return null;
            return data.GetData(typeof(Byte[])) as Byte[];
        }

        public void SetClipboardData(byte[] data, bool keepInMemory = true)
        {

            DataObject dataContainer = new DataObject();
            dataContainer.SetData(typeof(Byte[]), data);
            Clipboard.SetDataObject(data, keepInMemory);
        }
    }
}


