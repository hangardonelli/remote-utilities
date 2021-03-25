using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;


namespace client_server
{

    public abstract class AMessages
    { 
        public virtual void RunMessage(byte[] data)
        {

        }
        public virtual byte[] PrepareMessage(string message)
        {
            return null;
        }


        public const byte OPCODE = 7;
        public const byte SEPARATOR = 36;
    }

    public class ToastNotificationSender : AMessages
    {
        public override void RunMessage(byte[] data)
        {


            byte[] titleBYTES = data.Skip(2)
                        .TakeWhile(x => x != (byte)'%').ToArray();
            string title = Encoding.ASCII.GetString(titleBYTES);


            byte[] messageBYTES = data.SkipWhile(x => x != (byte)'%')
                          .ToArray();
            string message = Encoding.ASCII.GetString(messageBYTES);




            new ToastContentBuilder()
                                     .AddText(title)
                                     .AddText(message)
                                     .Show();
        }
    }
    public class MessageBoxSender : AMessages
    {

        public override void RunMessage(byte[] data)
        {
            byte[] messageARRAY;
            byte[] titleARRAY;
            Func<short, MessageBoxIcon> GetMessageBoxIcon = (code) =>
            {
                switch (code)
                {
                    case 0:
                        return MessageBoxIcon.Information;
                    case 1:
                        return MessageBoxIcon.Warning;
                    case 2:
                        return MessageBoxIcon.Error;
                    case 3:
                        return MessageBoxIcon.Question;
                    default:
                        return MessageBoxIcon.None;
                }
            };

            MessageBoxIcon IconType = GetMessageBoxIcon(data[2]);
            
            
            titleARRAY = data.Skip(3)
                        .TakeWhile(x => x != (byte)'%')
                        .ToArray();
            string title = Encoding.ASCII.GetString(titleARRAY);


            messageARRAY = data.SkipWhile(x => x != (byte)'%')
                          .ToArray();
            string message = Encoding.ASCII.GetString(messageARRAY);



            //Actually run the messagebox
            MessageBox.Show(message, title, MessageBoxButtons.OK, IconType);


        }


        public override byte[] PrepareMessage(string message)
        {
            const byte INFORMATION_ICON = 2;
            byte[] prefix = { OPCODE, INFORMATION_ICON };
            byte[] messageBuffer = Encoding.ASCII.GetBytes(message);

            return prefix.Concat(messageBuffer).ToArray();
        }

        public byte[] PrepareMessage(string message, string title, MessageBoxIcon icon)
        {
            Func<MessageBoxIcon, byte> GetCodeOfMessageBoxIcon = (x) =>
            {
                switch (x)
                {
                    case MessageBoxIcon.Information:
                        return 0;
                    case MessageBoxIcon.Warning:
                        return 1;
                    case MessageBoxIcon.Error:
                        return 2;
                    case MessageBoxIcon.Question:
                        return 3;
                    default:
                        return 4;
                }
            };

            byte INFORMATION_ICON = GetCodeOfMessageBoxIcon(icon);
            byte[] SEPARATOR_ARRAY = { SEPARATOR };
            byte[] titleBuffer = Encoding.ASCII.GetBytes(title)
                                                .Concat(SEPARATOR_ARRAY)
                                                .ToArray();

            byte[] messageBuffer = Encoding.ASCII.GetBytes(message);
            byte[] prefix = { OPCODE, INFORMATION_ICON};


            return prefix
                   .Concat(titleBuffer)
                   .Concat(messageBuffer)
                   .ToArray();

        }
    }
}
