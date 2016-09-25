using System;

namespace Mqd.SqlHelper.Entity
{
	 	//Shippers
		public class Shippers
	{
	
      	/// <summary>
		/// ShipperID
        /// </summary>
        public virtual int ShipperID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CompanyName
        /// </summary>
        public virtual string CompanyName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Phone
        /// </summary>
        public virtual string Phone
        {
            get; 
            set; 
        }        
		   
	}
}