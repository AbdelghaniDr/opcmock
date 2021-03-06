﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpcMock;

namespace OpcMockTests
{
    [TestClass]
    public class ProtocolTests
    {
        private const int NEW_PROTOCOL_LINE_COUNT = 0;
        private const string PROTOCOL_NAME = "protocolName";

        [TestMethod]
        public void New_Protocol_Has_No_Lines()
        {
            OpcMockProtocol omp = new OpcMockProtocol(PROTOCOL_NAME);

            Assert.AreEqual(NEW_PROTOCOL_LINE_COUNT, omp.Lines.Count);
        }

        [TestMethod]
        public void New_Protocol_Is_Created_With_A_Name()
        {
            OpcMockProtocol omp = new OpcMockProtocol(PROTOCOL_NAME);

            Assert.AreEqual(PROTOCOL_NAME, omp.Name);
        }

        [TestMethod]
        public void Add_Line_Appends_A_Line_At_The_End()
        {
            OpcMockProtocol omp = new OpcMockProtocol(PROTOCOL_NAME);

            string line1 = "Set;tagPath1;tagValue1;192";
            string lineEqualToLine1 = "Set;tagPath2;tagValue2;192";

            omp.Append(new ProtocolLine(line1));
            omp.Append(new ProtocolLine(lineEqualToLine1));

            ///PROPOSAL expose IEnumberable instead of List
            Assert.AreEqual(new ProtocolLine(lineEqualToLine1), omp.Lines[1]);
        }

        [TestMethod]
        public void Equality_Operator_Works_Based_On_Protocol_Name()
        {
            Assert.IsTrue(new OpcMockProtocol(PROTOCOL_NAME).Equals(new OpcMockProtocol(PROTOCOL_NAME)));
        }

        [TestMethod]
        public void ToString_Return_Name()
        {
            OpcMockProtocol omp = new OpcMockProtocol(PROTOCOL_NAME);

            Assert.AreEqual(PROTOCOL_NAME, omp.ToString());
        }

        /// PROPOSAL - parameterize test ==> NUnit
        /// 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Empty name should not be accepted")]
        public void Empty_Or_SpacesOnly_Name_Raises_ArgumentException()
        {
            OpcMockProtocol omp = new OpcMockProtocol(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Blanks-only name should not be accepted")]
        public void OpcMockProtocolConstructor_Should_Raise_ArgumentException_For_BlanksOnly_Name()
        {
            OpcMockProtocol omp = new OpcMockProtocol("    ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Tabs-only name should not be accepted")]
        public void OpcMockProtocolConstructor_Should_Raise_ArgumentException_For_TabsOnly_Name()
        {
            OpcMockProtocol omp = new OpcMockProtocol("\t");
        }

        [TestMethod]
        public void Adding_A_Line_Raises_LineAdded_Event()
        {
            bool eventRaised = false;

            OpcMockProtocol protocol = new OpcMockProtocol(PROTOCOL_NAME);

            protocol.OnProtocolLineAdded += delegate (object sender, ProtocolLineAddedArgs plaArgs) { eventRaised = true; };

            protocol.Append(new ProtocolLine("Set;tagPath;tagValue;192"));

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void Append_For_StringArray_Appends_All_Lines()
        {
            OpcMockProtocol protocol = new OpcMockProtocol(PROTOCOL_NAME);

            string[] testArray = new string[] { "Set; tagPath1; tagValue; 192", "Set;tagPath2;tagValue;192", "Set;tagPath3;tagValue;192" };

            protocol.Append(testArray);

            Assert.AreEqual(new ProtocolLine("Set;tagPath1;tagValue;192"), protocol.Lines[0]);
            Assert.AreEqual(new ProtocolLine("Set;tagPath2;tagValue;192"), protocol.Lines[1]);
            Assert.AreEqual(new ProtocolLine("Set;tagPath3;tagValue;192"), protocol.Lines[2]);
        }
    }
}
