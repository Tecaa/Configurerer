using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PrepareEventArgs : EventArgs
{
   public PrepareStatus status { get; private set; }
   public Caller caller { get; private set; }
   public PrepareEventArgs(PrepareStatus status, Caller caller) : base()
   {
          this.status = status;
          this.caller = caller;
   }
}
