using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Beaver
{
    public class BeaverInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Beaver";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("b83ea37e-0f59-42fc-a1bd-6f77cc161bcd");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
