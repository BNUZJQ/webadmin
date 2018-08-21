using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;

namespace Data.Model
{
    [XmlType(TypeName = "RATSupportBands")]
    public class CRATSet
    {
        private List<CRAT> m_strFormatList;
        [XmlElement("RAT")]
        public List<CRAT> M_strFormatList
        {
            get
            {
                return m_strFormatList;
            }
            set
            {
                m_strFormatList = value;
            }
        }
 
    }
    [XmlType(TypeName = "RAT")]
    public class CRAT
    {
        
        private String m_strRAT;
        [XmlAttribute(AttributeName = "Name")]
        public String M_strRAT
        {
            get
            {
                return m_strRAT;
            }
            set
            {
                m_strRAT = value;
            }
        }
        
        private UInt16 m_strIndex;
        [XmlAttribute(AttributeName = "Index")]
        public UInt16 M_strIndex
        {
            get
            {
                return m_strIndex;
            }
            set
            {
                m_strIndex = value;
            }
        }
        private String m_strRelease;
        [XmlAttribute(AttributeName = "Release")]
        public String M_strRelease
        {
            get
            {
                return m_strRelease;
            }
            set
            {
                m_strRelease = value;
            }
        }
        private String m_strBandWidth;
        [XmlAttribute(AttributeName = "BandWidth")]
        public String M_strBandWidth
        {
            get
            {
                return m_strBandWidth;
            }
            set
            {
                m_strBandWidth = value;
            }
        }

        private String m_strSCS;
        [XmlAttribute(AttributeName = "SCS")]
        public String M_strSCS
        {
            get
            {
                return m_strSCS;
            }
            set
            {
                m_strSCS = value;
            }
        }
        private String m_strPowerClass;
        [XmlAttribute(AttributeName = "PowerClass")]
        public String M_strPowerClass
        {
            get
            {
                return m_strPowerClass;
            }
            set
            {
                m_strPowerClass = value;
            }
        }
        private List<CBandInfo> m_BandList;
        [XmlElement("BandInfo")]
        //public List<CBandInfo> M_BandList
        public List<CBandInfo> M_BandList
        {
            get
            {
                return m_BandList;
            }
            set
            {
                m_BandList = value;
            }
        }

        private List<CPowerCtrLevInfo> m_PowerCtrLev;
        [XmlElement("PowerCtrLev")]
        //public List<CBandInfo> M_BandList
        public List<CPowerCtrLevInfo> M_PowerCtrLev
        {
            get
            {
                return m_PowerCtrLev;
            }
            set
            {
                m_PowerCtrLev = value;
            }
        }
    }
    [XmlType(TypeName = "BandInfo")]
    public class CBandInfo
    {
        private String m_strName;
        [XmlAttribute(AttributeName = "Name")]
        public String M_strName
        {
            get
            {
                return m_strName;
            }
            set
            {
                m_strName = value;
            }
        }
        private CFrequency m_Frequency;
        [XmlElement("Frequency")]
        public CFrequency M_Frequency
        {
            get
            {
                return m_Frequency;
            }
            set
            {
                m_Frequency = value;
            }
        }

        private CHisiInfo m_HisiInfo;
        [XmlElement("HisiInfo")]
        public CHisiInfo M_HisiInfo
        {
            get
            {
                return m_HisiInfo;
            }
            set
            {
                m_HisiInfo = value;
            }
        }


        private List<CBandWidth> m_BandWidth;
        [XmlElement("BandWidth")]
        public List<CBandWidth> M_BandWidth
        {
            get
            {
                if (this.M_BandWidthWithSCS != null)
                    if (this.M_BandWidthWithSCS.Count == 0)
                        return m_BandWidth;
                    else
                        return M_BandWidthWithSCS;
                else
                    return m_BandWidth;
            }
            set
            {
                m_BandWidth = value;
            }
        }
        
        private List<CBandWidth> m_BandWidthWithSCS;
        [XmlElement("BandWidthWithSCS")]
        public List<CBandWidth> M_BandWidthWithSCS
        {
            get
            {
                return m_BandWidthWithSCS;
            }
            set
            {
                m_BandWidthWithSCS = value;
            }
        }


        private CChanInfo m_ChanInfo;
        [XmlElement("ChanInfo")]
        public CChanInfo M_ChanInfo
        {
            get
            {
                return m_ChanInfo;
            }
            set
            {
                m_ChanInfo = value;
            }
        }

