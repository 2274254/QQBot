using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Event.Interface {
    public interface ICancelable {
        bool IsCanceled();
        bool IsCancelable();
        bool SetCancel();
    }
}
