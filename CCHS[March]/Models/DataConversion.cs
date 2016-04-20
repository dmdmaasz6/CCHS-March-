using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CCHS_March_.Models
{
    public class DataConversion
    {
        public byte[] fileToBytes(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            byte[] data = target.ToArray();

            return data;
        }


    }
}