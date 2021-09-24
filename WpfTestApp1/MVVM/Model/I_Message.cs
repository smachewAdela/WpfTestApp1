﻿using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.Model
{
    public enum IMessageTypeEnum
    {
        Info,
        Error,
        Message
    }

    [DbEntity("SystemMessages")]
    public class I_Message : BaseDbItem
    {
        [DbField()]
        public string Title { get; set; }

        [DbField()]
        public string Message { get; set; }

        [DbField()]
        public string ExtraData { get; set; }

        [DbField()]
        public string IType { get; set; }

        [DbField()]
        public DateTime Date { get; set; }

        public IMessageTypeEnum MessageType
        {
            set
            {
                IType = value.ToString();
            }
        }

        [DbField()]
        public bool SendMail { get;  set; }

        public static I_Message Genertae(IMessageTypeEnum messageType)
        {
            return new I_Message
            {
                MessageType = messageType,
                Date = DateTime.Now
            };
        }
    }
}