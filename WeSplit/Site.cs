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
    
    public partial class Site
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Site()
        {
            this.Journeys = new HashSet<Journey>();
        }
    
        public int ID_Site { get; set; }
        public int ID_Province { get; set; }
        public string Site_Name { get; set; }
        public string Site_Description { get; set; }
        public string Site_Link_Avt { get; set; }
        public string Site_Address { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Journey> Journeys { get; set; }
        public virtual Province Province { get; set; }

        //For Binding
        public double Distance { get; set; }
        public int ID_Journey { get; set; }
    }
}