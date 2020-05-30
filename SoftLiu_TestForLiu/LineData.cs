using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_TestForLiu
{
    public class LineData
    {
        public bool isPortID = false;

        public string portID;

        public string portName;

        public string portFlag;

        public string sendFlag;

        public int lineIndex;


        public LineData(bool isPort, string id, string name, string pF, string sF, int index, bool sendFlagChange = false)
        {
            isPortID = isPort;
            portID = id;
            portName = name;
            portFlag = pF;
            sendFlag = sF;
            if (!string.IsNullOrEmpty(sendFlag) && sendFlagChange)
            {
                if (sendFlag == "0") sendFlag = "1";
                else if (sendFlag == "1") sendFlag = "0";
            }
            lineIndex = index;
        }

        public override string ToString()
        {
            if (!isPortID)
                return portName;
            return string.Format("{0}={1}-{2}-{3}", portName, portFlag, portID, sendFlag);
        }
    }
}
