using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Beaver_ML
{
    public class Beaver_MLInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "BeaverML";
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
                return new Guid("ef70c791-7452-4d44-b66a-dcfc4ab42965");
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
