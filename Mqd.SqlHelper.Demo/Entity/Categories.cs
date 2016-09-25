using System;

namespace Mqd.SqlHelper.Entity
{
	 	//Categories
		public class Categories
	{
	
      	/// <summary>
		/// CategoryID
        /// </summary>
        public virtual int CategoryID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CategoryName
        /// </summary>
        public virtual string CategoryName
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Description
        /// </summary>
        public virtual string Description
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Picture
        /// </summary>
        public virtual byte[] Picture
        {
            get; 
            set; 
        }        
		   
	}
}