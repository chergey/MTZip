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

            try
            {
                new Coordinator(TestFileName + "g", TestFileName  + ".comp", Activity.Compress).Coordinate();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
 
            

        }

        [TestMethod]
        public void TestDecompress()
        {
           TestCompress();
            if (File.Exists(TestFileName))
            {
                File.Delete(TestFileName);
            }
            try
            {
                new Coordinator(TestFileName+".gzz", TestFileName + ".decomp", Activity.Decompress).Coordinate();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestCompressor()
        {
            byte[] data=new byte[1024*1024];
            using (var f = new FileStream("c:\\temp\\sub1.xml", FileMode.Open))
            {
                f.Read(data, 0, data.Length);
            }
            var comp = new Compressor(new FiFo.Chunk{Data = data, CheckPoint = 1},Activity.Compress);
            var compdata=comp.Do();

            using (var f = new FileStream("c:\\temp\\gzipped-data", FileMode.Create))
            {
                f.Write(compdata, 0, compdata.Length);
            }
        }

        
        [TestMethod]
        public void Test16GB()
        {

            try
            {
                new Coordinator("C:\\Temp\\HDP.VMDK", TestFileName + ".comp", Activity.Compress).Coordinate();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

    }
}