        private CPowerInfo m_PowerInfo;
        [XmlElement("PowerInfo")]
        public CPowerInfo M_PowerInfo
        {
            get
            {
                return m_PowerInfo;
            }
            set
            {
                m_PowerInfo = value;
            }
        }

    }
    [XmlType(TypeName = "Frequency")]
    public class CFrequency
    {
        private float m_FUlmin;
        [XmlAttribute(AttributeName = "UlMin")]
        public float M_FUlmin
        {
            get
            {
                return m_FUlmin;
            }
            set
            {
                m_FUlmin = value;
            }
        }
        private float m_FUlmax;
        [XmlAttribute(AttributeName = "UlMax")]
        public float M_FUlmax
        {
            get
            {
                return m_FUlmax;
            }
            set
            {
                m_FUlmax = value;
            }
        }
        private float m_FDlmin;
        [XmlAttribute(AttributeName = "DlMin")]
        public float M_FDlmin
        {
            get
            {
                return m_FDlmin;
            }
            set
            {
                m_FDlmin = value;
            }
        }
        private float m_FDlmax;
        [XmlAttribute(AttributeName = "DlMax")]
        public float M_FDlmax
        {
            get
            {
                return m_FDlmax;
            }
            set
            {
                m_FDlmax = value;
            }
        }
    }
    [XmlRoot("HisiInfo")]
    public class CHisiInfo
    {
        private UInt16 m_usIndex;
        [XmlAttribute(AttributeName = "Index")]
        public UInt16 M_usIndex
        {
            get
            {
                return m_usIndex;
            }
            set
            {
                m_usIndex = value;
            }
        }
        private String m_strDuplexMode;
        [XmlAttribute(AttributeName = "DuplexMode")]
        public String M_strDuplexMode
        {
            get
            {
                return m_strDuplexMode;
            }
            set
            {
                m_strDuplexMode = value;
            }
        }
        private String m_strIsSingleDL;
        [XmlAttribute(AttributeName = "IsSingleDL")]
        public String M_strIsSingleDL
        {
            get
            {
                return m_strIsSingleDL;
            }
            set
            {
                m_strIsSingleDL = value;
            }
        }
        private String m_strIsSingleUL;
        [XmlAttribute(AttributeName = "IsSingleUL")]
        public String M_strIsSingleUL
        {
            get
            {
                return m_strIsSingleUL;
            }
            set
            {
                m_strIsSingleUL = value;
            }
        }
    }

    [XmlRoot("BandWidth")]
    public class CBandWidth
    {
        private String m_strSCS;
        [XmlAttribute(AttributeName = "SCS")]
        public String M_strSCS
        {
            get
            {
                return m_strSCS;
            }
            set
            {
                m_strSCS=value;
            }
        }
        private String m_strBW;
        [XmlAttribute(AttributeName = "BW")]
        public String M_strBW
        {
            get
            {
                return m_strBW;
            }
            set
            {
                m_strBW = value;
            }
        }
    }
    [XmlRoot("ChanInfo")]
    public class CChanInfo
    {
        private UInt16 m_usChanSpacing;
        [XmlAttribute(AttributeName = "ChanSpacing")]
        public UInt16 M_usChanSpacing
        {
            get
            {
                return m_usChanSpacing;
            }
            set
            {
                m_usChanSpacing = value;
            }
        }
        [XmlAttribute(AttributeName = "StepSize")]
        public UInt16 m_usChanStepSize;
        public UInt16 M_usChanStepSize
        {
            get
            {
                if (m_usChanStepSize == 0)
                    return 1;
                else
                    return m_usChanStepSize;
            }
        }
        private UInt32 m_UUlChanMin;
        [XmlAttribute(AttributeName = "UlChanMin")]
        public UInt32 M_UUlChanMin
        {
            get
            {
                return m_UUlChanMin;
            }
            set
            {
                m_UUlChanMin = value;
            }
        }
        private UInt32 m_UUlChanMax;
        [XmlAttribute(AttributeName = "UlChanMax")]
        public UInt32 M_UUlChanMax
        {
            get
            {
                return m_UUlChanMax;
            }
            set
            {
                m_UUlChanMax = value;
            }
        }
        private UInt32 m_UDlChanMin;
        [XmlAttribute(AttributeName = "DlChanMin")]
        public UInt32 M_UDlChanMin
        {
            get
            {
                return m_UDlChanMin;
            }
            set
            {
                m_UDlChanMin = value;
            }
        }
        private UInt32 m_UDlChanMax;
        [XmlAttribute(AttributeName = "DlChanMax")]
        public UInt32 M_UDlChanMax
        {
            get
            {
                return m_UDlChanMax;
            }
            set
            {
                m_UDlChanMax = value;
            }
        }

    }
    [XmlType(TypeName = "PowerInfo")]
    public class CPowerInfo
    {
        private UInt16 m_usPCLIndex;
        [XmlAttribute(AttributeName = "PCLIndex")]
        public UInt16 M_usPCLIndex
        {
            get
            {
                return m_usPCLIndex;
            }
            set
            {
                m_usPCLIndex = value;
            }
        }
        private List<CPowerItem> m_PowerItem;
        [XmlElement("Item")]
        public List<CPowerItem> M_PowerItem
        {
            get
            {
                return m_PowerItem;
            }
            set
            {
                m_PowerItem = value;
            }
        }

    }
    [XmlRoot("Item")]
    public class CPowerItem
    {
        private String m_strClass;
        [XmlAttribute(AttributeName = "Class")]
        public String M_strClass
        {
            get
            {
                return m_strClass;
            }
            set
            {
                m_strClass = value;
            }
        }
        private float m_FMaxPower;
        [XmlAttribute(AttributeName = "MaxPower")]
        public float M_FMaxPower
        {
            get
            {
                return m_FMaxPower;
            }
            set
            {
                m_FMaxPower = value;
            }
        }
        private float m_FTolRight;
        [XmlAttribute(AttributeName = "TolRight")]
        public float M_FTolRight
        {
            get
            {
                return m_FTolRight;
            }
            set
            {
                m_FTolRight = value;
            }
        }
        private float m_FTolLeft;
        [XmlAttribute(AttributeName = "TolLeft")]
        public float M_FTolLeft
        {
            get
            {
                return m_FTolLeft;
            }
            set
            {
                m_FTolLeft = value;
            }
        }

