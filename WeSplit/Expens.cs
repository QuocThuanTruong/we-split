//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeSplit
{
    using System;
    using System.Collections.Generic;
    
    public partial class Expens
    {
        public int ID_Expenses { get; set; }
        public Nullable<int> ID_Journey { get; set; }
        public Nullable<decimal> Expenses_Money { get; set; }
        public string Expenses_Description { get; set; }
        public Nullable<int> Is_Active { get; set; }
    
        public virtual Journey Journey { get; set; }
    }
}
