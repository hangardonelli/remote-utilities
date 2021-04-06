using System;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;


namespace client_server
{
    /// <summary>
    /// It is the abstract class that all types of messages / notifications 
    /// (not chat) will have in common
    /// </summary>
    public abstract class AMessages
    {
        /// <summary>
        /// Execute the message as determined by the class
        /// </summary>
        /// <param name="data">The buffer</param>
        public virtual void RunMessage(byte[] data)
        {

        }
        /// <summary>
        /// Convert the message to a byte array to be used as a buffer, 
        /// adding extra bytes for additional information
        /// </summary>
        /// <param name="message">The message to be converted</param>
        public virtual byte[] PrepareMessage(string message)
            {
                return null;
            }


        public const byte OPCODE = 7;

        /// <summary>
        /// It is used as a buffer separator, dividing 
        /// it into different fragments to use each of them as appropriate
        /// </summary>
        public const byte SEPARATOR = 25;
    }

    public class ToastNotificationSender : AMessages
    {

        /// <summary>
        /// Run a Toast Notification in Windows 10 with a message title and body
        /// </summary>
        /// <param name="data">The buffer of the message</param>
        public override void RunMessage(byte[] data)
        {
            //Extract the title bytes from the buffer
            byte[] titleBYTES = data.Skip(2).TakeWhile(x => x != 25).ToArray();

            //Convert the bytes into a UTF-8 String
            string title = Encoding.UTF8.GetString(titleBYTES);
          
            //Extract the title bytes from the buffer 
            byte[] messageBYTES = data.SkipWhile(x => x != (int)25).Skip(1).ToArray();

            //Convert the message bytes into a UTF-8 String
            string message = Encoding.UTF8.GetString(messageBYTES);


            //Runs the Toast Notification
            new ToastContentBuilder()
                        .AddText(title)
                        .AddText(message)
                        .Show();
        }


        /// <summary>
        /// Convert the message of the Toast Notification to a byte array to be used as a buffer, 
        /// adding extra bytes for additional information
        /// </summary>
        /// <param name="message">The message to be converted into a buffer</param>
        /// <returns>An array of bytes, whose content is the message converted to buffer</returns>
        public override byte[] PrepareMessage(string message)
        {
            byte[] prefix = { OPCODE, 1 };

            //Putting a default title in this overload
            byte[] titleBytes = Encoding.UTF8.GetBytes("App message").ToArray();

            byte[] SEPARATOR_ARRAY = { SEPARATOR };
            byte[] titleBuffer = titleBytes.Concat(SEPARATOR_ARRAY).ToArray();

            //Encoding the message
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message).ToArray();

            return prefix
                        .Concat(titleBuffer)
                        .Concat(messageBuffer)
                        .ToArray();
        }

        /// <summary>
        ///  Convert the message and the title of the Toast Notification 
        ///  to a byte array to be used as a buffer.
        /// </summary>
        /// <param name="message">The message to be converted into a buffer</param>
        /// <param name="title">The title to be converted into a buffer</param>
        /// <returns>>An array of bytes, whose content is the message & title converted to buffer</returns>
        public byte[] PrepareMessage(string message, string title)
        {
            byte[] prefix = { OPCODE, 1 };

            //Encoding the title
            byte[] titleBytes = Encoding.UTF8.GetBytes(title).ToArray();

            byte[] SEPARATOR_ARRAY = { SEPARATOR };
            byte[] titleBuffer = titleBytes.Concat(SEPARATOR_ARRAY).ToArray();

            //Encoding the message
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message).ToArray();

            return prefix
                        .Concat(titleBuffer)
                        .Concat(messageBuffer)
                        .ToArray();
        }

    
}

    /// <summary>
    /// Class used to send messages with messagebox style
    /// I understand that it can be ironic to write so much to send a messagebox, but do not judge me, because the idea of this is that it can be used from a remote device and that the message is completely customizable.
    ///  Thank you and long live the red beer
    /// </summary>
    public class MessageBoxSender : AMessages
    {
        /// <summary>
        /// Run a messagebox
        /// </summary>
        /// <param name="data">The buffer</param>
        public override void RunMessage(byte[] data)
        {
            byte[] messageARRAY;
            byte[] titleARRAY;

            /*
             * Given a number, return its corresponding MessageBoxIcon enum
             */
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
                        .TakeWhile(x => x != (byte)25)
                        .ToArray();
            string title = Encoding.UTF8.GetString(titleARRAY);


            messageARRAY = data.SkipWhile(x => x != (byte)25)
                          .ToArray();
            string message = Encoding.UTF8.GetString(messageARRAY).Substring(1);
                                          



            //Actually run the messagebox
            MessageBox.Show(message, title, MessageBoxButtons.OK, IconType);


        }


        public override byte[] PrepareMessage(string message)
        {
            const byte INFORMATION_ICON = 2;
            byte[] prefix = { OPCODE, 0, INFORMATION_ICON, SEPARATOR };
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

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
            byte[] titleBuffer = Encoding.UTF8.GetBytes(title)
                                                .Concat(SEPARATOR_ARRAY)
                                                .ToArray();

            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            byte[] prefix = { OPCODE, 0, INFORMATION_ICON};


            return prefix
                   .Concat(titleBuffer)
                   .Concat(messageBuffer)
                   .ToArray();

        }
    }
}