        public String M_strTol
        {
            get
            {
                return "+" + M_FTolRight.ToString() + "/-" + M_FTolLeft.ToString();
            }
            set
            {

            }
        }
    }

    public class CPowerCtrLevInfo
    {
        private UInt16 m_usPCLIndex;
        [XmlAttribute(AttributeName = "PCLIndex")]
        public UInt16 M_usPCLIndex
        {
            get
            {
                return m_usPCLIndex;
            }
            set
            {
                m_usPCLIndex = value;
            }
        }
        private List<CPowerTable> m_PowerTable;
        [XmlElement("Table")]
        public List<CPowerTable> M_PowerTable
        {
            get
            {
                return m_PowerTable;
            }
            set
            {
                m_PowerTable = value;
            }
        }
    }
    public class CPowerTable
    {
        private String m_strPCL;
        [XmlAttribute(AttributeName = "PCL")]
        public String M_strPCL
        {
            get
            {
                return m_strPCL;
            }
            set
            {
                m_strPCL = value;
            }
        }
        private float m_FOutput;
        [XmlAttribute(AttributeName = "Output")]
        public float M_FOutput
        {
            get
            {
                return m_FOutput;
            }
            set
            {
                m_FOutput = value;
            }
        }
        private float m_FTolRight;
        [XmlAttribute(AttributeName = "TolRight")]
        public float M_FTolRight
        {
            get
            {
                return m_FTolRight;
            }
            set
            {
                m_FTolRight = value;
            }
        }
        private float m_FTolLeft;
        [XmlAttribute(AttributeName = "TolLeft")]
        public float M_FTolLeft
        {
            get
            {
                return m_FTolLeft;
            }
            set
            {
                m_FTolLeft = value;
            }
        }
        public String M_strTol
        {
            get
            {
                return "+" + M_FTolRight.ToString() + "/-" + M_FTolLeft.ToString();
            }
            set
            {

            }
        }
    }
    class XmlParse
    {
        public static T DeserializeFromXml<T>(string XmlPath)
        {
            try
            {
                if (!File.Exists(XmlPath))
                {
                    throw new ArgumentNullException(XmlPath + " not Exists");
                }
                using (StreamReader reader = new StreamReader(XmlPath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T ret = (T)xs.Deserialize(reader);
                    reader.Close();
                    return ret;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
                //return default(T);
            }
        }

        public static void SerializeToXml<T>(string filePath, T obj)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    xs.Serialize(writer, obj);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
