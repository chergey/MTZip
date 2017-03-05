using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestTask.Enums;
using TestTask.Imp;

namespace TestTask
{
    [TestClass]
    public class Test
    {
        private const string TestFileName = "textfile.txt";


        [TestMethod]
        public void TestCompress()
        {

            var random=new Random();

            var bytes = new byte[1000000];
            random.NextBytes(bytes);

            using (var fs = new FileStream(TestFileName, FileMode.Create))
            {
                fs.Write(bytes,0,bytes.Length);
            }

            var cor = new Coordinator(TestFileName, TestFileName + ".comp", Activity.Compress);
            Assert.IsTrue(cor.Coordinate());
 
            

        }

        [TestMethod]
        public void TestDecompress()
        {
           TestCompress();
            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }
            var cor = new Coordinator(TestFileName + ".comp", TestFileName + ".decomp", Activity.Decompress);
            Assert.IsTrue(cor.Coordinate());
        }
    }
}
