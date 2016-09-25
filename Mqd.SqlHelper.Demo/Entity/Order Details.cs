using System;

namespace Mqd.SqlHelper.Entity
{
	 	//Order Details
		public class OrderDetails
	{
	
      	/// <summary>
		/// OrderID
        /// </summary>
        public virtual int OrderID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ProductID
        /// </summary>
        public virtual int ProductID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UnitPrice
        /// </summary>
        public virtual decimal UnitPrice
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Quantity
        /// </summary>
        public virtual int Quantity
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Discount
        /// </summary>
        public virtual decimal Discount
        {
            get; 
            set; 
        }        
		   
	}
}