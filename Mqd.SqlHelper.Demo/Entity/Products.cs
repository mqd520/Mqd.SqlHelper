using System;

namespace Mqd.SqlHelper.Entity
{
	 	//Products
		public class Products
	{
	
      	/// <summary>
		/// ProductID
        /// </summary>
        public virtual int ProductID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ProductName
        /// </summary>
        public virtual string ProductName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// SupplierID
        /// </summary>
        public virtual int SupplierID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CategoryID
        /// </summary>
        public virtual int CategoryID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// QuantityPerUnit
        /// </summary>
        public virtual string QuantityPerUnit
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
		/// UnitsInStock
        /// </summary>
        public virtual int UnitsInStock
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UnitsOnOrder
        /// </summary>
        public virtual int UnitsOnOrder
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ReorderLevel
        /// </summary>
        public virtual int ReorderLevel
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Discontinued
        /// </summary>
        public virtual bool Discontinued
        {
            get; 
            set; 
        }        
		   
	}
}